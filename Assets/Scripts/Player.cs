using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(GunController))]
public class Player : LivingEntity
{
    public enum State { Idle, Run, Shoot };
    State currentState;

    public float moveSpeed = 5;

    Camera viewCamera;
    PlayerController controller;
    GunController gunController;
    public PowerUp vomitPowerUp;

    public bool shooting = false;

    Animator animator;


    protected override void Start()
    {
        base.Start();
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        animator = GetComponent<Animator>();
        viewCamera = Camera.main;
        currentState = State.Idle;
    }

    void Update()
    {
        // Movement input
        Vector3 oldPos = transform.position;
        Vector3 moveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        //moveInput = transform.TransformDirection(moveInput); //remove this to go back to normal
        Vector3 moveVelocity = moveInput.normalized * moveSpeed;
        controller.Move(moveVelocity);

        if (moveVelocity.sqrMagnitude > 0.0001f)
        {      
            currentState = State.Run;
        }
        else currentState = State.Idle;
        

        //transform.Rotate(0, Input.GetAxis("Horizontal"), 0); //remove this to go back to normal

        // Look input
        Ray ray = viewCamera.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            //Debug.DrawLine(ray.origin,point,Color.red);
            controller.LookAt(point);
        }

        // Weapon input
        if (Input.GetMouseButton(0))
        {
            shooting = true;
            gunController.Shoot();
        }
        else
        {
            shooting = false;
        }
        updateAnimation();
    }


    protected void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Subway")
        {
            Debug.Log("Sub");
            Physics.IgnoreCollision(col.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }

        if (col.gameObject.tag == "Pickable")
        {
            vomitPowerUp.Aquire();
            Destroy(col.gameObject);
        }
    }


    void updateAnimation()
    {
        animator.SetBool("Run", currentState == State.Run ? true : false);
        animator.SetBool("Idle", currentState == State.Idle ? true : false);
        //animator.SetBool("Shoot", currentState == State.Shoot ? true : false);
    }
}