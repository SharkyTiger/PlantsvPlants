using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    public BattleUnit BattleUnitScript;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Destructable"))
        {
            Debug.Log("gegner gefunden");
            //todo basisklasse, um teams zu prüfen
            BattleUnitScript.ShootBullet(collision.transform);
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Destructable"))
        {
            BattleUnitScript.ShootBullet(collision.transform);
        }
    }
}
