using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class PathfindingController : MonoBehaviour
{
    [SerializeField] private float nextWaypointDistance = 2f;

    [SerializeField] private float stoppingDistance = 1f;

    [SerializeField] private float pathUpdatePeriod = 0.5f;

    [SerializeField] private Vector2 destination;

    [SerializeField] private Transform target;

    [SerializeField] private bool useTarget = true;

    private float defaultStoppingDistance;

    private Seeker seeker;

    private int currentWaypoint;

    private Path path;

    private float timeFromLastPathUpdate = 0;

    [SerializeField] private bool reachedEndOfPath = false;

    private Vector2 desiredMovement;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        defaultStoppingDistance = stoppingDistance;
    }

    private void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
            return;
        }

        //Debug.Log("Path error");
        path = null;
    }

    public Vector2 GetDesiredMovement()
    {
        return desiredMovement;
    }

    public void SetDestination(Vector2 d)
    {
        ResetDestination();
        destination = d;
        useTarget = false;
    }

    public void SetDestination(Vector2 d, float stopDistance)
    {
        ResetDestination();
        destination = d;
        useTarget = false;
        stoppingDistance = stopDistance;
    }

    public void SetTarget(Transform t)
    {
        ResetDestination();
        target = t;
        useTarget = true;        
    }

    public void SetTarget(Transform t, float stopDistance)
    {
        ResetDestination();
        target = t;
        useTarget = true;
        stoppingDistance = stopDistance;
    }

    public void ResetDestination()
    {
        target = null;
        useTarget = true;
        reachedEndOfPath = false;
        currentWaypoint = 0;
        path = null;
        stoppingDistance = defaultStoppingDistance;
    }

    void FixedUpdate()
    {

        desiredMovement = Vector2.zero;

        if (timeFromLastPathUpdate > 0)
        {
            timeFromLastPathUpdate -= Time.deltaTime;
        }

        if (useTarget && target == null) return;

        if (timeFromLastPathUpdate <= 0 && seeker.IsDone())
        {
            if (!useTarget)
            {
                seeker.StartPath(transform.position, new Vector3(destination.x, destination.y, 0), OnPathComplete);
            }
            else seeker.StartPath(transform.position, new Vector3(target.position.x, target.position.y, 0), OnPathComplete);

            timeFromLastPathUpdate = pathUpdatePeriod;
        }

        if (path == null) return;

        float distance = (path.vectorPath[currentWaypoint] - transform.position).magnitude;

        if (distance < nextWaypointDistance && !reachedEndOfPath)
        {
            currentWaypoint++;
            //Debug.Log("Increased waypoint " + currentWaypoint + " : " + path.vectorPath.Count);
        }

        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            currentWaypoint = path.vectorPath.Count-1;
            //Debug.Log("Reached end of Path");
        }

        float globalDistance = (path.vectorPath[path.vectorPath.Count - 1] - transform.position).magnitude;

        if (globalDistance <= stoppingDistance) {
            ResetDestination();
            //Debug.Log("Reached Destination");
            return;
        }

        else reachedEndOfPath = false;

        desiredMovement = (path.vectorPath[currentWaypoint] - transform.position).normalized * Time.fixedDeltaTime;
    }
}
