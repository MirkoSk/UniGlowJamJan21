using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniGlow.Utility;

public class JumpUpTrigger : MonoBehaviour
{
    [SerializeField] Vector2 detachForce = new Vector2();

    public Vector2 DetachForce { get => detachForce; }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Constants.TAG_PLAYER)
        {
            WombatController player = collision.transform.GetComponentInParent<WombatController>();
            if (player.Chain) player.Chain.JumpUpTrigger = this;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == Constants.TAG_PLAYER)
        {
            WombatController player = collision.transform.GetComponentInParent<WombatController>();
            if (player.Chain) player.Chain.JumpUpTrigger = null;
        }
    }
}
