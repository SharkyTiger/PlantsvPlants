using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.Tilemaps;
using static UnityEngine.GraphicsBuffer;
using System.Linq;
using UnityEngine.SceneManagement;
using static UnityEngine.EventSystems.EventTrigger;
using Unity.VisualScripting;

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
    private GameObject mainBuilding;
    public Tilemap map;

    private RessourceManager ressourceManager;
    private BuildingsManager buildingsManager;

    private List<GameObject> buildings = new List<GameObject>();

    private Int32[] SpawnerCost = {100,100};
    private Int32[] WaterMineCost = {100,0};
    private Int32[] FertilizerMineCost = {0,100};
    private Int32 currentSpawnerId = -1;

    // Start is called before the first frame update
    void Start()
    {
        ressourceManager = RessourceManagerObject?.GetComponent<RessourceManager>();
        buildingsManager = BuildingsManagerObject?.GetComponent<BuildingsManager>();

        mainBuilding = Instantiate(MainBuildingPrefab, new Vector3(-2, 0, -1), Quaternion.identity);

        //Testcode
        SpawnBattleUnit(new Vector3(-10, 0, -1), Team.Team1, Color.red, -1);
        SpawnBattleUnit(new Vector3(10, -5, -1), Team.Team2, Color.blue, -1);
    }

    // Update is called once per frame
    void Update()
    {
        ShootBulletsTeam1();
        ShootBulletsTeam2();
        if (!Team2BattleUnits.Any())
        {
            SceneManager.LoadScene("VictoryScene");
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
        if (hitData.collider == null)
        {
            return null;
        }
        Debug.Log(hitData.collider.gameObject.name);
        GameObject hitObject = hitData.transform.gameObject;
        hitObject.TryGetComponent<BattleUnit>(out var unitData);
        if (unitData != null)
        {

        }

        if (!hitObject.tag.ToLower().Equals("ignorebyraycast"))
        {
            return hitObject;
        }

        return null; //TODO
    }

    public void DestroyBuilding(GameObject building)
    {
        if(buildings.Contains(building))
        {
            buildings.Remove(building);
            var script = building.GetComponent<Building>();
            if (true) ressourceManager.DestroyWaterMine();
            if (true) ressourceManager.DestroyFertilizerMine();
        }
    }

    public void CreateBuilding(GameObject building)
    {
        if (building == null) return;

        buildings.Add(building);
        var buildingScript = building.GetComponent<Building>();
        var kind = buildingScript.Kind;
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

    public void SpawnBattleUnit(Vector3 position, Team team, Color color, Int32 spawnerId)
    {
        var unit = Instantiate(BattleunitPrefab, position, Quaternion.identity);
        var script = unit.GetComponent<BattleUnit>();
        script.Spawn(team, color, 5, 1, 1f, spawnerId);
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
    }
}
