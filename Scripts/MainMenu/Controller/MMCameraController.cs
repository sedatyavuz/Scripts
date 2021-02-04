using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMCameraController : MonoBehaviour
{
    [SerializeField] Transform weaponsCam;
    [SerializeField] Transform playerCam;

    [SerializeField] Camera mainCam;
    [SerializeField] private float cameraMoveSpeed;

    [SerializeField] private GameObject nextSkinButton;
    [SerializeField] private GameObject prevSkinButton;

    [SerializeField] private List<Transform> maleSkinPositions;
    [SerializeField] private List<Transform> femaleSkinPositions;

    private int maleSkinIndex = 0;
    private int femaleSkinIndex = 0;

    private void Awake()
    {
        mainCam = Camera.main;
    }

    public void GoToWeaponsCam()
    {
        StopAllCoroutines();
        StartCoroutine(LerpToPosAndRot(weaponsCam.position, weaponsCam.rotation));
    }
    public void GoToPlayerCam()
    {
        StopAllCoroutines();
        StartCoroutine(LerpToPosAndRot(playerCam.position, playerCam.rotation));
    }
    public void GoToSkinsCam()
    {
        StopAllCoroutines();
        Vector3 newPosition = maleSkinPositions[maleSkinIndex].position + new Vector3(2,1,0);
        StartCoroutine(LerpToPosAndRot(newPosition, Quaternion.Euler(0,-90,0)));

        if(maleSkinIndex == 0)
        {
            prevSkinButton.SetActive(false);
        }
        else if(maleSkinIndex == maleSkinPositions.Count-1)
        {
            nextSkinButton.SetActive(false);
        }
        else
        {
            prevSkinButton.SetActive(true);
            nextSkinButton.SetActive(true);
        }
    }

    public void NextSkinButton_OnClick()
    {
        if (maleSkinIndex < maleSkinPositions.Count-1)
        {
            maleSkinIndex++;
            GoToSkinsCam();
        }
    }
    public void PrevSkinButton_OnClick()
    {
        if(maleSkinIndex > 0)
        {
            maleSkinIndex--;
            GoToSkinsCam();
        }
    }

    private IEnumerator LerpToPosAndRot(Vector3 position, Quaternion rotation)
    {
        float t = 0;
        Vector3 startPos = mainCam.transform.position;
        Quaternion startRot = mainCam.transform.rotation;
        while (true)
        {
            t += Time.deltaTime;
            mainCam.transform.position = Vector3.Lerp(startPos, position, t * cameraMoveSpeed);
            mainCam.transform.rotation = Quaternion.Lerp(startRot, rotation, t * cameraMoveSpeed);
            if (mainCam.transform.position == position)
                yield break;

            yield return null;
        }
    }

}
