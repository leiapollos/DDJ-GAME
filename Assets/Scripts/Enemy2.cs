﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Enemy2 : LivingEntity
{

    public enum State { Idle, Chasing, Attacking };
    State currentState;

    UnityEngine.AI.NavMeshAgent pathfinder;
    Transform target;
    LivingEntity targetEntity;
    Material skinMaterial;
    Animator animator;

    Color originalColour;

    float attackDistanceThreshold = .5f;
    float timeBetweenAttacks = 1;
    float damage = 1;

    float nextAttackTime;
    float myCollisionRadius;
    float targetCollisionRadius;

    bool hasTarget;

    public float ignoreTargetDistance = 10.0f;

    float lastTime = 0;
    public int distanceRange;

    Vector3 startPos;

    protected override void Start()
    {
        startPos = this.transform.position;
        lastTime = Time.time;
        base.Start();
        pathfinder = GetComponent<UnityEngine.AI.NavMeshAgent>();
        //skinMaterial = GetComponent<Renderer>().material;
        animator = GetComponent<Animator>();
        //originalColour = skinMaterial.color;

        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            currentState = State.Chasing;
            hasTarget = true;

            target = GameObject.FindGameObjectWithTag("Player").transform;
            targetEntity = target.GetComponent<LivingEntity>();
            targetEntity.OnDeath += OnTargetDeath;

            myCollisionRadius = GetComponent<CapsuleCollider>().radius;
            targetCollisionRadius = target.GetComponent<CapsuleCollider>().radius;

            StartCoroutine(UpdatePath());

        }
    }



    //protected void OnTriggerEnter(Collider col)
    //{
    //    Debug.Log(col);
    //    if (col.gameObject.tag != "Subway")
    //    {
    //        Debug.Log(col.gameObject.name);
    //        Physics.IgnoreCollision(col.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
    //    }
    //}

    void OnTargetDeath()
    {
        hasTarget = false;
        currentState = State.Idle;
    }

    void Update()
    {

        if (hasTarget)
        {
            if (Time.time > nextAttackTime)
            {
                float sqrDstToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDstToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }

            }
        }

        updateAnimation();

    }

    IEnumerator Attack()
    {

        currentState = State.Attacking;
        pathfinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 dirToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - dirToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        //skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while (percent <= 1)
        {

            if (percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }

            percent += Time.deltaTime * attackSpeed;
            float interpolation = (-Mathf.Pow(percent, 2) + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);

            yield return null;
        }

        //skinMaterial.color = originalColour;
        currentState = State.Chasing;
        pathfinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {

        float refreshRate = .25f;
        while (hasTarget)
        {
            if (currentState == State.Chasing && (target.position - transform.position).magnitude < ignoreTargetDistance)
            {
                Vector3 dirToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
            }
            else if(Time.time - lastTime > 2)
            {
                float x = Random.Range(startPos.x - distanceRange, startPos.x + distanceRange);
                float z = Random.Range(startPos.z - distanceRange, startPos.z + distanceRange);
                Vector3 tpos = new Vector3(x, 0, z);
                Vector3 dirToTarget = (tpos - startPos).normalized;
                Vector3 targetPosition = tpos - dirToTarget / 2;
                if (!dead)
                {
                    pathfinder.SetDestination(targetPosition);
                }
                lastTime = Time.time;
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    void updateAnimation()
    {
        animator.SetBool("Chasing", currentState == State.Chasing ? true : false);
        animator.SetBool("Idle", currentState == State.Idle ? true : false);
        animator.SetBool("Attacking", currentState == State.Attacking ? true : false);
    }

    //protected void OnCollisionEnter(Collision col)
    //{
    //    if (col.gameObject.tag != "Subway")
    //    {
    //        Physics.IgnoreCollision(col.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
    //    }
    //}
}