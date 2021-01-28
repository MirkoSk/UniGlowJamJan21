using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniGlow.Utility;

public class ChainThrower : MonoBehaviour
{
    [SerializeField] GameObject chainPrefab = null;
    [SerializeField] WombatController characterController = null;

    Vector2 inputDirection;
    float inputAngle;
    Chain chain;


    private void OnEnable()
    {
        GameEvents.PlayerDetached += HandlePlayerDetach;
    }

    private void OnDisable()
    {
        GameEvents.PlayerDetached -= HandlePlayerDetach;
    }

    // Update is called once per frame
    void Update()
    {
        inputDirection.x = Input.GetAxis(Constants.INPUT_HORIZONTAL);
        inputDirection.y = Input.GetAxis(Constants.INPUT_VERTICAL);
        inputDirection = inputDirection.normalized;
        inputAngle = Vector2.Angle(Vector2.up, inputDirection);

        if (Input.GetButtonDown(Constants.INPUT_CHAIN) && chain == null)
        {
            chain = Instantiate(chainPrefab, transform.position, Quaternion.identity).GetComponent<Chain>();

            // Calculate shot direction
            if (inputAngle < 17.5f) chain.Initialize(characterController, this, Chain.Direction.Up);
            else if (inputAngle >= 17.5f && inputAngle <= 72.5f) chain.Initialize(characterController, this, Chain.Direction.UpDiagonal);
            else if (inputAngle > 72.5f && inputAngle < 107.5f) chain.Initialize(characterController, this, Chain.Direction.Straight);
            else if (inputAngle >= 107.5f && inputAngle <= 162.5f) chain.Initialize(characterController, this, Chain.Direction.DownDiagonal);
            else if (inputAngle > 162.5f) chain.Initialize(characterController, this, Chain.Direction.Down);
        }
    }



    void HandlePlayerDetach()
    {
        chain = null;
    }
}
