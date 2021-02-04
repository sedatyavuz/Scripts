using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterLocationIndicatorManager : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;

    [SerializeField] private GameObject indicatorPrefab;
    [SerializeField] private GameObject indicatorsParent;

    private void Awake() {
        gameInfo.OnGameStart += ShowIndicators;
        gameInfo.OnGameResult += HideIndicators;
        
        //Add Event called after new character added to characters list
        //gameInfo.On+= (character) => { CreateIndicator(character.gameObject); };
    }

    private void ShowIndicators()
    {
        indicatorsParent.SetActive(true);
    }
    private void HideIndicators()
    {
        indicatorsParent.SetActive(false);
    }
    private void CreateIndicator(GameObject toCharacter)
    {
        GameObject newIndicator = Instantiate(indicatorPrefab, indicatorsParent.transform);
        newIndicator.GetComponent<CharacterLocationIndicator>().toCharacter = toCharacter;
    }
}
