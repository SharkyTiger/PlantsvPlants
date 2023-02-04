using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UIElements;

using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour
{
    public Boolean StartWithBuildingMode;
    public GameObject BattleunitPrefab;
    public List<GameObject> Team1BattleUnits;
    public List<GameObject> Team2BattleUnits;
    public List<GameObject> Team3BattleUnits;
    public List<GameObject> Team4BattleUnits;

    private RessourceManager ressourceManager;

    private List<System.Object> buildings; //TODO Buildings Klasse

    private Boolean isBuildingMode;

    // Start is called before the first frame update
    void Start()
    {
        ressourceManager = GameObject.FindGameObjectWithTag("RessourceManager")?.GetComponent<RessourceManager>();
        isBuildingMode = StartWithBuildingMode;

        //Testcode
        SpawnBattleUnit(new Vector3(-10, 0, -1), Team.Team1, Color.red);
        SpawnBattleUnit(new Vector3(0, -5, -1), Team.Team2, Color.blue);
    }

    // Update is called once per frame
    void Update()
    {
        ShootBulletsTeam1();
        ShootBulletsTeam2();
    }

    public void DestroyBuilding(System.Object building) //TODO Buildings Klasse
    {
        if(buildings.Contains(building))
        {
            buildings.Remove(building);
            if (building is String) ressourceManager.DestroyWaterMine();//TODO Wassermine Klasse
            if (building is String) ressourceManager.DestroyFertilizerMine();//TODO Düngermine Klasse
        }
    }

    public void CreateBuilding(System.Object building) //TODO Buildings Klasse
    {
        buildings.Add(building);
        if (building is String) ressourceManager.AddWaterMine();//TODO Wassermine Klasse
        if (building is String) ressourceManager.AddFertilizerMine();//TODO Düngermine Klasse
    }

    public Boolean IsBuildingMode() => isBuildingMode;

    public void SetBuildingMode(Boolean isBuilding)
    {
        isBuildingMode = isBuilding;
    }

    public void SpawnBattleUnit(Vector3 position, Team team, Color color)
    {
        var unit = Instantiate(BattleunitPrefab, position, Quaternion.identity);
        unit.GetComponent<BattleUnit>().Spawn(team, color, 5, 1, 1f, -1);
        unit.GetComponent<BattleUnit>().DestroyedEvent += RemoveBattleUnitOnDestroy;
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
