using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    // Start is called before the first frame update
    public Int32 Health;
    public Int32 MaxHealth;
    public Int32 Level;
    public BuildingKind Kind;
    public Building(BuildingKind kind)
    {
        Kind = kind;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
public enum BuildingKind
{
    Spawner,
    WaterMine,
    FertilizerMine
}
