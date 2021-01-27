using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UniGlow.Utility
{
	public class GameEvents
	{
        public delegate void AttachmentHandler(Vector3 position);
        public static event AttachmentHandler PlayerAttached;

        public static void AttachPlayer(Vector3 position)
        {
            PlayerAttached?.Invoke(position);
        }

        public delegate void DetachHandler();
        public static event DetachHandler PlayerDetached;

        public static void DetachPlayer()
        {
            PlayerDetached?.Invoke();
        }
    }
}