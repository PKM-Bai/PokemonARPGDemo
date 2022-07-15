using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{   // 闲置，巡逻，发现，追踪，攻击, 受伤, 死亡
    Idle, Patrol, Guard, React, Chase, Attack, Hit, Death, Skill_One
}
[Serializable]
public class Parameter
{
    public int health;                  // 生命值
    public float idleTime;              // 闲置时间
    public Transform guardPoint;        // 静止、守卫地点
    public Transform[] patrolPoints;    // 巡逻范围
    public Transform[] chasePoints;     // 追踪范围
    public Transform target;            // 发现、追踪的目标
    public LayerMask targetLayer;       // 目标图层
    public Transform attackPoint;       // 攻击
    public Vector3 attackArea;          // 攻击范围
    public Animator animator;           // 动画
    public bool getHit;                 // 受伤
}

public class EnemyFSM : MonoBehaviour
{
    public Parameter parameter;
    public Enemy enemy;
    
    private IState currentState;

    public Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();

    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatorlState(this));
        states.Add(StateType.Guard, new GuardState(this));
        states.Add(StateType.React, new ReactState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Hit, new HitState(this));
        states.Add(StateType.Death, new DeathState(this));
        states.Add(StateType.Skill_One, new Skill_One(this));

        enemy = transform.GetComponent<Enemy>();
        parameter.animator = transform.GetComponent<Animator>();
        TransitionState(StateType.Idle);
    }

    void Update()
    {
        currentState.OnUpdate();
    }

    public void TransitionState(StateType type)
    {
        if (currentState != null)
            currentState.OnExit();
        currentState = states[type];
        currentState.OnEnter();
    }

    public void FilpTo(Transform target)
    {
        if (target != null)
        {
            if (transform.position.x > target.position.x)
                transform.localScale = new Vector3(1, 1, 1);
            else if(transform.position.x < target.position.x)
                transform.localScale = new Vector3(-1, 1, 1);
        }
    }
    public float FilpToDirection(Transform target)
    {
        if (target != null)
        {
            if (transform.position.x > target.position.x)
                return 1f;
            else if(transform.position.x < target.position.x)
                return -1f;
        }
        return 0;
    }

    private void OnTriggerStay2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            parameter.target = other.transform;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Player"))
        {
            parameter.target = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(parameter.attackPoint.position, parameter.attackArea);
    }
}
