using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MMSelectionManager : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private GameEvent OnSelectionFinished;

    [SerializeField] private GameObject selectionSectionObj;
    [SerializeField] private Button maleSelectBtn;
    [SerializeField] private Button femaleSelectBtn;
    [SerializeField] private InputField nicknameInputField;

    [SerializeField] private Text errorText;

    private Gender selectedGender;
    private string playerName;

    private bool genderIsSelected;
    private bool nameIsFilledCorrectly;

    private void Start()
    {
        if (PlayerPrefs.HasKey("PlayerSelectionDone"))
        {
            PlayerDataManager.LoadPlayerData(playerData);
            OnSelectionFinished.Invoke();
            selectionSectionObj.SetActive(false);
            return;
        }

        femaleSelectBtn.onClick.AddListener(() => SelectGender(Gender.Female));
        maleSelectBtn.onClick.AddListener(() => SelectGender(Gender.Male));
        nicknameInputField.onEndEdit.AddListener((name) => OnNicknameChanged(name));
    }

    private void SelectGender(Gender gender)
    {
        selectedGender = gender;
        if (gender == Gender.Male)
        {
            maleSelectBtn.interactable = false;
            femaleSelectBtn.interactable = true;
            maleSelectBtn.GetComponentInChildren<Text>().text = "Selected";
            femaleSelectBtn.GetComponentInChildren<Text>().text = "Select";
        }
        else
        {
            maleSelectBtn.interactable = true;
            femaleSelectBtn.interactable = false;
            maleSelectBtn.GetComponentInChildren<Text>().text = "Select";
            femaleSelectBtn.GetComponentInChildren<Text>().text = "Selected";
        }
        genderIsSelected = true;
    }
    private void OnNicknameChanged(string name)
    {
        if(name.Length > 5)
        {
            playerName = name;
            nameIsFilledCorrectly = true;
            return;
        }
        nameIsFilledCorrectly = false;
    }
    public void SaveSelection()
    {
        if(genderIsSelected && nameIsFilledCorrectly)
        {
            if (selectedGender == Gender.Male)
                PlayerDataManager.SetToDefaultMale(playerData);
            else
                PlayerDataManager.SetToDefaultFemale(playerData);

            playerData.PlayerNickname = playerName;
            PlayerDataManager.SavePlayerData(playerData);

            PlayerPrefs.SetInt("PlayerSelectionDone", 1);
            PlayerPrefs.Save();
            OnSelectionFinished.Invoke();
            selectionSectionObj.SetActive(false);
        }
        else
        {
            if (!nameIsFilledCorrectly)
            {
                errorText.gameObject.SetActive(true);
                errorText.text = "Nickname should be longer than 5 characters";
            }
            if (!genderIsSelected)
            {
                errorText.gameObject.SetActive(true);
                errorText.text = "You must select a gender.";
            }
        }
    }
}
