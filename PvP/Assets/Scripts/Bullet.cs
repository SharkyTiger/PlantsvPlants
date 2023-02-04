using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Lifetime = 3000f;

    public SpriteRenderer SpriteRenderer;

    public Int32 DamageValue;

    public Team Team;

    private void Awake()
    {
        Destroy(gameObject, Lifetime);

    }

    public void SetValues(Team team, Color color, Int32 damageValue)
    {
        SpriteRenderer.color = color;
        Team = team;
        DamageValue = damageValue;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Destructable"))
        {
            //TODO: Damage am gegner machen
            //Destroy(gameObject);
            Destroy(gameObject);
        }
    }
}
