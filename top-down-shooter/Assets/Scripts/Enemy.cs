using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof (NavMeshAgent))]
public class Enemy : LivingEntity
{
    public enum State
    {
        Idle,
        Chasing,
        Attacking
    };
    private State currentState;
    private NavMeshAgent pathFinder;
    private Transform target;
    private LivingEntity targetEntity;
    private Material skinMaterial;
    private Color originalColor;

    private float attackDistanceThreshold = .5f;
    private float timeBetweenAttacks = 1;
    float damage = 1;

    private float nextAttackTime;
    // need these so that the attacks of the enemies dont make them jump into the player and get stuck inside.
    private float myCollisionRadius;
    private float targetCollisionRadius;

    bool hasTarget;

    protected override void Start()
    {
        base.Start();
        pathFinder = GetComponent<NavMeshAgent>();
        skinMaterial = GetComponent<Renderer>().material;
        originalColor = skinMaterial.color;

        if(GameObject.FindGameObjectWithTag("Player") != null)
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

    private void OnTargetDeath()
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
                // we will use the not squared distance between the vectors to save performance, since taking sqrt is expensive
                float sqrDistanceToTarget = (target.position - transform.position).sqrMagnitude;
                if (sqrDistanceToTarget < Mathf.Pow(attackDistanceThreshold + myCollisionRadius + targetCollisionRadius, 2))
                {
                    nextAttackTime = Time.time + timeBetweenAttacks;
                    StartCoroutine(Attack());
                }
            }
        }
    }

    IEnumerator Attack()
    {
        currentState = State.Attacking;
        pathFinder.enabled = false;

        Vector3 originalPosition = transform.position;
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        Vector3 attackPosition = target.position - directionToTarget * (myCollisionRadius);

        float attackSpeed = 3;
        float percent = 0;

        skinMaterial.color = Color.red;
        bool hasAppliedDamage = false;

        while(percent <= 1)
        {
            // we want to take damage at the point where enemy reaches the player which is when 4(-x^2+x) = 1 which is x=1/2
            if(percent >= .5f && !hasAppliedDamage)
            {
                hasAppliedDamage = true;
                targetEntity.TakeDamage(damage);
            }
            percent += Time.deltaTime * attackSpeed;
            // we need a value that goes from 0 to 1 back to 0 so we use 4(-x^2 + x)
            float interpolation = (-percent * percent + percent) * 4;
            transform.position = Vector3.Lerp(originalPosition, attackPosition, interpolation);
            yield return null; 
        }

        skinMaterial.color = originalColor;
        currentState = State.Chasing;
        pathFinder.enabled = true;
    }

    IEnumerator UpdatePath()
    {
        float refreshRate = 0.25f; // we do this so that we calculate position 4 times each second instead of 30 or 60!

        while(hasTarget)
        {
            if(currentState == State.Chasing)
            {
                Vector3 directionToTarget = (target.position - transform.position).normalized;
                Vector3 targetPosition = target.position - directionToTarget *(myCollisionRadius + targetCollisionRadius + attackDistanceThreshold/2);
                if (!dead)
                {
                    pathFinder.SetDestination(targetPosition);
                }
            }
            yield return new WaitForSeconds(refreshRate); //When a yield statement is used, the coroutine pauses execution and automatically resumes at the next frame
        }
    }
}
