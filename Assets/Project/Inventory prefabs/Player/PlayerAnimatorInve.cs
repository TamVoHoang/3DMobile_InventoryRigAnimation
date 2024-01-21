using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAnimatorInve : Singleton_<PlayerAnimatorInve>
{
    public Transform headTransform;
    public Transform faceTransfrom;
    public Transform shoulderLTransfrom;
    public Transform shoulderRTransfrom;


    const float locomotionAnimationSmoothTime = 0.1f;
    NavMeshAgent agent;
    Animator animator;

    protected override void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float speedPercent = agent.velocity.magnitude / agent.speed;
        animator.SetFloat("speedPercent", speedPercent, locomotionAnimationSmoothTime, Time.deltaTime);
    }
}
