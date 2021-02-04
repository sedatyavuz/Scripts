using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BotController : MonoBehaviour
{
    private Character character;

    [SerializeField] private GameInfo gameInfo;
    
    #region SO Variables
    [SerializeField] private FloatVar currentZoneRadius;
    [SerializeField] private Vector3Var currentZonePosition;
    #endregion

    private CharacterController movementController;

    [SerializeField] Vector3[] currentPath;
    int currentPathIndex;

    bool gotBestPath = false;
    bool hasPath = false;
    bool requestingPath = false;
    bool move = false;
    float stayAfter;
    float staySec;
    bool lookedAtTarget;
    bool staySetAfterFire;
    private void Awake()
    {
        character = GetComponent<Character>();
        movementController = GetComponent<CharacterController>();
        character.OnFire += () =>
        {
            if (!staySetAfterFire)
            {
                staySec = Random.Range(0.25f, 1f);
                staySetAfterFire = true;
            }
        };

        character.OnPositionChanged += (newPos) => {
            movementController.enabled = false;
            transform.position = newPos;
            movementController.enabled = true;
            GetPath(false); };
    }
    private void Start()
    {
        GetPath(false);
        move = true;
    }
    private void Update()
    {
        if (character.isDead || gameInfo.GameFinished)
            return;


        #region Deciding what kinda path should we get
        bool getPath = false;
        bool randomPath = true;

        if (character.isInsideZone)
            gotBestPath = false;

        if (!hasPath)
        {
            getPath = true;
            if (character.isInsideZone)
                randomPath = true;
            else
                randomPath = false;
        }
        else
        {
            Vector3 newZonePos = currentZonePosition.Value;
            newZonePos.y = transform.position.y;
            float lastPointDstFromZone = (newZonePos - currentPath[currentPath.Length - 1]).sqrMagnitude;
            if (lastPointDstFromZone > currentZoneRadius.Value * currentZoneRadius.Value)
            {
                getPath = true;
                randomPath = true;
            }
        }

        if (!character.isInsideZone && !gotBestPath)
        {
            getPath = true;
            randomPath = false;
        }

        if (getPath && !requestingPath)
        {
            if (!randomPath)
                gotBestPath = true;
            GetPath(randomPath);
        }
        #endregion

        
        stayAfter -= Time.deltaTime;
        staySec -= Time.deltaTime;

        if (stayAfter <= 0 && staySec <= 0)
        {
            stayAfter = Random.Range(2, 5);
            staySec = Random.Range(0.25f, 1f);
        }

        if (staySec > 0)
            move = false;
        else
            move = true;

        if (move && hasPath)
        {
            lookedAtTarget = false;
            staySetAfterFire = false;
            character.isMoving = true;
            FollowPath();
        }
        else
        {
            character.isMoving = false;
            if(character.targetHandler.currentTarget && !lookedAtTarget)
            {
                character.transform.LookAt(character.targetHandler.currentTarget.transform.position);
                lookedAtTarget = true;
            }
        }
    }

    private void FollowPath()
    {
        //TODO gravity calculation not done when no path is found.
        Vector3 currentWaypoint = new Vector3(currentPath[currentPathIndex].x, transform.position.y, currentPath[currentPathIndex].z);
        Vector3 directionToMove = (currentWaypoint - transform.position).normalized;
        transform.LookAt(currentWaypoint);
        directionToMove *= character.movementSpeed;

        directionToMove.y = Physics.gravity.y;
        movementController.Move(directionToMove * Time.deltaTime);

        float dst = (currentWaypoint - transform.position).sqrMagnitude;
        if (dst < 0.2f * 0.2f)
            currentPathIndex++;
        
        if (currentPathIndex >= currentPath.Length)
            hasPath = false;
    }

    #region Pathfinding And Following
    private void GetPath(bool randomize)
    {
        requestingPath = true;
        float maxPointRadius;
        Vector2 randomPointInCircle;
        Vector3 randomPointInZone;

        Vector3 fromPosition;
        Vector3 toPosition;

        NavMeshPath path = new NavMeshPath();
        NavMeshHit hitI = new NavMeshHit();
        NavMeshHit hitT = new NavMeshHit();

        NavMesh.SamplePosition(transform.position, out hitI, 100, NavMesh.AllAreas);
        fromPosition = hitI.position;

        while (true)
        {
            maxPointRadius = currentZoneRadius.Value * 0.9f;
            randomPointInCircle = Random.insideUnitCircle * maxPointRadius;
            randomPointInZone.x = currentZonePosition.Value.x + randomPointInCircle.x;
            randomPointInZone.y = transform.position.y;
            randomPointInZone.z = currentZonePosition.Value.z + randomPointInCircle.y;

            if(NavMesh.SamplePosition(randomPointInZone, out hitT, 100, NavMesh.AllAreas)){
                toPosition = hitT.position;
                break;
            }
        }

        if (NavMesh.CalculatePath(fromPosition, toPosition, NavMesh.AllAreas, path))
        {
            currentPath = path.corners;
            currentPathIndex = 0;
            hasPath = true;
            requestingPath = false;
        }
        //PathRequestManager.RequestPath(new PathRequest(transform.position, randomPointInZone, OnPathFound, randomize));
    }
    /*public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {

            return;
        }
        GetPath(false);
    }*/

    void OnDrawGizmos()
    {
        if (currentPath != null)
        {
            for (int i = currentPathIndex; i < currentPath.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(currentPath[i], Vector3.one);

                if (i == currentPathIndex)
                {
                    Gizmos.DrawLine(transform.position, currentPath[i]);
                }
                else
                {
                    Gizmos.DrawLine(currentPath[i - 1], currentPath[i]);
                }
            }
        }
    }
    #endregion
}
