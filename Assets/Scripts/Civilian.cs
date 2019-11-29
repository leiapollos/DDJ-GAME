using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : LivingEntity
{

    public enum State { Idle, Walk };
    State currentState;

    UnityEngine.AI.NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;
    Material skinMaterial;
    Animator animator;

    Color originalColour;

    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;

    public float ignoreTargetDistance;

    Vector3 finalPos;
    bool goTo = false;

    bool hide = false;
    Vector3 hiddingSpot;

    GameObject player;

    float lastTime = 0;

    protected override void Start()
    {
        base.Start();
        pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //skinMaterial = GetComponent<Renderer>().material;
        animator = GetComponent<Animator>();
        //originalColour = skinMaterial.color;       

        GameObject finalPosObject = GameObject.FindGameObjectWithTag("Subway");
        finalPos = finalPosObject.transform.position;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Walk;

            hasTarget = true;

            player = GameObject.FindGameObjectWithTag("Player");
            target = player.transform;
            targetEntity = target.GetComponent<LivingEntity>();

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());

        }
    }

    protected void Update()
    {

        if ((this.transform.position - finalPos).magnitude < 1)
        {
            Destroy(this.gameObject);
            GetComponent<LivingEntity>().gameController.GetComponent<GameController>().UpdateScore(3);
        }

        if ((this.transform.position - hiddingSpot).magnitude < 1 || (player.GetComponent<Player>().shooting == false &&
                    (GetComponent<LivingEntity>().gameController.GetComponent<GameController>().ClosestZombie(this.transform).position - this.transform.position).magnitude > 7 && Time.time - lastTime > 1.5f))
        {
            hide = false;
            pathfinder.speed = 3.5f;
        }

    }

    protected void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Subway")
        {
            goTo = true;
        }
    }
    IEnumerator UpdatePath()
    {

        float refreshRate = .25f;
        while (hasTarget)
        {
            if(goTo == true)
            {
                pathfinder.SetDestination(finalPos);
            }
            else if(hide == true)
            {
                pathfinder.speed = 6f;
                hiddingSpot = GetComponent<LivingEntity>().gameController.GetComponent<GameController>().ClosestHiddingSpot(this.transform).position;
                pathfinder.SetDestination(hiddingSpot);
                lastTime = Time.time;
            }
            else if (currentState == State.Walk && (target.position - transform.position).magnitude < ignoreTargetDistance)
            {
                if (player.GetComponent<Player>().shooting == true &&
                    (GetComponent<LivingEntity>().gameController.GetComponent<GameController>().ClosestZombie(this.transform).position - this.transform.position).magnitude < 7)
                {
                    hide = true;
                }
                else
                {
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius / 2);
                    if (!dead)
                    {
                        pathfinder.SetDestination(targetPosition);
                    }
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    void updateAnimation()
    {
        animator.SetBool("Walk", currentState == State.Walk ? true : false);
        animator.SetBool("Idle", currentState == State.Idle ? true : false);
    }
}
