using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] public GameInfo gameInfo;

    #region Components' Reference
    [HideInInspector] public Team team;
    [HideInInspector] public TargetHandler targetHandler;
    [HideInInspector] public CharacterAnimatorController animatorController;
    [HideInInspector] public ProjectileController projectilController;
    [HideInInspector] public CharacterAudioController audioController;
    [HideInInspector] public SpineController spineController;
    [HideInInspector] public CharacterStats stats;
    #endregion

    #region Character Variables
    [HideInInspector] public string nickname;
    [HideInInspector] public float movementSpeed;
    [HideInInspector] public float health;
    [HideInInspector] public float baseHealth;
    #endregion

    #region Boolean Variables
    [HideInInspector] public bool isReloading = false;
    [HideInInspector] public bool isShooting = false;
    [HideInInspector] public bool isMoving = false;
    [HideInInspector] public bool isInsideZone;
    [HideInInspector] public bool isDead = false;
    #endregion

    #region Events
    public Action<Vector3> OnPositionChanged;
    public Action OnHitEnemy;
    public Action OnFoundTarget;
    public Action<Character, Character> OnPreDeath;
    public Action<Character, Character> OnPostDeath;
    public Action<Character> OnEnemyKilled;
    public Action<float> OnStartReload;
    public Action<float> OnFinishReload;
    public Action OnFire;
    public Action OnWon;
    public Action OnLost;
    public Action<float> OnReceiveDamageZone;
    public Action<float, Character> OnReceiveDamageCharacter;
    #endregion

    private void Awake()
    {
        team = GetComponentInParent<Team>();
        targetHandler = GetComponentInChildren<TargetHandler>();
        animatorController = GetComponentInChildren<CharacterAnimatorController>();
        audioController = GetComponent<CharacterAudioController>();
        spineController = GetComponent<SpineController>();
        stats = GetComponent<CharacterStats>();

        OnReceiveDamageCharacter += CheckHealth;
        OnReceiveDamageZone += (damage) => { CheckHealth(damage, null); };
    }


    #region Components Initilizations
    public void SetGunController(ProjectileController controller)
    {
        projectilController = controller;
        animatorController.SetAnimationType(controller.gunItem);
        audioController.SetUpAudio(controller);
    }
    #endregion


    public void ReceiveDamage(float damage, Character shooter)
    {
        if (isDead || gameInfo.GameOnWait)
            return;
        health -= damage;
        OnReceiveDamageCharacter?.Invoke(damage, shooter);
    }
    private void CheckHealth(float damage, Character shooter = null)
    {
        if (health <= 0)
        {
            isDead = true;
            isMoving = false;
            StartCoroutine(Die(shooter));
        }
    }
    private IEnumerator Die(Character killer)
    {
        if (killer) killer.OnEnemyKilled(this);
        OnPreDeath?.Invoke(this, killer);
        yield return new WaitForSeconds(1.5f);
        OnPostDeath?.Invoke(this, killer);
    }

}
