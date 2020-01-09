using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Civilian : LivingEntity
{

    public enum State { Idle, Run };
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

    protected bool hide = true;
    public bool inHiding = true;
    Vector3 hidingSpot;

    GameObject player;

    float lastTime = 0;

    protected override void Start()
    {
        base.Start();
        pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //skinMaterial = GetComponent<Renderer>().material;
        animator = GetComponent<Animator>();
        //originalColour = skinMaterial.color;       
        hidingSpot = GetComponent<LivingEntity>().gameController.GetComponent<GameController>().ClosestHiddingSpot(this.transform).position;
        GameObject finalPosObject = GameObject.FindGameObjectWithTag("Subway");
        finalPos = finalPosObject.transform.position;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Idle;

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
        if (this.dead)
        {
            pathfinder.SetDestination(this.transform.position);
            var animInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (animInfo.IsName("Death") && (animInfo.normalizedTime >= 1))
            {
                GameObject.Destroy(this.gameObject);
            }
            return;
        }

        if ((this.transform.position - finalPos).magnitude < 1)
        {
            Destroy(this.gameObject);
            GetComponent<LivingEntity>().gameController.GetComponent<GameController>().UpdateScore(20);
            this.audioManager.Play("Pling");
        }

        if ((this.transform.position - hidingSpot).magnitude < 2 ||
                    ((GetComponent<LivingEntity>().gameController.GetComponent<GameController>().ClosestZombie(this.transform).position - this.transform.position).magnitude > 7 && Time.time - lastTime > 1.5f))
        {
            hide = false;
            pathfinder.speed = 3.5f;
        }

        inHiding = (this.transform.position - hidingSpot).magnitude < 2 ? true : false;

        updateAnimation();
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
        var targetPos = transform.position;
        while (hasTarget)
        {
            if(goTo == true)
            {
                targetPos = finalPos;
            }
            else if(hide == true)
            {
                pathfinder.speed = 6f;
                hidingSpot = GetComponent<LivingEntity>().gameController.GetComponent<GameController>().ClosestHiddingSpot(this.transform).position;
                targetPos = hidingSpot;
                lastTime = Time.time;
            }
            else if ((target.position - transform.position).magnitude < ignoreTargetDistance)
            {
                if ((GetComponent<LivingEntity>().gameController.GetComponent<GameController>().ClosestZombie(this.transform).position - this.transform.position).magnitude < 7)
                {
                    hide = true;
                }
                else
                {
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius / 2);
                    targetPos = targetPosition;
                }
            }

            pathfinder.SetDestination(targetPos);

            //Update State
            if (this.pathfinder.velocity.sqrMagnitude > 0.001f)
            {
                currentState = State.Run;
            }
            else currentState = State.Idle;

            yield return new WaitForSeconds(refreshRate);
        }
    }

    void updateAnimation()
    {
        animator.SetBool("Run", currentState == State.Run ? true : false);
        animator.SetBool("Idle", currentState == State.Idle ? true : false);
    }
}
