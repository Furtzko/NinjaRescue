using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        EventManager.OnAllOnBoard += AllOnBoard;
    }

    private void OnDestroy()
    {
        EventManager.OnAllOnBoard -= AllOnBoard;
    }


    void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    private void AllOnBoard()
    {
        StartCoroutine(DelayedShipMovement(gameObject));
    }

    IEnumerator DelayedShipMovement(GameObject ship)
    {
        yield return new WaitForSeconds(1.5f);
        ship.GetComponent<BezierFollow>().enabled = true;
        animator.SetTrigger("Sail");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.name.Equals("ShipDestroyer"))
        {
            Destroy(gameObject);
        }
    }
}
