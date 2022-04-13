using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class PathMover : MonoBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Queue<Vector3> pathPoints = new Queue<Vector3>();

    private bool isCaught = false;
    private bool isFree = false;
    private string layer;

    private void OnEnable()
    {
        EventManager.OnCaught += Caught;
    }

    private void OnDisable()
    {
        EventManager.OnCaught -= Caught;
    }

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        FindObjectOfType<PathCreator>().OnNewPathCreated += SetPoints;

        layer = LayerMask.LayerToName(gameObject.layer);
        if (layer.Equals("Player"))
        {
            animator.SetBool("isSlave", false);
        }
        else
        {
            animator.SetBool("isSlave", true);
        }
    }

    private void SetPoints(IEnumerable<Vector3> points)
    {
        pathPoints = new Queue<Vector3>(points);
    }

    void Update()
    {
        if (GameManager.Instance.State.Equals(GameState.LevelCompleted) && (layer.Equals("Player") || (layer.Equals("Slave") && isFree)))
        {
            GetComponent<BezierFollow>().enabled = true;
        }
        else
        {
            UpdatePathing();
        }

    }

    private void UpdatePathing()
    {
        if (!isCaught && ShouldSetDestination())
        {
            if(layer.Equals("Player") || (layer.Equals("Slave") && isFree))
            {
                if (Input.GetMouseButton(0))
                {
                    animator.SetBool("isWalking", true);
                    navMeshAgent.SetDestination(pathPoints.Dequeue());
                }
                else
                {
                    animator.SetBool("isWalking", false);
                }

            }
        }
    }

    private bool ShouldSetDestination()
    {
        if (GameManager.Instance.State.Equals(GameState.PathCreatedMoving) && pathPoints.Count == 0)
        {
            GameManager.Instance.UpdateGameState(GameState.LevelCompleted);
            
            return false;
        }

        if (GameManager.Instance.State.Equals(GameState.PathCreatedMoving) && (!navMeshAgent.hasPath || navMeshAgent.remainingDistance < 0.5f))
        {
            return true;
        }

        return false;
    }
    private void Caught()
    {
        isCaught = true;
        animator.SetBool("isWalking", false);
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject obj = collision.collider.gameObject;
        if (LayerMask.LayerToName(obj.layer).Equals("Slave") && !obj.GetComponent<PathMover>().isFree)
        {
            obj.GetComponent<NavMeshAgent>().enabled = true;
            obj.transform.GetChild(0).GetComponent<Animator>().SetBool("isWalking", true);
            obj.GetComponent<PathMover>().isFree = true;
            if (obj.transform.childCount > 1)
            {
                Destroy(obj.transform.GetChild(1).gameObject);
            }

            EventManager._onSlaveFreed();

            Vector3 closestPoint = GetClosest(obj.transform.position, obj.GetComponent<PathMover>().pathPoints.ToList());
            Vector3 firstPt = obj.GetComponent<PathMover>().pathPoints.Dequeue();

            for (int i = 0; i < obj.GetComponent<PathMover>().pathPoints.Count; i++)
            {
                while (firstPt != closestPoint)
                {
                    firstPt = obj.GetComponent<PathMover>().pathPoints.Dequeue();
                }
            }
        }
        else if (obj.CompareTag("Ship"))
        {
            obj.GetComponent<Animator>().SetTrigger("HopOn");
            GameManager.Instance.DecreaseNinjaCount();

            if (GameManager.Instance.SavedNinjaCount == 0)
            {
                EventManager._onAllOnBoard();
            }

            Destroy(gameObject);
        }
    }


    public Vector3 GetClosest(Vector3 startPosition, List<Vector3> pickups)
    {
        Vector3 bestTarget = new Vector3(100f,100f,100f);
        float closestDistanceSqr = Mathf.Infinity;

        foreach (Vector3 potentialTarget in pickups)
        {
            Vector3 directionToTarget = potentialTarget - startPosition;

            float dSqrToTarget = directionToTarget.sqrMagnitude;

            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }
        }

        return bestTarget;
    }
}
