using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainThrower : MonoBehaviour
{
    [SerializeField] GameObject chainPrefab = null;
    [SerializeField] CharacterController characterController = null;



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            GameObject chain = Instantiate(chainPrefab, transform.position, Quaternion.identity);
            chain.GetComponent<ChainProjectile>().Initialize(characterController);
        }
    }
}
