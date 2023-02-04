using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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
    public Tilemap map;

    private RessourceManager ressourceManager;
    private BuildingsManager buildingsManager;

    private List<GameObject> buildings;

    private Boolean isBuildingMode;

    // Start is called before the first frame update
    void Start()
    {
        ressourceManager = RessourceManagerObject.GetComponent<RessourceManager>();
        buildingsManager = BuildingsManagerObject.GetComponent<BuildingsManager>();
        isBuildingMode = StartWithBuildingMode;

        //Testcode
        SpawnBattleUnit(new Vector3(-10, 0, -1), Team.Team1, Color.red);
        SpawnBattleUnit(new Vector3(0, -5, -1), Team.Team2, Color.blue);
    }

    // Update is called once per frame
    void Update()
    {
        
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
        isBuildingMode = true;
        buildingsManager.EnterBuildMode(BuildingKind.Spawner);
    }

    public void CreateWaterMine()
    {
        isBuildingMode = true;
        buildingsManager.EnterBuildMode(BuildingKind.WaterMine);
    }

    public void CreateFertilizerMine()
    {
        isBuildingMode = true;
        buildingsManager.EnterBuildMode(BuildingKind.FertilizerMine);
    }

    public GameObject GetGameObjectFromPosition(Vector3 position)
    {
        /*GameObject[] allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        foreach (var obj in allObjects)
        {
            float dist = Vector3.Distance(position, obj.transform.position);
            if (dist < 1 && obj != highlight)
            {
                return obj;
            }
        }
        return null;*/
        return null; //TODO
    }

    public void DestroyBuilding(GameObject building) //TODO Buildings Klasse
    {
        if(buildings.Contains(building))
        {
            buildings.Remove(building);
            var script = building.GetComponent<Building>();
            if (true) ressourceManager.DestroyWaterMine();//TODO Wassermine Klasse
            if (true) ressourceManager.DestroyFertilizerMine();//TODO Düngermine Klasse
        }
    }

    public void CreateBuilding(GameObject building)
    {
        buildings.Add(building);
        var buildingsKind = building.GetComponent<Building>().Kind;
        if (buildingsKind == BuildingKind.WaterMine) ressourceManager.AddWaterMine();
        if (buildingsKind == BuildingKind.FertilizerMine) ressourceManager.AddFertilizerMine();
    }

    public Boolean IsBuildingMode() => isBuildingMode;

    public void SetBuildingMode(Boolean isBuilding)
    {
        isBuildingMode = isBuilding;
    }

    public void SpawnBattleUnit(Vector3 position, Team team, Color color)
    {
        var unit = Instantiate(BattleunitPrefab, position, Quaternion.identity);
        unit.GetComponent<BattleUnit>().Spawn(team, color, 5, 1, 1f);
        switch(team)
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
}
