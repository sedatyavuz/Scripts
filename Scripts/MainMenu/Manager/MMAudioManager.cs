using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMAudioManager : MonoBehaviour
{
    [SerializeField] private GameEvent OnButtonClick;

    private AudioManager audioManager;
    
    private void Awake()
    {
        audioManager = GetComponent<AudioManager>();

        OnButtonClick.AddListener(delegate { audioManager.Play("Button Click"); });
    }
}
