using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillRecordsManager : MonoBehaviour
{
    [SerializeField] private GameObject killRecordTextArea;
    [SerializeField] private GameObject emptyKillRecord;

    [SerializeField] private GameInfo gameInfo;

    [SerializeField] private Color playerColor;
    [SerializeField] private Color enemyColor;
    [SerializeField] private Color deathColor;

    private void Awake()
    {
        gameInfo.OnCharacterPreDeath += OnCharacterDied;
    }

    private void OnCharacterDied(Character character, Character killer)
    {
        if (killer == null)
            return;
        GameObject newKillRecord = Instantiate(emptyKillRecord, killRecordTextArea.transform);
        newKillRecord.transform.SetAsFirstSibling();

        Text killerText = newKillRecord.transform.GetChild(0).GetComponent<Text>();
        Text killedText = newKillRecord.transform.GetChild(2).GetComponent<Text>();

        killerText.text = killer.nickname;
        killedText.text = character.nickname;

        killerText.color = killer == gameInfo.mainPlayer ? playerColor : enemyColor;
        killedText.color = deathColor;

        Canvas.ForceUpdateCanvases();
        newKillRecord.GetComponent<HorizontalLayoutGroup>().enabled = false;
        newKillRecord.GetComponent<HorizontalLayoutGroup>().enabled = true;
        
        StartCoroutine(FadeAndDestroy(newKillRecord));
    }

    private IEnumerator FadeAndDestroy(GameObject killRecord)
    {
        yield return new WaitForSeconds(2);
        CanvasGroup killRecordCG = killRecord.GetComponent<CanvasGroup>();
        float t = 0;
        while (true)
        {
            killRecordCG.alpha = Mathf.Lerp(1, 0, t);
            if (t >= 1)
                break;
            t += Time.deltaTime;
            yield return null;
        }
        Destroy(killRecord);
    }
}
