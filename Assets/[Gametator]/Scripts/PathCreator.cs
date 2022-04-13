using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PathCreator : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private GameObject player;
    private GameObject finish;
    private bool isMoving = false;

    private List<Vector3> points = new List<Vector3>();

    public Action<IEnumerable<Vector3>> OnNewPathCreated = delegate { };

    private ParticleSystem drawParticleSystem;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        player = GameObject.Find("Player");
        finish = GameObject.Find("Finish");
        drawParticleSystem = GameObject.Find("DrawParticle").GetComponent<ParticleSystem>();
    }

    private void Start()
    {
        drawParticleSystem.Stop();
    }

    private void Update()
    {
        if (!isMoving)
        {
            if (Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    if (DistanceToLastPoint(hitInfo.point) > 0.2f && hitInfo.collider.CompareTag("Playground"))
                    {
                        if (GameManager.Instance.State.Equals(GameState.InGame))
                        {
                            GameManager.Instance.UpdateGameState(GameState.InGameDrawing);
                        }
                        
                        points.Add(hitInfo.point);

                        var emitParams = new ParticleSystem.EmitParams();
                        emitParams.position = new Vector3(hitInfo.point.x, 0.1f, hitInfo.point.z);
                        drawParticleSystem.Emit(emitParams, 1);

                        lineRenderer.positionCount = points.Count;
                        lineRenderer.SetPositions(points.ToArray());

                        if (Vector3.Distance(finish.transform.position, hitInfo.point) < 1.2f)
                        {
                            GameManager.Instance.UpdateGameState(GameState.PathCreated);
                            OnNewPathCreated(points);
                            isMoving = true;
                        }
                    }
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                if (points.Any() && Vector3.Distance(finish.transform.position, points.ElementAt(points.Count - 1)) < 1.2f)
                {
                    GameManager.Instance.UpdateGameState(GameState.PathCreated);
                    OnNewPathCreated(points);
                    isMoving = true;
                }
                else
                {
                    if (GameManager.Instance.State.Equals(GameState.InGameDrawing))
                    {
                        GameManager.Instance.UpdateGameState(GameState.InGame);
                        points.Clear();
                        lineRenderer.enabled = false;
                    }
                }
            }
        }
    }

    private float DistanceToLastPoint(Vector3 point)
    {
        if (!points.Any() && isDrawable(point))
        {
            lineRenderer.enabled = true;
            return Mathf.Infinity;
        }
        else if (points.Any())
        {
            return Vector3.Distance(points.Last(), point);
        }
        else
        {
            return 0f;
        }
    }

    private bool isDrawable(Vector3 point)
    {
        if (Vector3.Distance(player.transform.position, point) < 0.5f)
        {
            return true;
        }

        return false;

    }

}
