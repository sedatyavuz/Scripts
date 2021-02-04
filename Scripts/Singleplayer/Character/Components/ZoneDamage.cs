using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneDamage : MonoBehaviour
{
    #region SO Variables
    [SerializeField] private FloatVar currentZoneRadius;
    [SerializeField] private Vector3Var currentZonePosition;
    #endregion

    [SerializeField] private GameInfo gameInfo;
    private Character character;

    private void Awake()
    {
        character = GetComponent<Character>();
    }
    private void Update()
    {
        character.isInsideZone = CheckInsideZone();
        if (!character.isInsideZone && !character.isDead && gameInfo.GameStarted && !gameInfo.GameFinished)
        {
            float damage = character.baseHealth * 0.025f * Time.deltaTime;
            character.health -= damage;
            character.OnReceiveDamageZone?.Invoke(damage);
        }
    }

    private bool CheckInsideZone()
    {
        Vector3 newZonePos = currentZonePosition.Value;
        newZonePos.y = transform.position.y;
        float dstSqr = (newZonePos - transform.position).sqrMagnitude;
        return dstSqr < currentZoneRadius.Value * currentZoneRadius.Value;
    }
}
