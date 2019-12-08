using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Enemy2 : LivingEntity
{

    public enum State { Idle, Chasing, Attacking, Staggered };
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

    public float ignoreTargetDistance;

    float lastTime = 0;
    public int distanceRange;

    Vector3 startPos;

    [Range(0.0f, 1.0f)]
    public float StaggerProbability;
    private float staggerTimeout;

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
        //animator SET DEATH ANIMATION
        //Maybe use END state in animator
    }

    void Update()
    {
        if (dead)
        {
            if (pathfinder.isOnNavMesh) pathfinder.SetDestination(this.transform.position);

            var animInfo = animator.GetCurrentAnimatorStateInfo(0);
            if (animInfo.IsName("Death") && (animInfo.normalizedTime >= 1))
            {
                GameObject.Destroy(this.gameObject);
            }
            return;
        }

        if (currentState == State.Staggered)
        {
            if (staggerTimeout <= 0)
            {
                //No longer staggered
                currentState = State.Idle;
            }
            else
            {
                //Don't move and decrement timeout
                if (pathfinder.isOnNavMesh) pathfinder.SetDestination(this.transform.position);
                staggerTimeout -= Time.deltaTime;
            }
        }

        getClosestTarget();
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


        if (this.pathfinder.velocity.sqrMagnitude > 0.01f)
        {
            currentState = State.Chasing;
        }
        else currentState = State.Idle;

        updateAnimation();

    }


    IEnumerator Attack()
    {
        this.audioManager.Play("ZombieAttack");
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
            //If zombie is staggered - Stop attacking:
            if (currentState == State.Staggered)
            {
                percent = 1;
                continue;
            }

            currentState = State.Attacking;

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
            if (currentState == State.Staggered)
            {
                pathfinder.velocity = Vector3.zero;
                yield return new WaitForSeconds(Mathf.Max(staggerTimeout, 0.0f));
            }

            if (!dead && this.currentState != State.Attacking && (pathfinder.isOnNavMesh))
            {
                if (currentState == State.Chasing && (target.position - transform.position).magnitude < ignoreTargetDistance)
                {
                    pathfinder.speed = 6f;
                    Vector3 dirToTarget = (target.position - transform.position).normalized;
                    Vector3 targetPosition = target.position - dirToTarget * (myCollisionRadius + targetCollisionRadius + attackDistanceThreshold / 2);
                    pathfinder.SetDestination(targetPosition);
                }
                else if (Time.time - lastTime > 2)
                {
                    pathfinder.speed = 3.5f;
                    float x = Random.Range(startPos.x - distanceRange, startPos.x + distanceRange);
                    float z = Random.Range(startPos.z - distanceRange, startPos.z + distanceRange);
                    Vector3 tpos = new Vector3(x, 0, z);
                    Vector3 dirToTarget = (tpos - startPos).normalized;
                    Vector3 targetPosition = tpos - dirToTarget / 2;
                    pathfinder.SetDestination(targetPosition);
                    lastTime = Time.time;
                }
            }
            yield return new WaitForSeconds(refreshRate);
        }
    }

    void updateAnimation()
    {
        switch (currentState)
        {
            case State.Chasing: animator.SetTrigger("Chasing"); break;
            case State.Attacking: animator.SetTrigger("Attacking"); break;
            case State.Staggered: animator.SetTrigger("Staggered"); break;
            default: animator.SetTrigger("Idle"); break;
        }
    }

    void getClosestTarget()
    {
        var closestTarget = GameObject.FindGameObjectWithTag("Player");

        if (closestTarget == null)
        {
            hasTarget = false;
            return;
        }

        var smallestDistance = (closestTarget.transform.position - this.transform.position).sqrMagnitude;

        foreach (var target in GameObject.FindGameObjectsWithTag("Civilian"))
        {
            if (target.GetComponent<Civilian>().inHiding) break;//Zombies don't attack people who are hiding

            var distance = (target.transform.position - this.transform.position).sqrMagnitude;
            if (distance < smallestDistance)
            {
                smallestDistance = distance;
                closestTarget = target;
            }
        }

        this.target = closestTarget.transform;
        this.targetEntity = target.GetComponent<LivingEntity>();
        targetEntity.OnDeath += OnTargetDeath;
        hasTarget = true;
    }

    public void TriggerStagger()
    {
        if (currentState == State.Staggered) return;

        if (Random.value > 1 - StaggerProbability)
        {
            currentState = State.Staggered;
            staggerTimeout = 2;
        }
    }

    //protected void OnCollisionEnter(Collision col)
    //{
    //    if (staggered == true) return;

    //    if (col.gameObject.tag == "Bullet")
    //    {
    //        Debug.Log("Hit");
    //        if (Random.value > 0.9)
    //        {
    //            staggered = true;
    //        }
    //    }
    //}
}