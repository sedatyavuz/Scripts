using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    private Character character;

    [HideInInspector] public int killCount;
    [HideInInspector] public int deathCount;
    [HideInInspector] public float travelDistance;
    [HideInInspector] public float surviveTime;

    private void Awake()
    {
        character = GetComponent<Character>();
        character.OnPreDeath += (x, y) => { deathCount++; };
        character.OnEnemyKilled += (x) => { killCount++; };
    }
    private void FixedUpdate()
    {
        if (character.isMoving)
            travelDistance += Time.deltaTime*3;

        surviveTime += Time.deltaTime;
    }
}
