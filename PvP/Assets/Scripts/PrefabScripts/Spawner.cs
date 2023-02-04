using System;
using UnityEngine;

public class Spawner : Building
{
    Int32 Tick;
    public Int32 Id;
    public Spawner(BuildingKind kind) : base(kind)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        Tick = 0;
    }

    // Update is called once per frame
    void Update()
    {
        Tick++;
        Tick %= Cooldown;
        if (Tick == 0) 
        {
            var test = this.transform.position;
            test.x -=  2;
            test.z = -1;
            Manager.SpawnBattleUnit(test, TeamNumber, TeamColor, Id);
        }
    }
}
