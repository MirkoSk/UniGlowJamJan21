using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniGlow.Utility
{
	public class GameEvents
	{
        public delegate void AttachmentHandler(Vector2 position);
        public static event AttachmentHandler PlayerAttached;

        public static void AttachPlayer(Vector2 position)
        {
            PlayerAttached?.Invoke(position);
        }

        public delegate void DetachHandler(Vector2 detachForce);
        public static event DetachHandler PlayerDetached;

        public static void DetachPlayer(Vector2 detachForce)
        {
            PlayerDetached?.Invoke(detachForce);
        }

        public delegate void DamageTakenHandler(Vector2 enemyPosition);
        public static event DamageTakenHandler PlayerDamaged;

        public static void DamagePlayer(Vector2 enemyPosition)
        {
            PlayerDamaged?.Invoke(enemyPosition);
        }

        public delegate void GameOverHandler();
        public static event GameOverHandler GameOver;

        public static void EndGame()
        {
            Debug.Log("GAME OVER");
            GameOver?.Invoke();
        }
    }
}