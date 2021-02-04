using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControllerNew : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;
    [SerializeField] private Transform origin;
    [SerializeField] private float rotationSpeed = 10;
    private void Awake()
    {
        gameInfo.OnGamePreWait += SetCameraOrigin;
        gameInfo.OnGameStart += SetCameraOrigin;
    }
    private void LateUpdate()
    {
        SetCameraOrigin();
    }

    private void SetCameraOrigin()
    {
        if (gameInfo.FocusedCharacter == null)
            return;

        origin.transform.position = gameInfo.FocusedCharacter.transform.position;
        Quaternion fromRotation = origin.transform.rotation;
        Quaternion toRotation = Quaternion.Euler(origin.eulerAngles.x, gameInfo.FocusedCharacter.transform.eulerAngles.y, origin.eulerAngles.z);
        origin.transform.rotation = Quaternion.Lerp(fromRotation, toRotation, rotationSpeed * Time.deltaTime);
    }
}
