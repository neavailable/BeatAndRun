using System;
using UnityEngine;

public class MegaEnemy : Enemie
{
    public static Action<Vector3> MegaEnemyDied;

    private void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        base.Start();
    }

    protected override void Die()
    {
        MegaEnemyDied?.Invoke(transform.position);

        base.Die();
    }

    private void Update()
    {
        base.Update();
    }
}
