using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    Quaternion rotation;

    private void Start()
    {
        //offset = new Vector3(target.position.x, target.position.y + 8.0f, target.position.z - 4.0f);
    }

    private void FixedUpdate()
    {
        //Vector3 desiredPosition = target.position + offset;
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        //transform.position = smoothedPosition;

        //Vector3 relativePos = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(target.position, Vector3.up);
        transform.rotation = rotation;

        //  transform.LookAt(target);


        //offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X"), Vector3.up) * offset;
        transform.position = target.position + offset;
        //transform.rotation = target.rotation;
        transform.LookAt(target.position);

        //Quaternion camTurnAngle = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * 5, Vector3.up);
        //offset = camTurnAngle * offset;
        //Vector3 newPos = target.transform.position + offset;
        //transform.position = Vector3.Slerp(transform.position, newPos,1);
        //transform.LookAt(target.position);

    }
}
