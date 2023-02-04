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

    public Team Team;
    public SpriteRenderer SpriteRenderer;
    public GameObject BulletPrefab;

    public Int32 Cooldown;
    public Int32 MaxCooldown;
    private Boolean shotBullet;

    public void Spawn(Team team, Int32 health, Int32 damageValue, float speed)
    {
        Team = team;
        MaxHealth = CurrentHealth = health;
        MaxDamageValue = CurrentDamageValue = damageValue;
        MaxSpeed = CurrentSpeed = speed;
        MaxCooldown = Cooldown = 5;
    }

    public void ShootBullet(Transform target)
    {
        shotBullet = true;
        var bullet = Instantiate(BulletPrefab, this.transform.position, this.transform.rotation);
        bullet.GetComponent<Bullet>().SetValues(Team, SpriteRenderer.color, CurrentDamageValue);
        bullet.GetComponent<Rigidbody2D>().AddRelativeForce(target.transform.position.normalized * 0.02f, ForceMode2D.Force);

    }

    private void FixedUpdate()
    {
        if (shotBullet)
        {
            Debug.Log("Cooldown");
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
        if (Team == Team.Team1)
        {
            Vector3 pos = transform.position;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                pos.y += 4 * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.DownArrow))
            {
                pos.y -= 4 * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                pos.x += 4 * Time.deltaTime;
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                pos.x -= 4 * Time.deltaTime;
            }

            transform.position = pos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage") && collision.gameObject.GetComponent<Bullet>().Team != Team)
        {
            CurrentHealth--;
            if(CurrentHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
