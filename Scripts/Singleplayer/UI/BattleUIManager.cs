using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIManager : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;
    //[SerializeField] private GameObject joystick;
    [SerializeField] private Text characterCountText;
    [SerializeField] private GameObject exitButton;

    private void Awake()
    {
        gameInfo.OnGameResult += () => { exitButton.SetActive(false); };
        //gameInfo.OnGameResult += () => { joystick.SetActive(false); };

        gameInfo.OnGamePostWait += UpdateCharacterCounter;
        gameInfo.OnCharacterPostDeath += (x, y) => { UpdateCharacterCounter(); };
    }

    private void UpdateCharacterCounter()
    {
        characterCountText.text = string.Format("Alive: {0}", gameInfo.characters.Count);
    }

}
