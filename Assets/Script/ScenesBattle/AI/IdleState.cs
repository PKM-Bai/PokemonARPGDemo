using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleState : IState
{
    private EnemyFSM manager;
    private Parameter parameter;
    private Enemy enemy;

    private float timer;
    private int patorCount = 0;

    public IdleState(EnemyFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.animator.Play("idle");
    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;

        if (parameter.target != null && parameter.target.position.x >= parameter.chasePoints[0].position.x && parameter.target.position.x <= parameter.chasePoints[1].position.x)
            manager.TransitionState(StateType.Chase);

        if (timer >= parameter.idleTime)
        {
            if (patorCount == 2)
            {
                patorCount = 0;
                manager.TransitionState(StateType.Guard);
            }
            else
            {
                patorCount++;
                manager.TransitionState(StateType.Patrol);
            }
        }
    }

    public void OnExit()
    {
        timer = 0;
    }

}

public class PatorlState : IState
{
    private EnemyFSM manager;
    private Parameter parameter;
    private Enemy enemy;

    private int patorlPostionIndex;

    public PatorlState(EnemyFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.enemy = manager.enemy;
    }

    public void OnEnter()
    {
        parameter.animator.Play("walk");
    }

    public void OnUpdate()
    {
        manager.FilpTo(parameter.patrolPoints[patorlPostionIndex]);

        if (parameter.target != null && parameter.target.position.x >= parameter.chasePoints[0].position.x && parameter.target.position.x <= parameter.chasePoints[1].position.x)
        {
            patorlPostionIndex = -1;
            manager.TransitionState(StateType.Chase);
        }

        // 移动到巡逻点
        manager.transform.position = Vector2.MoveTowards(manager.transform.position, parameter.patrolPoints[patorlPostionIndex].position, enemy.moveSpeed * Time.deltaTime);
        if (Vector2.Distance(manager.transform.position, parameter.patrolPoints[patorlPostionIndex].position) < .2f)
        {
            manager.TransitionState(StateType.Idle);
        }


    }

    public void OnExit()
    {
        patorlPostionIndex++;
        if (patorlPostionIndex >= parameter.patrolPoints.Length)
        {
            patorlPostionIndex = 0;
        }
    }

}

public class GuardState : IState
{
    private EnemyFSM manager;
    private Parameter parameter;
    private Enemy enemy;

    private float timer;

    public GuardState(EnemyFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.enemy = manager.enemy;
    }

    public void OnEnter()
    {
        timer = 0;
        parameter.animator.Play("walk");
    }

    public void OnUpdate()
    {
        manager.FilpTo(parameter.guardPoint);

        if (parameter.target != null && parameter.target.position.x >= parameter.chasePoints[0].position.x && parameter.target.position.x <= parameter.chasePoints[1].position.x)
            manager.TransitionState(StateType.Chase);

        // 移动到岗哨点
        manager.transform.position = Vector2.MoveTowards(manager.transform.position, parameter.guardPoint.position, enemy.moveSpeed * Time.deltaTime);
        if (Vector2.Distance(manager.transform.position, parameter.guardPoint.position) < .7f)
        {

            timer += Time.deltaTime;
            if (timer > 3f)
            {
                parameter.animator.Play("guard");
            }
            else
            {
                parameter.animator.Play("idle");
            }
        }
    }

    public void OnExit()
    {

    }

}

public class ReactState : IState
{
    private EnemyFSM manager;
    private Parameter parameter;
    private Enemy enemy;

    public ReactState(EnemyFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {

    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }

}

public class ChaseState : IState
{
    private EnemyFSM manager;
    private Parameter parameter;
    private Enemy enemy;

    private float timer = 0;

    public ChaseState(EnemyFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.enemy = manager.enemy;
    }

    public void OnEnter()
    {
        parameter.animator.Play("walk");
    }

    public void OnUpdate()
    {
        manager.FilpTo(parameter.target);
        if (parameter.target)
        {
            if (manager.FilpToDirection(parameter.target) > 0)
                manager.transform.position = Vector2.MoveTowards(manager.transform.position, parameter.target.position - Vector3.left, enemy.chaseSpeed * Time.deltaTime);
            else
                manager.transform.position = Vector2.MoveTowards(manager.transform.position, parameter.target.position - Vector3.right, enemy.chaseSpeed * Time.deltaTime);
        }

        if (parameter.target == null || manager.transform.position.x < parameter.chasePoints[0].position.x || manager.transform.position.x > parameter.chasePoints[1].position.x)
        {
            timer = 0;
            manager.TransitionState(StateType.Idle);
        }
        else
        {
            if (timer != 0)
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    timer = 0;
                }
            }
        }
        if (timer == 0)
        {
            if (Physics2D.OverlapBox(enemy.skill.releasePoint.position, enemy.skill.skillArea, 0f, parameter.targetLayer))
            {
                if (Physics2D.OverlapBox(parameter.attackPoint.position, parameter.attackArea, 0f, parameter.targetLayer))
                {
                    manager.TransitionState(StateType.Attack);
                }
                else
                {
                    manager.TransitionState(StateType.Skill_One);
                }
            }
        }

    }

    public void OnExit()
    {
        timer = 0.5f;
    }

}

public class AttackState : IState
{
    private EnemyFSM manager;
    private Parameter parameter;
    private Enemy enemy;

    private AnimatorStateInfo info;

    public AttackState(EnemyFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.enemy = manager.enemy;
    }

    public void OnEnter()
    {
        enemy.animator.Play("attack");
    }

    public void OnUpdate()
    {
        info = enemy.animator.GetCurrentAnimatorStateInfo(0);

        if (info.normalizedTime >= .95f)
        {
            manager.TransitionState(StateType.Chase);
        }
    }

    public void OnExit()
    {

    }
}

public class Skill_One : IState
{
    private EnemyFSM manager;
    private Parameter parameter;
    private Enemy enemy;

    private AnimatorStateInfo info;

    public Skill_One(EnemyFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.enemy = manager.enemy;
    }

    public void OnEnter()
    {
        enemy.animator.Play("skill01");
    }

    public void OnUpdate()
    {
        info = enemy.animator.GetCurrentAnimatorStateInfo(0);


        if (info.normalizedTime >= .95f)
        {
            manager.TransitionState(StateType.Chase);
        }
    }

    public void OnExit()
    {
        enemy.OverSkill();
    }
}

public class HitState : IState
{
    private EnemyFSM manager;
    private Parameter parameter;
    private Enemy enemy;

    private AnimatorStateInfo info;

    public HitState(EnemyFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.enemy = manager.enemy;
    }

    public void OnEnter()
    {
        parameter.animator.Play("hit");

    }

    public void OnUpdate()
    {
        info = parameter.animator.GetCurrentAnimatorStateInfo(0);

        if (enemy.currentHP <= 0)
            manager.TransitionState(StateType.Death);

        else if (info.normalizedTime >= enemy.hitTimer)
        {
            manager.TransitionState(StateType.Chase);
        }
    }

    public void OnExit()
    {

    }

}
public class DeathState : IState
{
    private EnemyFSM manager;
    private Parameter parameter;
    private Enemy enemy;

    private AnimatorStateInfo info;

    public DeathState(EnemyFSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
        this.enemy = manager.enemy;
    }

    public void OnEnter()
    {
        parameter.animator.Play("death");
    }

    public void OnUpdate()
    {

    }

    public void OnExit()
    {

    }

}

