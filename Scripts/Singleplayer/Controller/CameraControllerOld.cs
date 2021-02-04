using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerOld : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private float distanceFromTarget;

    private void Awake()
    {
        gameInfo.OnGamePreWait += InitilizePosition;
        gameInfo.OnGameStart += InitilizePosition;
    }
    private void LateUpdate()
    {
        FollowTarget();
    }

    private void InitilizePosition()
    {
        transform.position = GetPosition();
    }
    private void FollowTarget()
    {
        transform.position = Vector3.Lerp(transform.position, GetPosition(), 10 * Time.deltaTime);
    }

    private Vector3 GetPosition()
    {
        if (gameInfo.FocusedCharacter == null)
            return Vector3.zero;
        Vector3 characterPos = gameInfo.FocusedCharacter.transform.position;
        return characterPos + (-transform.forward) * distanceFromTarget;
    }
}
