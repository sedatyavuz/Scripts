using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesHandler : MonoBehaviour
{
    private Character character;

    [SerializeField] private GameInfo gameInfo;

    private GameObject dustTrailEffectObj;
    private void Awake()
    {
        character = GetComponent<Character>();
        dustTrailEffectObj = transform.Find("DustTrailEffect")?.gameObject;
        gameInfo.OnGameStart += () => { dustTrailEffectObj.GetComponent<ParticleSystem>().Play(); };
    }

    public void ActivateEffects()
    {
        dustTrailEffectObj.SetActive(true);
    }

}
