using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniGlow;

public class ChainThrower : MonoBehaviour
{
    [SerializeField] GameObject chainPrefab = null;
    [SerializeField] CharacterController characterController = null;

    Vector2 inputDirection;
    float inputAngle;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection.x = Input.GetAxis(Constants.INPUT_HORIZONTAL);
        inputDirection.y = Input.GetAxis(Constants.INPUT_VERTICAL);
        inputDirection = inputDirection.normalized;
        inputAngle = Vector2.Angle(Vector2.up, inputDirection);

        if (Input.GetButtonDown(Constants.INPUT_CHAIN))
        {
            GameObject chain = Instantiate(chainPrefab, transform.position, Quaternion.identity);

            // Calculate shot direction
            if (inputAngle <= 22.5f) chain.GetComponent<ChainProjectile>().Initialize(characterController, ChainProjectile.Direction.Up);
            else if (inputAngle > 22.5f && inputAngle < 67.5f) chain.GetComponent<ChainProjectile>().Initialize(characterController, ChainProjectile.Direction.UpDiagonal);
            else if (inputAngle >= 67.5f && inputAngle <= 112.5f) chain.GetComponent<ChainProjectile>().Initialize(characterController, ChainProjectile.Direction.Straight);
            else if (inputAngle > 112.5f && inputAngle < 157.5f) chain.GetComponent<ChainProjectile>().Initialize(characterController, ChainProjectile.Direction.DownDiagonal);
            else if (inputAngle >= 157.5f) chain.GetComponent<ChainProjectile>().Initialize(characterController, ChainProjectile.Direction.Down);
        }
    }
}
