using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimatorController : MonoBehaviour
{
    private Character character;
    private Animator animator;

    private void Awake()
    {
        character = GetComponentInParent<Character>();
        animator = GetComponent<Animator>();

        character.OnPreDeath += (x, y) => { OnDeath(); };
        character.OnWon += OnWon;
    }
    private void Update()
    {
        animator.SetBool("Moving", character.isMoving);
    }

    public void SetAnimationType(GunItem gunItem)
    {
        if (gunItem.gunType == GunType.Pistol)
            animator.SetInteger("Gun", 1);
        else
            animator.SetInteger("Gun", 2);
    }

    private void OnWon()
    {
        animator.SetTrigger("Dance");
    }
    private void OnDeath()
    {
        animator.SetTrigger("Die");
    }
}
