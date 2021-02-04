using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycastHider : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;

    private Camera mainCamera;
    private RaycastHit[] hits = new RaycastHit[4];

    private float distance = 100;

    private void Awake()
    {
        mainCamera = Camera.main;
    }
    private void Update()
    {
        MakeBuildingsTransparent();
    }
    /// <summary>
    /// Check whether or not a character is behind a building and hide that building or obstacle accordingly.
    /// </summary>
    private void MakeBuildingsTransparent()
    {
        if (gameInfo.characters.Count == 0)
            return;
        for (int i = 0; i < gameInfo.characters.Count; i++)
        {
            GameObject character = gameInfo.characters[i].gameObject;
            Vector3 charPos = character.transform.position;

            Vector3 charScreenPosition = mainCamera.WorldToScreenPoint(charPos);
            if (!IsInsideScreen(charScreenPosition))
                continue;

            Ray ray = mainCamera.ScreenPointToRay(charScreenPosition);

            hits = Physics.RaycastAll(ray, distance);
            for (int k = 0; k < hits.Length; k++)
            {
                if (hits[k].collider == null)
                    break;
                if (hits[k].collider.gameObject.CompareTag("Building"))
                {
                    hits[k].collider.GetComponent<WallFader>().MakeTransparent();
                }
            }
        }
    }
    private bool IsInsideScreen(Vector3 screenPos)
    {
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y > Screen.height || screenPos.y < 0)
            return false;
        return true;
    }
}
