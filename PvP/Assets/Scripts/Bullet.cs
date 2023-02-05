using System;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float Lifetime = 300f;

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
}
