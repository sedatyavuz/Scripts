using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class CounterManager : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;

    private Text counterTx;
    private float counter = 10;
    private bool counting;

    private void Awake()
    {
        counterTx = GetComponent<Text>();
        gameInfo.OnGamePreWait += () => { counting = true; };
    }
    private void Update()
    {
        if (counting)
        {
            counter -= Time.deltaTime;
            if (counter <= 0)
                gameObject.SetActive(false);
            counterTx.text = Mathf.Ceil(counter).ToString();
        }
    }
}
