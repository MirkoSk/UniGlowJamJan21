using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMode : MonoBehaviour
{
    [SerializeField] float attackModeVelocity = 5f;
    [SerializeField] float minTimeActive = 1f;

    [Header("References")]
    [SerializeField] new Rigidbody2D rigidbody = null;
    [SerializeField] new ParticleSystem particleSystem = null;

    float velocityLastFrame;
    bool active;
    float timeActive;

    public bool Active { get => active; }



    private void Update()
    {
        if (active) timeActive += Time.deltaTime;
    }

    void FixedUpdate()
    {
        // Just entered attack mode
        if (!active && rigidbody.velocity.magnitude >= attackModeVelocity && velocityLastFrame < attackModeVelocity)
        {
            active = true;
            timeActive = 0f;
            if (particleSystem) particleSystem.Play();
        }
        // Leaving attack mode
        else if (active && timeActive >= minTimeActive && rigidbody.velocity.magnitude < attackModeVelocity)
        {
            active = false;
            if (particleSystem) particleSystem.Stop();
        }
    }
}
