using System;
using UnityEngine;

[RequireComponent(typeof(InputHandler))]
[DisallowMultipleComponent]

public class Player : Character
{
    [Header("We take the camera into account for the movement")]
    [SerializeField] private Camera camera;
    [SerializeField] private float moveSpeed, rotateSpeed;
    [SerializeField] private InputHandler inputHandler;
    private Enemie closestEnemie = null;
    private float MaxHp;

    public static Action<bool> IsEnemyNear, IsntAttacking;
    public static Action<float, float> ChangeHp;

    private void Awake()
    {
        MaxHp = Hp;
    }

    private void Update()
    {
        if (currentState == states.die) return;

        if (CheckDied())
        {
            Die();
            return;
        }

        SetClosestEnemy();

        Vector3 target = new Vector3(inputHandler.InputVector.x, 0, inputHandler.InputVector.y);

        if (target == Vector3.zero)
        {
            AnimatorController.SetFloat("Speed", 0);
        }

        else
        {
            RotateTo(MoveTo(target));
        }

        if (CheckEnemyNear())
        {
            IsntAttacking?.Invoke(Time.time - lastAttackTime > AtackSpeed);
        }
    }

    private void OnEnable()
    {
        LiteAttackButtonClicked.LiteAttackButtonClicked_ += LiteAttack;
        DoubleAttackButtonClicked.DoubleAttackButtonClicked_ += DoubleAttack;
        Enemie.ChangeHp += EnemyDead;
    }

    private void OnDisable()
    {
        LiteAttackButtonClicked.LiteAttackButtonClicked_ -= LiteAttack;
        DoubleAttackButtonClicked.DoubleAttackButtonClicked_ -= DoubleAttack;
        Enemie.ChangeHp -= EnemyDead;
    }

    private void EnemyDead()
    {
        ChangeHp?.Invoke(Hp, MaxHp);
    }

    private void SetClosestEnemy()
    {
        var enemies = SceneManager.Instance.Enemies;
        closestEnemie = null;

        for (int i = 0; i < enemies.Count; i++)
        {
            var enemie = enemies[i];
            if (enemie == null)
            {
                continue;
            }

            if (closestEnemie == null)
            {
                closestEnemie = enemie;
                continue;
            }

            var distance = Vector3.Distance(transform.position, enemie.transform.position);
            var closestDistance = Vector3.Distance(transform.position, closestEnemie.transform.position);

            if (distance < closestDistance)
            {
                closestEnemie = enemie;
            }
        }
    }
    private bool CheckEnemyNear()
    {
        if (closestEnemie == null) return false;

        float distance = Vector3.Distance(transform.position, closestEnemie.transform.position);

        bool inAttackRange = distance <= AttackRange;
        IsEnemyNear?.Invoke(inAttackRange);
        return inAttackRange;
    }

    private Vector3 MoveTo(Vector3 target)
    {
        // here we take into account the coordinates of the camera
        target = Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * target;
        
        transform.position += target * moveSpeed * Time.deltaTime;

        AnimatorController.SetFloat("Speed", moveSpeed);

        currentState = states.walk;

        return target;
    }

    private void RotateTo(Vector3 target)
    {
        var rotation = Quaternion.LookRotation(target);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }

    protected override void Die()
    {
        base.Die();

        SceneManager.Instance.GameOver();
    }

    public override void Attack(string trigger, float damage)
    {
        if (closestEnemie == null || currentState == states.die || Time.time - lastAttackTime < AtackSpeed) return;


        float distance = Vector3.Distance(transform.position, closestEnemie.transform.position);


        lastAttackTime = Time.time;

        AnimatorController.SetTrigger(trigger);

        if (distance > AttackRange) return;


        transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

        closestEnemie.Hp -= damage;
        Hp += 5;

        return;
    }

    public void LiteAttack()
    {
        Attack("Attack", Damage);
    }

    public void DoubleAttack()
    {
        Attack("DoubleAttack", 2 * Damage);
    }
}