using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Boolean StartWithBuildingMode;
    public GameObject BattleunitPrefab;
    public List<GameObject> Team1BattleUnits;
    public List<GameObject> Team2BattleUnits;
    public List<GameObject> Team3BattleUnits;
    public List<GameObject> Team4BattleUnits;
    public GameObject BuildingsManagerObject;
    public GameObject RessourceManagerObject;
    public GameObject MainBuildingPrefab;
    public GameObject SelectionMarker;
    private GameObject mainBuilding;
    public Tilemap map;

    private RessourceManager ressourceManager;
    private BuildingsManager buildingsManager;

    private List<GameObject> buildings = new List<GameObject>();

    private Int32[] SpawnerCost = { 100, 100 };
    private Int32[] WaterMineCost = { 100, 0 };
    private Int32[] FertilizerMineCost = { 0, 100 };
    private Int32 currentSpawnerId = -1;
    private Vector3[] spawnPositions = { new Vector3(-95, 70, -1), new Vector3(110, 70, -1), new Vector3(-95, -85, -1), new Vector3(110, -85, -1) };
    private Int32 survivedWaves = 0;
    private Boolean isSelectionMode = false;
    private Vector3 originalSelectionPos;
    private BoxCollider2D selectionCollider;

    private List<GameObject> selectedUnits;

    private void Awake()
    {
        selectedUnits = new List<GameObject>();
    }
    private float timeUntilNextWave;
    private float startTimeUntilWave = 60;
    public TMP_Text TimeUntilNextWaveText;
    private GameObject selectedUnit;

    // Start is called before the first frame update
    void Start()
    {
        ressourceManager = RessourceManagerObject?.GetComponent<RessourceManager>();
        buildingsManager = BuildingsManagerObject?.GetComponent<BuildingsManager>();

        selectionCollider = SelectionMarker.GetComponent<BoxCollider2D>();

        mainBuilding = Instantiate(MainBuildingPrefab, new Vector3(0, 0, -1), Quaternion.identity);
        mainBuilding.GetComponent<MainBuilding>().SetValues(Team.Team1, Color.magenta, 20);
        timeUntilNextWave = startTimeUntilWave;
        //Secret "Boss" Unit
        SpawnBossUnit(new Vector3(113, -88, -1), Team.Team2, Color.black, -1, new Vector3(113, -88, -1));
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("MainMenu");
        }
        if (timeUntilNextWave <= 0f)
        {
            SpawnEnemyWave(10 * (survivedWaves + 1), Math.Min(Math.Min(survivedWaves / 4, 1), 4));
            timeUntilNextWave = startTimeUntilWave;
        }
        ShootBulletsTeam1();
        ShootBulletsTeam2();
        if (!Team2BattleUnits.Any())
        {
            SceneManager.LoadScene("VictoryScene");
        }
        CheckUnitSelection();
    }

    private void FixedUpdate()
    {
        timeUntilNextWave -= Time.deltaTime;
        TimeUntilNextWaveText.text = $"{timeUntilNextWave:0}";
    }

    public void SpawnEnemyWave(Int32 numberOfEnemies, Int32 numberOfCorners = 1)
    {
        for (var i = 0; i < numberOfEnemies; i++)
        {
            var selectedPostion = Random.Range(1, numberOfCorners);
            SpawnBattleUnit(spawnPositions[selectedPostion - 1], Team.Team2, Color.blue, -1, mainBuilding.transform.position);
        }
    }

    public Vector3Int GetMouseToWorldPos()
    {
        Vector3 vec3 = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        vec3.x += 0.5f;
        vec3.y += 0.5f;
        return map.LocalToCell(vec3);
    }

    public void CreateSpawner()
    {
        if (!ressourceManager.HasEnough(SpawnerCost[0], SpawnerCost[1])) return;
        buildingsManager.EnterBuildMode(BuildingKind.Spawner);
    }

    public void CreateWaterMine()
    {
        if (!ressourceManager.HasEnough(WaterMineCost[0], WaterMineCost[1])) return;
        buildingsManager.EnterBuildMode(BuildingKind.WaterMine);
    }

    public void CreateFertilizerMine()
    {
        if (!ressourceManager.HasEnough(FertilizerMineCost[0], FertilizerMineCost[1])) return;
        buildingsManager.EnterBuildMode(BuildingKind.FertilizerMine);
    }

    public Int32 GetNextSpawnerId()
    {
        currentSpawnerId++;
        return currentSpawnerId;
    }

    public GameObject GetGameObjectFromPosition(Vector3 position)
    {
        var hitData = Physics2D.Raycast(position, Vector2.zero);
        if (hitData.collider == null || hitData.collider.gameObject.tag.ToLower().Equals("obstacle"))
        {
            return null;
        }
        GameObject hitObject = hitData.transform.gameObject;

        if (!hitObject.tag.ToLower().Equals("ignorebyraycast"))
        {
            return hitObject;
        }

        return null;
    }

    public void DestroyBuilding(BuildingKind buildingKind, GameObject building)
    {
        if (buildings.Contains(building))
        {
            switch (buildingKind)
            {
                case BuildingKind.WaterMine:
                    ressourceManager.DestroyWaterMine();
                    break;
                case BuildingKind.FertilizerMine:
                    ressourceManager.DestroyFertilizerMine();
                    break;
                case BuildingKind.Spawner:
                    var spawnerId = building.GetComponent<Spawner>().Id;
                    foreach (var unit in Team1BattleUnits)
                    {
                        if (unit.GetComponent<BattleUnit>().SpawnerBuildingId == spawnerId)
                        {
                            Destroy(unit);
                        }
                    }
                    break;
            }

            buildings.Remove(building);
        }
    }

    public void CreateBuilding(GameObject building)
    {
        if (building == null) return;

        buildings.Add(building);
        var buildingScript = building.GetComponent<Building>();
        var kind = buildingScript.Kind;
        buildingScript.DestroyedEvent += RemoveBuildingsOnDestruction;
        var cost = new Int32[] { 0, 0 };
        switch (kind)
        {
            case BuildingKind.Spawner:
                cost = SpawnerCost;
                break;
            case BuildingKind.WaterMine:
                ressourceManager.AddWaterMine();
                cost = WaterMineCost;
                break;
            case BuildingKind.FertilizerMine:
                ressourceManager.AddFertilizerMine();
                cost = FertilizerMineCost;
                break;
        }
        ressourceManager.DeductRessourceCost(cost[0], cost[1]);
    }

    public void SpawnBattleUnit(Vector3 position, Team team, Color color, Int32 spawnerId, Vector3 moveToPosition)
    {
        var unit = Instantiate(BattleunitPrefab, position, Quaternion.identity);
        var script = unit.GetComponent<BattleUnit>();
        script.Spawn(team, color, 5, 1, 1f, spawnerId);
        if (position != moveToPosition)
        {
            script.SetDestination(moveToPosition);
        }
        script.DestroyedEvent += RemoveBattleUnitOnDestroy;
        switch (team)
        {
            case Team.Team1:
                Team1BattleUnits.Add(unit);
                break;
            case Team.Team2:
                Team2BattleUnits.Add(unit);
                break;
            case Team.Team3:
                Team3BattleUnits.Add(unit);
                break;
            case Team.Team4:
                Team4BattleUnits.Add(unit);
                break;
        }
    }

    public void SpawnBossUnit(Vector3 position, Team team, Color color, Int32 spawnerId, Vector3 moveToPosition)
    {
        var unit = Instantiate(BattleunitPrefab, position, Quaternion.identity);
        var script = unit.GetComponent<BattleUnit>();
        script.Spawn(team, color, 20, 2, 1f, spawnerId);
        if (position != moveToPosition)
        {
            script.SetDestination(moveToPosition);
        }
        script.DestroyedEvent += RemoveBattleUnitOnDestroy;
        switch (team)
        {
            case Team.Team1:
                Team1BattleUnits.Add(unit);
                break;
            case Team.Team2:
                Team2BattleUnits.Add(unit);
                break;
            case Team.Team3:
                Team3BattleUnits.Add(unit);
                break;
            case Team.Team4:
                Team4BattleUnits.Add(unit);
                break;
        }
    }

    public void ShootBulletsTeam1()
    {
        var triggerDistance = 8f;

        foreach (var unit in Team1BattleUnits)
        {
            foreach (var enemy in Team2BattleUnits)
            {
                if (triggerDistance >= Vector3.Distance(unit.transform.position, enemy.transform.position))
                {
                    unit.GetComponent<BattleUnit>()?.ShootBullet(enemy.transform);
                    break;
                }
            }
            foreach (var enemy in Team3BattleUnits)
            {
                if (triggerDistance >= Vector3.Distance(unit.transform.position, enemy.transform.position))
                {
                    unit.GetComponent<BattleUnit>()?.ShootBullet(enemy.transform);
                    break;
                }
            }
            foreach (var enemy in Team4BattleUnits)
            {
                if (triggerDistance >= Vector3.Distance(unit.transform.position, enemy.transform.position))
                {
                    unit.GetComponent<BattleUnit>()?.ShootBullet(enemy.transform);
                    break;
                }
            }
        }
    }

    public void ShootBulletsTeam2()
    {
        var triggerDistance = 8f;

        foreach (var unit in Team2BattleUnits)
        {
            foreach (var enemy in Team1BattleUnits)
            {
                if (triggerDistance >= Vector3.Distance(unit.transform.position, enemy.transform.position))
                {
                    unit.GetComponent<BattleUnit>()?.ShootBullet(enemy.transform);
                    break;
                }
            }
            foreach (var enemy in Team3BattleUnits)
            {
                if (triggerDistance >= Vector3.Distance(unit.transform.position, enemy.transform.position))
                {
                    unit.GetComponent<BattleUnit>()?.ShootBullet(enemy.transform);
                    break;
                }
            }
            foreach (var enemy in Team4BattleUnits)
            {
                if (triggerDistance >= Vector3.Distance(unit.transform.position, enemy.transform.position))
                {
                    unit.GetComponent<BattleUnit>()?.ShootBullet(enemy.transform);
                    break;
                }
            }

            if (!mainBuilding.IsUnityNull() && triggerDistance >= Vector3.Distance(unit.transform.position, mainBuilding.transform.position))
            {
                unit.GetComponent<BattleUnit>()?.ShootBullet(mainBuilding.transform);
                break;
            }
            if (buildings.Any())
            {
                foreach (var enemy in buildings)
                {
                    if (triggerDistance >= Vector3.Distance(unit.transform.position, enemy.transform.position))
                    {
                        unit.GetComponent<BattleUnit>()?.ShootBullet(enemy.transform);
                        break;
                    }
                }
            }
        }
    }

    public void RemoveBuildingsOnDestruction(object sender, Building.DestructionEventArgs args)
    {
        if (args.Team == Team.Team1)
        {
            DestroyBuilding(args.BuildingKind, args.Building);
        }
    }

    public void RemoveBattleUnitOnDestroy(object sender, BattleUnit.DeathEventArgs args)
    {
        switch (args.Team)
        {
            case Team.Team1:
                Team1BattleUnits.Remove(args.BattleUnit);
                break;
            case Team.Team2:
                Team2BattleUnits.Remove(args.BattleUnit);
                break;
            case Team.Team3:
                Team3BattleUnits.Remove(args.BattleUnit);
                break;
            case Team.Team4:
                Team4BattleUnits.Remove(args.BattleUnit);
                break;
        }

        if (args.Team != Team.Team1 && (Team2BattleUnits.Count + Team3BattleUnits.Count + Team4BattleUnits.Count <= 1))
        {
            survivedWaves += 1;
        }
    }

    private void CheckUnitSelection()
    {
        if (buildingsManager.IsInBuildMode())
        {
            if (isSelectionMode) EndMultiSelect();
            return;
        }

        var mouse0click = Input.GetMouseButtonDown(0);
        var mouse0hold = Input.GetMouseButton(0);
        var mouse0up = Input.GetMouseButtonUp(0);

        if (!mouse0hold && Input.GetMouseButtonDown(1) && !isSelectionMode)
        {
            SendSelectedUnitTo(GetMouseToWorldPos());
            return;
        }

        if ((mouse0hold && !mouse0click) || isSelectionMode)
        {
            if(!isSelectionMode)
            {
                StartMultiSelect();
                return;
            }

            if (mouse0hold)
            {
                ContinueMultiSelect();
                return;
            }

            if (mouse0up)
            {
                EndMultiSelect();
                return;
            }
        }
    }

    private void StartMultiSelect()
    {
        SelectionMarker.SetActive(true);
        var pos = (Vector3)GetMouseToWorldPos();

        var renderer= SelectionMarker.GetComponent<LineRenderer>();
        renderer.positionCount = 4;
        renderer.SetPositions(new Vector3[] { pos, pos, pos, pos });

        selectionCollider.size = new Vector2(0, 0);

        originalSelectionPos = pos;
        isSelectionMode = true;
    }

    private void ContinueMultiSelect()
    {
        var pos = (Vector3)GetMouseToWorldPos();

        var renderer = SelectionMarker.GetComponent<LineRenderer>();

        renderer.SetPosition(0, originalSelectionPos);
        renderer.SetPosition(1, new Vector3(originalSelectionPos.x, pos.y));
        renderer.SetPosition(2, pos);
        renderer.SetPosition(3, new Vector3(pos.x, originalSelectionPos.y));

        SelectionMarker.transform.position = (pos + originalSelectionPos) / 2;

        selectionCollider.size = new Vector2(
            Math.Abs(originalSelectionPos.x - pos.x),
            Math.Abs(originalSelectionPos.y - pos.y));
    }

    private void EndMultiSelect()
    {
        Deselect();
        var colliders = new Collider2D[Team1BattleUnits.Count + Team2BattleUnits.Count + Team3BattleUnits.Count + Team4BattleUnits.Count];
        selectionCollider.OverlapCollider(new ContactFilter2D(), colliders);

        var realColliders = colliders.Where(x => x != null && x.gameObject != null
                                                && x.gameObject.GetComponent<BattleUnit>() != null
                                                && x.gameObject.GetComponent<BattleUnit>().Team == Team.Team1)
                                    .Select(x => x.gameObject).ToList();
        var currentUnit = GetUnitOnCurrentMouse();
        if (currentUnit != null) realColliders.Add(currentUnit);

        foreach (var rc in realColliders)
        {
            rc.GetComponent<BattleUnit>().ToggleHighlight();
            selectedUnits.Add(rc.gameObject);
        }

        isSelectionMode = false;
        SelectionMarker.GetComponent<LineRenderer>().positionCount = 0;
    }

    private void Deselect()
    {
        foreach (var unit in selectedUnits)
        {
            unit.GetComponent<BattleUnit>().ToggleHighlight();
        }

        selectedUnits = new List<GameObject>();
    }

    private GameObject GetUnitOnCurrentMouse()
    {
        var pos = (Vector3)GetMouseToWorldPos();
        var hitData = Physics2D.Raycast(pos, Vector2.zero);

        if (hitData.transform == null)
        {
            return null;
        }
        GameObject hitObject = hitData.transform.gameObject;

        Deselect();

        var battleUnit = hitObject.GetComponent<BattleUnit>();
        if (battleUnit == null)
        {
            return null;
        }

        return hitObject;
    }

    private void SendSelectedUnitTo(Vector3 pos)
    {
        if (selectedUnits.Count == 0) return;
        var averagePos = GetAvaerageSelectedPoint();
        var movement = pos - averagePos;
        foreach(var unit in selectedUnits)
        {
            unit.GetComponent<BattleUnit>().SetDestination(unit.transform.position + movement);
        }
    }

    private Vector3 GetAvaerageSelectedPoint()
    {
        var x = 0f;
        var y = 0f;
        foreach(var unit in selectedUnits)
        {
            x += unit.transform.position.x;
            y += unit.transform.position.y;
        }
        return new Vector3((Int32)x / selectedUnits.Count, (Int32)y / selectedUnits.Count);
    }
}
