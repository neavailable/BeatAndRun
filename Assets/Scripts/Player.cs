using UnityEngine;

public class Player : MonoBehaviour
{
    public float Hp;
    public float Damage;
    public float AtackSpeed;
    public float AttackRange = 2;
    public Animator AnimatorController;

    [SerializeField] private Camera camera;
    [SerializeField] private float moveSpeed, rotateSpeed;
    [SerializeField] private InputHandler inputHandler;

    private Enemie closestEnemie = null;
    private enum states { idle, attack, die };
    private states currentState = states.idle;
    private float lastAttackTime = 0;

    private void Update()
    {
        if (currentState == states.die) return;
        

        if (Hp <= 0)
        {
            Die();
            return;
        }


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

        Vector3 target = new Vector3(inputHandler.InputVector.x, 0, inputHandler.InputVector.y);

        if (target == Vector3.zero) AnimatorController.SetFloat("Speed", 0);
        
        else RotateTo(MoveTo(target));
    }

    private Vector3 MoveTo(Vector3 target)
    {
        target = Quaternion.Euler(0, camera.gameObject.transform.eulerAngles.y, 0) * target;
        
        transform.position += target * moveSpeed * Time.deltaTime;

        AnimatorController.SetFloat("Speed", moveSpeed);

        return target;
    }

    private void RotateTo(Vector3 target)
    {
        var rotation = Quaternion.LookRotation(target);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, rotation, rotateSpeed);
    }

    private void Die()
    {
        currentState = states.die;
        AnimatorController.SetTrigger("Die");

        SceneManager.Instance.GameOver();
    }

    public void Attack()
    {
        if (closestEnemie == null ||
            Time.time - lastAttackTime < AtackSpeed ||
            currentState == states.die) return;


        var distance = Vector3.Distance(transform.position, closestEnemie.transform.position);

        if (distance > AttackRange) return;


        transform.transform.rotation = Quaternion.LookRotation(closestEnemie.transform.position - transform.position);

        lastAttackTime = Time.time;
        closestEnemie.Hp -= Damage;
        AnimatorController.SetTrigger("Attack");
    }
}