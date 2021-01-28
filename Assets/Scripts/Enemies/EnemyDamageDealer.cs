using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniGlow.Utility;

public class EnemyDamageDealer : MonoBehaviour
{
    [SerializeField] Enemy enemy = null;



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Constants.TAG_PLAYER)
        {
            WombatController player = collision.transform.GetComponentInParent<WombatController>();
            
            // Player does damage
            if (player.Chain && player.Chain.Attached)
            {
                enemy.ReleaseSoul();
            }
            // Enemy does damage
            else
            {
                player.TakeDamage(enemy.transform.position);
            }
        }
    }
}
