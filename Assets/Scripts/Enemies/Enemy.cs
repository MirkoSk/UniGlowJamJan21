using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class Enemy : MonoBehaviour
{
    [SerializeField] protected GameObject deathParticlesPrefab = null;

    [Header("Camera Shake")]
    [SerializeField] protected float shakeMagnitude = 1f;
    [SerializeField] protected float shakeRoughness = 1f;
    [SerializeField] protected float shakeFadeIn = 1f;
    [SerializeField] protected float shakeFadeOut = 1f;

    protected bool movingRight;
    protected new Rigidbody2D rigidbody;

    public float ShakeMagnitude { get => shakeMagnitude; }
    public float ShakeRoughness { get => shakeRoughness; }
    public float ShakeFadeIn { get => shakeFadeIn; }
    public float ShakeFadeOut { get => shakeFadeOut; }



    void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }



    public void ReleaseSoul()
    {
        Instantiate(deathParticlesPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    protected void Flip()
    {
        movingRight = !movingRight;
    }
}
