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
            if (player.AttackModeActive)
            {
                enemy.ReleaseSoul();
            }
            // Player takes damage
            else if (!player.AttackModeActive && !player.Invincible)
            {
                GameEvents.DamagePlayer(enemy.transform.position);
            }
        }
    }
}
