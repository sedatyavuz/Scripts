using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpineController : MonoBehaviour
{
    private Character character;

    public Transform[] spines;
    private float[] spineRotation = new float[3];

    private float maxTurnAngle = 60;

    private const float MIN_ROTATION_SPEED = 100f;
    private const float MAX_ROTATION_SPEED = 200f;

    public bool IsOnTarget;

    private void Awake()
    {
        character = GetComponent<Character>();
    }

    private void LateUpdate()
    {
    /*    if (character == character.gameInfo.mainPlayer)
            character.health = 1000;*/

        if (character.targetHandler.currentTarget)
            RotateSpinesToTarget(character.targetHandler.currentTarget.gameObject);
        else
            ResetSpineRotation();
    }

    private void RotateSpinesToTarget(GameObject target)//TODO: Optimize and remove unnecessary code
    {
        Vector3 targetDir = target.transform.position - transform.position;

        float angleDiff = Vector3.Angle(targetDir, transform.forward);
        if (Vector3.Dot(transform.right, targetDir) < 0) angleDiff *= -1; // If the angle is to the left then multiple by -1

        float totalSpineAngle = 0;

        for (int i = 0; i < spines.Length; i++)
        {
            if (angleDiff <= maxTurnAngle && angleDiff >= -maxTurnAngle && angleDiff != 0)
                RotateSpineTo(spines[i], angleDiff / 3, i);

            totalSpineAngle += ClampAngle(spineRotation[i]);
        }
        float angleDiffSpines = angleDiff - totalSpineAngle;

        if (angleDiffSpines <= 5 && angleDiffSpines >= -5)
            IsOnTarget = true;
        else
            IsOnTarget = false;
    }
    private void ResetSpineRotation()
    {
        IsOnTarget = false;
        for (int i = 0; i < spines.Length; i++)
            if (spines[i].localEulerAngles.x != 0)
                RotateSpineTo(spines[i], 0, i);
    }
    
    private void RotateSpineTo(Transform spine, float angle, int spineIndex)
    {
        Vector3 curRot = spine.localRotation.eulerAngles;

        float angleDiff = Mathf.Abs(angle - ClampAngle(spineRotation[spineIndex]));
        float pc = angleDiff / maxTurnAngle / 3;
        float speedMod = Mathf.Lerp(MIN_ROTATION_SPEED, MAX_ROTATION_SPEED, pc);

        spineRotation[spineIndex] = Mathf.MoveTowards(spineRotation[spineIndex], angle, speedMod * Time.deltaTime);
        spine.localRotation = Quaternion.Euler(spineRotation[spineIndex] + curRot.x, curRot.y, curRot.z);
    }

    private float ClampAngle(float angle)
    {
        if (angle > 180) angle = angle - 360;
        if (angle < -180) angle = angle + 360;
        return angle;
    }

}
