using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterLocationIndicator : MonoBehaviour
{
    public static GameObject refCharacter;
    public GameObject toCharacter;

    private float screenWidth;
    private float screenHeight;
    private float halfScreenWidth;
    private float halfScreenHeight;

    private Image indicatorImg;

    private Camera mainCamera;

    void Start()
    {
        RectTransform canvas = GameObject.FindWithTag("MainCanvas").GetComponent<RectTransform>();
        mainCamera = Camera.main;
        indicatorImg = GetComponent<Image>();
        screenWidth = canvas.rect.width;
        screenHeight = canvas.rect.height;
        halfScreenWidth = screenWidth / 2;
        halfScreenHeight = screenHeight / 2;
        //GetComponent<Image>().color = toCharacter.GetComponent<Character>().teamRef.teamColor;
    }
    void Update()
    {
        if (!toCharacter || toCharacter == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 toCharacterScreenPosition = mainCamera.WorldToScreenPoint(toCharacter.transform.position);
        if (IsInsideScreen(toCharacterScreenPosition))
        {
            indicatorImg.enabled = false;
            return;
        }
        
        indicatorImg.enabled = true;
        UpdatePosition();
    }

    private void UpdatePosition()
    {
        Vector3 direction = (toCharacter.transform.position - refCharacter.transform.position).normalized;
        direction = Quaternion.Euler(0, -45, 0) * direction;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360;
        float angleInRad = angle * Mathf.Deg2Rad;

        float magnitude;
        float abs_cos_angle = Mathf.Abs(Mathf.Cos(angleInRad));
        float abs_sin_angle = Mathf.Abs(Mathf.Sin(angleInRad));
        if (halfScreenWidth * abs_sin_angle <= halfScreenHeight * abs_cos_angle)
            magnitude = halfScreenWidth / abs_cos_angle;
        else
            magnitude = halfScreenHeight / abs_sin_angle;

        float xPosition = Mathf.Cos(angleInRad) * magnitude;
        float yPosition = Mathf.Sin(angleInRad) * magnitude;

        GetComponent<RectTransform>().transform.localPosition = new Vector2(xPosition , yPosition);
        GetComponent<RectTransform>().transform.localRotation = Quaternion.Euler(0,0, angle-90);
    }
    private bool IsInsideScreen(Vector3 position)
    {
        if (position.x < 0 || position.x > Screen.width || position.y < 0 | position.y > Screen.height)
            return false;
        else if (position.x > 0 && position.x < Screen.width && position.y > 0 && position.y < Screen.height)
            return true;
        return false;
    }

    public static void SetReferenceCharacter(Transform character)
    {
        refCharacter = character.gameObject;
    }
}
