using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUIManager : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;

    [SerializeField] private GameObject tutorialUI;

    [SerializeField] private Text tutorialText;

    [SerializeField] private GameObject tutorial_DragToMove;
    [SerializeField] private GameObject tutorial_StayToShoot;
    [SerializeField] private GameObject tutorial_TapToReload;

    private float delayBetweenTutorials = 1;

    private void Awake()
    {
        gameInfo.OnGamePreWait += StartTutorial;
        gameInfo.OnGameResult += FinishTutorial;
    }

    private void StartTutorial()
    {
        tutorialUI.SetActive(true);
        tutorialText.gameObject.SetActive(true);
        StartCoroutine(TutorialCorutine());
    }
    private IEnumerator TutorialCorutine()
    {
        tutorialText.text = "Drag To Move";
        tutorial_DragToMove.SetActive(true);
        yield return new WaitForSeconds(delayBetweenTutorials);
        tutorial_DragToMove.SetActive(false);

        tutorialText.text = "Stay To Shoot";
        tutorial_StayToShoot.SetActive(true);
        yield return new WaitForSeconds(delayBetweenTutorials);
        tutorial_StayToShoot.SetActive(false);

        tutorialText.text = "Tap To Reload";
        tutorial_TapToReload.SetActive(true);
        yield return new WaitForSeconds(delayBetweenTutorials);
        tutorial_TapToReload.SetActive(false);

        FinishTutorial();
    }
    private void FinishTutorial()
    {
        StopAllCoroutines();
        tutorialUI.SetActive(false);
        tutorialText.gameObject.SetActive(false);
    }

}
