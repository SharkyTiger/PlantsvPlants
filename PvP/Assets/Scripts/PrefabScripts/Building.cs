using System;

using Unity.VisualScripting;

using UnityEngine;

public class Building : DamageableBuilding
{
    // Start is called before the first frame update
    public Int32 Cooldown;
    public Int32 MaxCooldown;
    public BuildingKind Kind;
    public Team TeamNumber;
    public Color TeamColor;
    public GameManager Manager;
    public event EventHandler<DestructionEventArgs> DestroyedEvent;

    public Building(BuildingKind kind)
    {
        Kind = kind;
        Team = Team.Team1;
    }

    protected override void OnBeforeDestruction()
    {

    }

    private void OnDestroy()
    {
        if (this.IsUnityNull())
        {
            return;
        }
        DestroyedEvent.Invoke(this, new DestructionEventArgs(Team, Kind, this.gameObject));
    }

    public class DestructionEventArgs : EventArgs
    {
        public Team Team;
        public GameObject Building;
        public BuildingKind BuildingKind;

        public DestructionEventArgs(Team team, BuildingKind buildingKind, GameObject building)
        {
            Team = team;
            Building = building;
            BuildingKind = buildingKind;
        }
    }
}
public enum BuildingKind
{
    None,
    Spawner,
    WaterMine,
    FertilizerMine
}
