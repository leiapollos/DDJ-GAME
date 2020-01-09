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

    Vector3 finalPos;

    Animator animator;

    public Text text1;
    public Text text2;

    public bool canMove = false;

    protected override void Start()
    {
        base.Start();
        controller = GetComponent<PlayerController>();
        gunController = GetComponent<GunController>();
        animator = GetComponent<Animator>();
        viewCamera = Camera.main;
        currentState = State.Idle;
        GameObject finalPosObject = GameObject.FindGameObjectWithTag("FinalPos");
        finalPos = finalPosObject.transform.position;

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

        if ((this.transform.position - finalPos).magnitude < 1)
        {
            //Destroy(this.gameObject);
         
            text1.enabled = true;
            text2.enabled = true;

            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("SAIR");
                this.audioManager.Play("Pling");
            }

            //GetComponent<LivingEntity>().gameController.GetComponent<GameController>().UpdateScore(20);

        }
        else
        {
            text1.enabled = false;
            text2.enabled = false;
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
    }


    void updateAnimation()
    {
        animator.SetBool("Run", currentState == State.Run ? true : false);
        animator.SetBool("Idle", currentState == State.Idle ? true : false);
        //animator.SetBool("Shoot", currentState == State.Shoot ? true : false);
    }
}