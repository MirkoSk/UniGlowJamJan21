using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackMode : MonoBehaviour
{
    [SerializeField] float attackModeVelocity = 5f;
    [SerializeField] float minTimeActive = 1f;
    [SerializeField] float timeToDeactivate = 1f;

    [Header("References")]
    [SerializeField] WombatController wombatController = null;
    [SerializeField] new ParticleSystem particleSystem = null;

    float velocityLastFrame;
    bool active;
    float timeActive;
    float timeVelocityBelowThreshold;

    public bool Active { get => active; }



    void FixedUpdate()
    {
        if (active) timeActive += Time.deltaTime;

        if (active && wombatController.Rigidbody.velocity.magnitude < attackModeVelocity) timeVelocityBelowThreshold += Time.deltaTime;
        else if (active && wombatController.Rigidbody.velocity.magnitude >= attackModeVelocity) timeVelocityBelowThreshold = 0f;

        // Just entered attack mode
        if (!active && !wombatController.Invincible && wombatController.Rigidbody.velocity.magnitude >= attackModeVelocity && velocityLastFrame < attackModeVelocity)
        {
            active = true;
            timeActive = 0f;
            timeVelocityBelowThreshold = 0f;
            if (particleSystem) particleSystem.Play(true);
        }
        // Leaving attack mode
        else if (active && timeActive >= minTimeActive && timeVelocityBelowThreshold >= timeToDeactivate && wombatController.Rigidbody.velocity.magnitude < attackModeVelocity)
        {
            active = false;
            if (particleSystem) particleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
