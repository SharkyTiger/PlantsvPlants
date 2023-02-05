using System;
using UnityEngine;

public abstract class DamageableBuilding : MonoBehaviour
{
    public Int32 CurrentHealth;
    public Int32 MaxHealth;
    public Team Team;

    protected abstract void OnBeforeDestruction();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Damage") && collision.gameObject.GetComponent<Bullet>()?.Team != Team)
        {
            CurrentHealth -= collision.gameObject.GetComponent<Bullet>().DamageValue;
            Destroy(collision.gameObject);
            if (CurrentHealth <= 0)
            {
                OnBeforeDestruction();
                Destroy(gameObject);
            }
        }
    }
}
