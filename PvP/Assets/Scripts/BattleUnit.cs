using System;

using Unity.VisualScripting;

using UnityEngine;

public class BattleUnit : MonoBehaviour
{
    public Int32 CurrentHealth;
    public Int32 MaxHealth = 5;
    public float CurrentSpeed;
    public float MaxSpeed = 1;
    public Int32 CurrentDamageValue;
    public Int32 MaxDamageValue = 1;
    public Int32 SpawnerBuildingId { get; private set; }

    public Team Team;
    public SpriteRenderer SpriteRenderer;
    public GameObject BulletPrefab;
    public GameObject HighlightObject;

    public Int32 Cooldown;
    public Int32 MaxCooldown;
    private Boolean shotBullet;
    private Vector3 currentDestination;
    private Boolean onMove;

    public event EventHandler<DeathEventArgs> DestroyedEvent;

    public void Spawn(Team team, Color color, Int32 health, Int32 damageValue, float speed, Int32 buildingId)
    {
        Team = team;
        SpriteRenderer.color = color;
        MaxHealth = CurrentHealth = health;
        MaxDamageValue = CurrentDamageValue = damageValue;
        MaxSpeed = CurrentSpeed = speed;
        MaxCooldown = Cooldown = 10;
        SpawnerBuildingId = buildingId;
        currentDestination = transform.position;
        onMove = false;
    }

    public void ShootBullet(Transform target)
    {
        if (shotBullet)
        {
            return;
        }
        shotBullet = true;
        Vector3 direction = (Vector3)target.position - this.transform.position;
        direction.Normalize();
        var bullet = Instantiate(BulletPrefab, this.transform.position + direction, this.transform.rotation);

        bullet.GetComponent<Bullet>().SetValues(Team, SpriteRenderer.color, CurrentDamageValue);
        bullet.GetComponent<Rigidbody2D>().AddForce(direction * 1000f);

    }

    private void FixedUpdate()
    {
        if (shotBullet)
        {
            Cooldown--;
            if(Cooldown <= 0)
            {
                shotBullet = false;
                Cooldown = MaxCooldown;
            }
        }
    }
    void Update()
    {
        if (!onMove) return;
        
        Vector3 pos = transform.position;
        Vector3 direction = currentDestination - transform.position;

        if (direction.magnitude < 1)
        {
            onMove = false;
            return;
        }

        direction.Normalize();

        transform.position += direction * Time.deltaTime;
        transform.rotation = Quaternion.identity;
    }

    public void ToggleHighlight()
    {
        HighlightObject.SetActive(!HighlightObject.activeSelf);
    }

    public void SetDestination(Vector3 vector)
    {
        currentDestination = vector;
        onMove = true;
    }

    private void OnDestroy()
    {
        DestroyedEvent.Invoke(this, new DeathEventArgs(Team, this.gameObject));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage") && collision.gameObject.GetComponent<Bullet>()?.Team != Team)
        {
            CurrentHealth-= collision.gameObject.GetComponent<Bullet>().DamageValue;
            Destroy(collision.gameObject);
            if(CurrentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }

    public class DeathEventArgs : EventArgs {
        public Team Team;
        public GameObject BattleUnit;

        public DeathEventArgs(Team team, GameObject battleUnit)
        {
            Team = team;
            BattleUnit = battleUnit;
        }
    }
}
