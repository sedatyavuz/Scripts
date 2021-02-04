using System;
using UnityEngine;

public class CrashlyticsTester : MonoBehaviour
{
    float secondsBeforeUpdate;

    void Awake()
    {
        secondsBeforeUpdate = 5;
    }

    void Update()
    {
        throwExceptionEvery60Updates();
    }

    void throwExceptionEvery60Updates()
    {
        if (secondsBeforeUpdate > 0)
        {
            secondsBeforeUpdate -= Time.deltaTime;
        }
        else
        {
            secondsBeforeUpdate = 3;
            throw new Exception("Other Exception");
        }
    }
}