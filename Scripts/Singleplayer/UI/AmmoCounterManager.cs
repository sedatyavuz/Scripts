using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoCounterManager : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;

    [SerializeField] private IntVar playerMaxMagCount;
    [SerializeField] private IntVar playerMagCount;

    [SerializeField] private Text ammoCounterText;

    private void Awake()
    {
        gameInfo.OnGameResult += () => { ammoCounterText.gameObject.SetActive(false); };
    }
    void Update()
    {
        ammoCounterText.text = string.Format("Ammo: {0}/{1}", playerMagCount.Value, playerMaxMagCount.Value);
    }
}
