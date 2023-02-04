using System;

using UnityEngine;
using UnityEngine.SceneManagement;


public class MainBuilding : DamageableBuilding
{
    public SpriteRenderer SpriteRenderer;

    public void SetValues(Team team, Color color, Int32 maxHealth)
    {
        Team = team;
        SpriteRenderer.color = color;
        MaxHealth = CurrentHealth = maxHealth;
    }

    protected override void OnDestruction()
    {
        SceneManager.LoadScene("GameOverScene");
    }
}
