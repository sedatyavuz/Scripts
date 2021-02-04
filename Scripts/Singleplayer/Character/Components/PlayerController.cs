using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;

    private Character character;
    private CharacterController movementController;

    private GameObject camera;

    private void Awake()
    {
        character = GetComponent<Character>();
        movementController = GetComponent<CharacterController>();
        camera = Camera.main.gameObject;

        character.OnPositionChanged += (newPos) => {
            movementController.enabled = false;
            transform.position = newPos;
            movementController.enabled = true;
        };
    }
    private void Update()
    {
        if (character.isDead || gameInfo.GameResulted || gameInfo.GameFinished)
            return;

        Vector3 directionToMove = Vector3.zero;
        Vector3 inputDirection = Vector3.zero;

        if (InputManager.direction != Vector2.zero)
            character.isMoving = true;
        else
            character.isMoving = false;

        if (character.isMoving)
        {
            inputDirection = Quaternion.Euler(0, 45, 0) * new Vector3(InputManager.direction.x, 0, InputManager.direction.y);
            transform.rotation = Quaternion.LookRotation(inputDirection);
        }

        directionToMove += inputDirection * character.movementSpeed;
        directionToMove.y = directionToMove.y > 0 ? 0 : directionToMove.y;
        directionToMove.y = Physics.gravity.y;
        movementController.Move(directionToMove * Time.deltaTime);
    }
    
}
