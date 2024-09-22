using UnityEngine;


public class Player : Character
{
    [Header("We take the camera into account for the movement of the game")]
    [SerializeField] private Camera camera;
    [SerializeField] private float moveSpeed, rotateSpeed;
    [SerializeField] private InputHandler inputHandler;
    private Enemie closestEnemie = null;

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

        if (target == Vector3.zero) AnimatorController.SetFloat("Speed", 0);
        
        else RotateTo(MoveTo(target));
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

    private Vector3 MoveTo(Vector3 target)
    {
        // 
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

    public override void Attack()
    {
        if (closestEnemie == null ||
            Time.time - lastAttackTime < AtackSpeed ||
            currentState == states.die) return;


        var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);


        lastAttackTime = Time.time;

        AnimatorController.SetTrigger("Attack");

        if (distance > AttackRange) return;


        transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

        closestEnemie.Hp -= Damage;
        return;
    }
}