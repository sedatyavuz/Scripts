using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAudioController : MonoBehaviour
{
    private Character character;
    private AudioManager audioManager;

    private void Awake()
    {
        character = GetComponent<Character>();
        audioManager = GetComponent<AudioManager>();
    }

    public void SetUpAudio(ProjectileController controller)
    {
        character.OnFire += () => { OnFire(controller.gunItem.gunType); };
    }

    private void OnFire(GunType gunType)
    {
        if (enabled == false)
            return;

        switch (gunType)
        {
            case GunType.Pistol:
                audioManager.PlayOneShot("Pistol Sound");
                break;
            case GunType.Rifle:
                audioManager.PlayOneShot("Rifle Sound");
                break;
            case GunType.Sniper:
                audioManager.PlayOneShot("Sniper Sound");
                break;
            case GunType.RPG:
                audioManager.PlayOneShot("RPG Sound");
                break;
            default:
                break;
        }
    }
}
