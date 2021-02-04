using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    [SerializeField] private GameInfo gameInfo;

    [SerializeField] private IntVar playerMaxMagCount;
    [SerializeField] private IntVar playerMagCount;

    private Character character;

    public GameEvent OnPlayerManualReload;
    [SerializeField] public GunItem gunItem;
    [SerializeField] private GameObject bulletPrefab;

    private float nextFireTimer = 0;
    private int magCount;

    private const float MAX_MISS_ANGLE = 10;

    private void Awake()
    {
        character = GetComponentInParent<Character>();
        gameInfo.OnGameStart += () => {
            magCount = gunItem.magSize;
            character.isReloading = false;
            StopCoroutine(Reload());
            character.OnFinishReload?.Invoke(gunItem.reloadTime);
        };
    }
    private void Start()
    {
        if (character == character.gameInfo.mainPlayer)
            OnPlayerManualReload?.AddListener(ManuelReload);

        character.SetGunController(this);
        character.targetHandler.SetProximityRadius(gunItem.range);
        magCount = gunItem.magSize;

        if (character == character.gameInfo.mainPlayer)
            playerMaxMagCount.Value = gunItem.magSize;
    }
    private void Update()
    {
        TryFire();
        if(character == character.gameInfo.mainPlayer)
            playerMagCount.Value = magCount;
    }

    private void TryFire()
    {
        if (character.isDead || character.gameInfo.GameFinished)
            return;

        nextFireTimer += Time.deltaTime;
        
        if (!character.isMoving && character.spineController.IsOnTarget)
        {
            if (magCount > 0 && character.targetHandler.currentTarget && !character.isReloading)
            {
                if (nextFireTimer >= (1 / gunItem.fireRate))
                {
                    nextFireTimer = 0;
                    magCount--;
                    Fire(character.targetHandler.currentTarget.transform.position);
                }
            }
        }

        if(magCount == 0 && !character.isReloading)
        {
            StartCoroutine(Reload());
        }
    }

    private void ManuelReload() //TODO No need for this function
    {
        if (magCount < gunItem.magSize && !character.isReloading)
        {
            if(character.gameObject.activeSelf)//Error preventing if GameObject is disabled
                StartCoroutine(Reload());
        }
    }
    private IEnumerator Reload()
    {
        character.OnStartReload?.Invoke(gunItem.reloadTime);
        character.isReloading = true;
        yield return new WaitForSeconds(gunItem.reloadTime);
        character.isReloading = false;
        character.OnFinishReload?.Invoke(gunItem.reloadTime);
        magCount = gunItem.magSize;
    }
    private void Fire(Vector3 targetPosition)
    {
        character.OnFire?.Invoke();

        float gunAccuracyPercentage = 1 - (gunItem.accuracy / 100);
        float missAngle = MAX_MISS_ANGLE * gunAccuracyPercentage;
        missAngle = Random.Range(-missAngle, missAngle);

        Vector3 missVector = new Vector3(0, missAngle, 0);
        Vector3 relativePos = targetPosition - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(relativePos, Vector3.up);
        Quaternion finalRotation = Quaternion.Euler(lookRotation.eulerAngles + missVector);
        
        GameObject newBullet = Instantiate(bulletPrefab, transform.position, finalRotation);
        Bullet bulletComponent = newBullet.GetComponent<Bullet>();
        bulletComponent.InitBullet(gunItem, character);
    }
}
