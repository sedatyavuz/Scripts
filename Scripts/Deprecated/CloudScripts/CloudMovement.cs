using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudMovement : MonoBehaviour
{
    private float speed;

    void Awake()
    {
        speed = Random.Range(0.5f, 1f);
        Invoke("DestroySelf", 30);
    }
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, -transform.forward, speed);
        //transform.Translate(-transform.forward * speed);
    }

    void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
