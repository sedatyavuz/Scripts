using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class TargetHandler : MonoBehaviour
{
    private Character character;

    private List<Character> targets = new List<Character>();
    [HideInInspector] public Character currentTarget;

    [SerializeField] private LayerMask shootableLayers;

    private float fov = 120;

    private void Awake()
    {
        character = GetComponentInParent<Character>();

        Physics.IgnoreCollision(GetComponent<Collider>(), GetComponentInParent<Collider>()); //Prevent TargetHandler to recognize its parent Character collider
    }

    private void FixedUpdate()
    {
        UpdateTargets();
    }
    private void Update()
    {
        CheckCurrentTarget();
    }

    public void SetProximityRadius(float range)
    {
        //GetComponent<SphereCollider>().radius = radius;//DEPRECATED
        GenerateCollider(range);
    }

    private void GenerateCollider(float range)
    {
        Vector3 forward = character.transform.forward;
        Vector3 right = character.transform.right;

        float xPos = range * Mathf.Tan(fov/2 * Mathf.Deg2Rad);

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();

        int i = 0;
        for (int y = -1; y <= 1; y++)
        {
            Vector3 point0 = Vector3.zero + Vector3.up*y;
            Vector3 point1 = new Vector3(xPos, y, range);
            Vector3 point2 = new Vector3(-xPos, y, range);
            vertices.Add(point0);
            vertices.Add(point1);
            vertices.Add(point2);
            triangles.Add(i);
            triangles.Add(i+1);
            triangles.Add(i+2);
        }


        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.name = "Generated For Mesh Collider";

        GetComponent<MeshCollider>().sharedMesh = mesh;
    }

    #region Target Handling
    private void UpdateTargets()
    {
        //Constantly Update and order targets by closest and remove destroied/dead characters from list
        targets.RemoveAll(x => { return x == null || !x || x.isDead; });
        targets.Sort((x, y) => {
            float xDst = (x.transform.position - transform.position).sqrMagnitude;
            float yDst = (y.transform.position - transform.position).sqrMagnitude;
            if (xDst == yDst)
                return 0;
            else if (xDst > yDst)
                return 1;
            else
                return -1;
        });

        //Set closest shootable target
        for (int i = 0; i < targets.Count; i++)
        {
            if (CheckIfTargetShootable(targets[i].gameObject))
            {
                currentTarget = targets[i];
                character.OnFoundTarget?.Invoke();
                break;
            }
        }
    }
    private bool CheckIfTargetShootable(GameObject target)
    {
        Vector3 direction = (target.transform.position - transform.position).normalized;
        Ray ray = new Ray(transform.position, direction);
        if (Physics.Raycast(ray, out RaycastHit hit, shootableLayers))
        {
            if (hit.collider.gameObject == target && !hit.collider.GetComponent<Character>().isDead)
                return true;
        }
        return false;
    }
    private void CheckCurrentTarget()
    {
        if (targets.Count == 0)
            currentTarget = null;
        if (currentTarget)
            if (!CheckIfTargetShootable(currentTarget.gameObject) || currentTarget.isDead)
                currentTarget = null;
            else if (!currentTarget)
                currentTarget = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Character"))
            return;
        Character otherCharacter = other.gameObject.GetComponent<Character>();

        if (otherCharacter.team == character.team)
            return;

        targets.Add(otherCharacter);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Character"))
            return;
        Character otherCharacter = other.gameObject.GetComponent<Character>();

        if (otherCharacter.team == character.team)
            return;

        targets.Remove(otherCharacter);
    }
    #endregion
}
