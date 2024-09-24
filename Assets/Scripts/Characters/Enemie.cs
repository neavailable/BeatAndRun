using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]

public class Enemie : Character
{
    public NavMeshAgent Agent;
    public static Action ChangeHp;

    private float currentSpeed;

    protected void Awake()
    {
        SceneManager.Instance.AddEnemie(this);
    }

    protected void Start()
    {
        Agent.SetDestination(SceneManager.Instance.Player.transform.position);
    }

    protected void Update()
    {
        if (currentState == states.die || currentState == states.endOfGame) return;

        if (CheckDied())
        {
            Die();
            Agent.isStopped = true;
            return;
        }

        Attack("Attack", Damage);
    }

    protected override void Die()
    {
        base.Die();

        ChangeHp?.Invoke();
        SceneManager.Instance.RemoveEnemie(this);
    }

    protected Enemie GetInstance() => this;

    public void Stop()
    {
        AnimatorController.SetFloat("Speed", 0);

        AnimatorController.SetTrigger("GameOver");
        currentState = states.endOfGame;
    }

    public override void Attack(string trigger, float damage)
    {
        var distance = Vector3.Distance(transform.position, SceneManager.Instance.Player.transform.position);

        float currentSpeed;

        if (distance <= AttackRange)
        {
            Agent.isStopped = true;
            currentSpeed = 0;

            if (Time.time - lastAttackTime > AtackSpeed)
            {
                lastAttackTime = Time.time;
                SceneManager.Instance.Player.Hp -= damage;
                currentState = states.attack;
                AnimatorController.SetTrigger(trigger);
                ChangeHp?.Invoke();
            }
        }
        else
        {
            if (currentState == states.endOfGame) return;

            Agent.SetDestination(SceneManager.Instance.Player.transform.position);
            Agent.isStopped = false;
            currentState = states.walk;
            currentSpeed = Agent.speed;
        }
        AnimatorController.SetFloat("Speed", currentSpeed);
    }

    private void LiteAttack()
    {
        Attack("attack", Damage);
    }
}
