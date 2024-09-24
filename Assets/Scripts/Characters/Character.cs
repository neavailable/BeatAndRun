using UnityEngine;

[RequireComponent(typeof(Animator))]

public abstract class Character : MonoBehaviour
{
    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;
    public Animator AnimatorController;
    
    protected enum states { idle, walk, attack, die, endOfGame };
    protected states currentState = states.idle;
    protected float lastAttackTime = 0;

    public virtual void Attack(string trigger, float damage) {}

    protected bool CheckDied()
    {
        return Hp <= 0? true : false;
    }

    protected virtual void Die() 
    {
        currentState = states.die;
        AnimatorController.SetTrigger("Die");
    }
}
