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
    }
}