using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BezierFollow : MonoBehaviour
{
    [SerializeField]
    public Transform route;

    private float tParam;

    private Vector3 objectPosition;

    [SerializeField]
    private float speedModifier = 0.5f;

    Vector3 p0;


    void Start()
    {
        tParam = 0f;
        p0 = transform.position;
    }

    void Update()
    {
        GoByTheRoute();
    }

    private void GoByTheRoute()
    {
        Vector3 p1 = route.GetChild(0).position;
        Vector3 p2 = route.GetChild(1).position;
        Vector3 p3 = route.GetChild(2).position;

        tParam += Time.deltaTime * speedModifier;

        objectPosition = Mathf.Pow(1 - tParam, 3) * p0 + 3 * Mathf.Pow(1 - tParam, 2) * tParam * p1 + 3 * (1 - tParam) * Mathf.Pow(tParam, 2) * p2 + Mathf.Pow(tParam, 3) * p3;

        transform.position = objectPosition;
        transform.LookAt(p3);
    }
}
