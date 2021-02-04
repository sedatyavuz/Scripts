using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneController : MonoBehaviour
{
    #region SO Variables
    [SerializeField] private FloatVar currentZoneRadius;
    [SerializeField] private IntVar currentNumberOfShrink;
    [SerializeField] private Vector3Var currentZonePosition;
    #endregion

    [SerializeField] private GameInfo gameInfo;

    private float nextZoneRadius;
    
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float zoneShrinkInterval = 5;
    
    //private ParticleSystem zoneEffect;

    private AudioManager audioManager;

    public void Awake()
    {
        audioManager = GetComponent<AudioManager>();
        //zoneEffect = GetComponentInChildren<ParticleSystem>();

        currentZoneRadius.Value = transform.localScale.x;
        currentZonePosition.Value = transform.position;
        nextZoneRadius = currentZoneRadius.Value;

        gameInfo.OnGameStart += () => { Invoke("StartShinking", zoneShrinkInterval); };
        gameInfo.OnGameFinish += StopShrinking;
    }

    public void StartShinking()
    {
        StartCoroutine("ShrinkZone");
    }
    public void StopShrinking()
    {
        StopAllCoroutines();
        CancelInvoke();
    }
    private IEnumerator ShrinkZone()
    {
        audioManager.Play("Zone Sound");

        Vector3 oldZoneSize = transform.localScale;
        Vector3 newZoneSize = new Vector3(oldZoneSize.x/2, oldZoneSize.y, oldZoneSize.z/2);

        Vector3 oldZonePosition = currentZonePosition.Value;
        Vector3 newZonePosition = GetNewZonePosition();

        float t = 0;
        while (true)
        {
            t += shrinkSpeed * Time.deltaTime;
            transform.localScale = Vector3.Lerp(oldZoneSize, newZoneSize, t);
            transform.position = Vector3.Lerp(oldZonePosition, newZonePosition, t);
            currentZoneRadius.Value = transform.localScale.x;
            currentZonePosition.Value = transform.position;
            //var shape = zoneEffect.shape;
            //shape.radiusThickness = 1 - transform.localScale.x/600;
            if (newZoneSize == transform.localScale)
            {
                currentNumberOfShrink.Value++;
                if (currentNumberOfShrink.Value != 3)
                {
                    Invoke("StartShinking", zoneShrinkInterval);
                }
                yield break;
            }
            yield return null;
        }
    }
    private Vector3 GetNewZonePosition()
    {
        nextZoneRadius /= 2;
        Vector2 randPointInCircle = Random.insideUnitCircle * nextZoneRadius;
        return new Vector3(
            currentZonePosition.Value.x + randPointInCircle.x,
            transform.position.y,
            currentZonePosition.Value.z + randPointInCircle.y);
    }

    void OnDrawGizmos()
    {
        Vector3 newZonePos = currentZonePosition.Value;
        newZonePos.y = 10;
        Gizmos.color = Color.green;
        Gizmos.DrawCube(newZonePos, Vector3.one);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(newZonePos, newZonePos + (Vector3.right * currentZoneRadius.Value));
    }
}
