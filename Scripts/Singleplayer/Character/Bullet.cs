using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] private float damage;
    [HideInInspector] private float speed;
    [HideInInspector] private Character shooter;
    [HideInInspector] private float range;
    private Vector3 initialPosition;
    private void Start()
    {
        initialPosition = transform.position;
    }
    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + transform.forward, speed * Time.deltaTime);
        if (Vector3.Distance(initialPosition, transform.position) > range) //TODO use SQR MAgnitued or any other method for better performance
            Destroy(this.gameObject);
    }
    public void InitBullet(GunItem gunItem, Character shooter)
    {
        this.damage = gunItem.damage;
        this.speed = gunItem.bulletSpeed;
        this.range = gunItem.range;
        this.shooter = shooter;
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Bullet") || collider.isTrigger)
            return;

        if (collider.gameObject.CompareTag("Character"))
        {
            if (shooter && shooter.team == collider.GetComponent<Character>().team)
                return;
            Character enemyChar = collider.gameObject.GetComponent<Character>();
            shooter.OnHitEnemy?.Invoke();
            enemyChar.ReceiveDamage(damage, shooter);
        }

        Destroy(gameObject);
    }
}