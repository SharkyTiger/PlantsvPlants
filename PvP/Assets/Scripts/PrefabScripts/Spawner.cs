using System;

public class Spawner : Building
{
    public Int32 Id;
    public Spawner(BuildingKind kind) : base(kind)
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        MaxCooldown = Cooldown;
    }

    private void FixedUpdate()
    {
        Cooldown--;
        if (Cooldown <= 0)
        {
            var test = this.transform.position;
            test.x -= 2;
            test.z = -1;
            Manager.SpawnBattleUnit(test, TeamNumber, TeamColor, Id, test);
            Cooldown = MaxCooldown;
        }
    }
}
