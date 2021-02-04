using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudGenerator : MonoBehaviour
{
    [SerializeField] private GameObject[] cloudObjs;

    int randomIndex;

    public void Start()
    {
        InvokeRepeating("GenerateCloud", 0, 0.5f);
    }
    void GenerateCloud()
    {
        randomIndex = Random.Range(0, cloudObjs.Length);
        Vector3 position = new Vector3(Random.Range(-350, 350), transform.position.y + Random.Range(-20,-40), -250);
        GameObject clone = Instantiate(cloudObjs[randomIndex], position, Quaternion.Euler(0, 90, 0), this.transform);
        float rdm = Random.Range(0.50f, 1.5f);
        clone.transform.localScale *= rdm;
    }
}
