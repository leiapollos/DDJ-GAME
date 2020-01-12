using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

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
    public List<PowerUp> powerUps;

    public bool shooting = false;

    Animator animator;

    public bool canMove = false;

    public GameObject rapidFireUI;

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
        if (dead)return;

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
        if(canMove == true)
        {

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
    

    }


    protected void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Subway")
        {

            Physics.IgnoreCollision(col.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
        foreach (var p in powerUps)
        {
            if (p.name == col.gameObject.tag)
            {
                p.Aquire();
                Destroy(col.gameObject);
                break;
            }
        }
        if(col.gameObject.tag == "Weapon_PowerUp")
        {
            rapidFireUI.SetActive(true);
            gunController.equippedGun.msBetweenShots = 110;
            Destroy(col.gameObject);
            StartCoroutine(waitForRapidFire());
            
        }
    }

    IEnumerator waitForRapidFire()
    {
        yield return new WaitForSeconds(2f);
        rapidFireUI.SetActive(false);
    }

    void updateAnimation()
    {
        animator.SetBool("Run", currentState == State.Run ? true : false);
        animator.SetBool("Idle", currentState == State.Idle ? true : false);
        //animator.SetBool("Shoot", currentState == State.Shoot ? true : false);
    }
}