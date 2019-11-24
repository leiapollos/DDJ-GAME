using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Person : LivingEntity
{

    public enum State { Idle, Chasing};
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

    public float ignoreTargetDistance = 10.0f;

    Vector3 finalPos;
    bool goTo = false;

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
            currentState = State.Chasing;
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());

        }
    }

    protected void Update()
    {
        if((this.transform.position - finalPos).magnitude < 1)
        {
            Destroy(this.gameObject);
            Debug.Log("Saved!");
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
            else if (currentState == State.Chasing && (target.position - transform.position).magnitude < ignoreTargetDistance)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius  / 2);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    //void updateAnimation()
    //{
    //    animator.SetBool("Walking", currentState == State.Chasing ? true : false);
    //    animator.SetBool("Idle", currentState == State.Idle ? true : false);
    //}
}
