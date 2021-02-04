using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharacterUIController : MonoBehaviour
{
    private Character character;

    [SerializeField] private Material radialFill;
    [SerializeField] private Material linearFill;

    private GameObject healthBarObj;
    private Material healthBarMaterial;
    private GameObject nameHolderObj;
    private GameObject receivedDmgText;
    private Vector3 receivedDmgTextStartPosition;

    private GameObject aimIndicator;
    private GameObject reloadSpriteObj;
    private Material reloadSpriteMaterial;

    private Camera mainCamera;

    private void Awake()
    {
        character = GetComponent<Character>();
        character.OnReceiveDamageCharacter += AnimateReceiveDamageText;
        character.OnStartReload += (x) => { StartCoroutine(StartReloading(x)); };
        character.gameInfo.OnGameStart += () => { 
            StopCoroutine("StartReloading");
            FinishReloading();
            };
        
        mainCamera = Camera.main;

        //Global
        receivedDmgText = transform.Find("ReceivedDmg").gameObject;
        receivedDmgTextStartPosition = receivedDmgText.transform.localPosition;
        healthBarObj = transform.Find("HPBAR").gameObject;
        healthBarObj.transform.GetChild(0).GetComponent<SpriteRenderer>().material = linearFill;
        healthBarMaterial = healthBarObj.transform.GetChild(0).GetComponent<SpriteRenderer>().material;
        nameHolderObj = transform.Find("NameHolder").gameObject;

        //Player Only
        aimIndicator = transform.Find("AimIndicator").gameObject;
        reloadSpriteObj = transform.Find("Reload").gameObject;
        reloadSpriteObj.GetComponent<SpriteRenderer>().material = radialFill;
        reloadSpriteMaterial = reloadSpriteObj?.GetComponent<SpriteRenderer>().material;

        reloadSpriteObj.SetActive(false);
        receivedDmgText.SetActive(false);
    }
    private void Start()
    {
        nameHolderObj.GetComponent<TextMeshPro>().text = character.nickname;
    }
    private void Update()
    {
        UpdateCharacterUI();
    }


    #region UI Component Activation
    public void ActivateUIComponents()
    {
        healthBarObj.SetActive(true);
        nameHolderObj.SetActive(true);
    }
    public void DestroyPlayerUIComponents()
    {
        Destroy(aimIndicator);
        Destroy(reloadSpriteObj);
        Destroy(reloadSpriteObj);
    }
    #endregion


    private IEnumerator StartReloading(float reloadTime)
    {
        if (!reloadSpriteObj)
            yield break;

        reloadSpriteObj.SetActive(true);
        float currentTimer = 0;
        while (true)
        {
            currentTimer += Time.deltaTime;
            float fill = (currentTimer / reloadTime) * 360;
            reloadSpriteMaterial.SetFloat("_Fill", fill);
            if(fill >= 360)
            {
                FinishReloading();
                yield break;
            }
            yield return null;
        }
    }
    private void FinishReloading()
    {
        if (!reloadSpriteObj)
            return;

        reloadSpriteObj.SetActive(false);
        reloadSpriteMaterial.SetFloat("_Fill", 360);
    }

    private void UpdateCharacterUI()
    {
        if(reloadSpriteObj)
            reloadSpriteObj?.transform.LookAt(mainCamera.transform.position);

        float hpFill = character.health / character.baseHealth * 100;
        healthBarMaterial.SetFloat("_Fill", hpFill);
        healthBarObj.transform.rotation = mainCamera.transform.rotation;
        receivedDmgText.transform.rotation = mainCamera.transform.rotation;
        nameHolderObj.transform.rotation = mainCamera.transform.rotation;

        if (aimIndicator)
        {
            if (character.targetHandler.currentTarget)
            {
                if (!aimIndicator.activeSelf) aimIndicator.SetActive(true);

                aimIndicator.transform.LookAt(new Vector3(
                    character.targetHandler.currentTarget.transform.position.x,
                    aimIndicator.transform.position.y,
                    character.targetHandler.currentTarget.transform.position.z));
            }
            else
            {
                if (aimIndicator.activeSelf)
                    aimIndicator.SetActive(false);
            }
        }
    }

    public void AnimateReceiveDamageText(float damage, Character shooter)
    {
        StopCoroutine("AnimateReceiveDamageTextCorutine");
        StartCoroutine(AnimateReceiveDamageTextCorutine(damage));
    }
    public IEnumerator AnimateReceiveDamageTextCorutine(float damage)
    {
        receivedDmgText.SetActive(true);
        receivedDmgText.GetComponent<TextMeshPro>().text = damage.ToString();

        receivedDmgText.transform.localPosition = receivedDmgTextStartPosition;
        float startYPoint = receivedDmgTextStartPosition.y;
        float endYPoint = startYPoint + 3;
        Vector3 endPosition = new Vector3(receivedDmgText.transform.localPosition.x, endYPoint, receivedDmgText.transform.localPosition.z);
        float t = 0;
        while (receivedDmgText.transform.localPosition != endPosition)
        {
            float currentYPosition = Mathf.Lerp(startYPoint, endYPoint, t / 50);
            Vector3 currentPosition = new Vector3(receivedDmgText.transform.localPosition.x, currentYPosition, receivedDmgText.transform.localPosition.z);
            receivedDmgText.transform.localPosition = currentPosition;
            t++;
            yield return new WaitForEndOfFrame();
        }

        receivedDmgText.SetActive(false);
    }
}
