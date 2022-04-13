using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spiral : MonoBehaviour
{
    void Update()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).transform.LookAt(Camera.main.transform);
        }
    }
}
