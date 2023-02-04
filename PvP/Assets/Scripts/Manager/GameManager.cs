using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Boolean StartWithBuildingMode;

    private RessourceManager ressourceManager;

    private List<System.Object> buildings; //TODO Buildings Klasse

    private Boolean isBuildingMode;

    // Start is called before the first frame update
    void Start()
    {
        ressourceManager = GameObject.FindGameObjectWithTag("RessourceManager").GetComponent<RessourceManager>();
        isBuildingMode = StartWithBuildingMode;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
