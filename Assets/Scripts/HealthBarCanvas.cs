using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarCanvas : MonoBehaviour
{

    Transform target;
    //Quaternion fixedRotation;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;

    }

    void Update()
    {
        Vector3 relativePos = Camera.main.transform.position - transform.position;

        Quaternion rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        transform.rotation = rotation;

    }
}
