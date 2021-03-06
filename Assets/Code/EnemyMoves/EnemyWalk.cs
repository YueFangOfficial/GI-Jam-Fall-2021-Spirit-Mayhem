using UnityEngine;

public class EnemyWalk : EnemyMove
{
    public float speed = 1f;
    private GameObject player;


    private void Start()
    {
        if (enemy.type == 0) { speed = 2f; }
        else if (enemy.type == 1) { speed = 3f; }
        else if (enemy.type == 2) { speed = 1f; }
    }

    private void FixedUpdate()
    {
        if (!IsActive) return;
        // Melee enemies
        if (enemy.type == 0)
        {
            // If enemy is between alert and attack range
            if (enemy.DistanceToPlayer > enemy.MAttackRange && enemy.DistanceToPlayer <= enemy.MAlertRange)
            {
                // Move enemy closer to player
                enemy.MyRigidbody.velocity = (enemy.PlayerPosition() - enemy.transform.position).normalized * speed;
            }
            // If enemy is outside alert range
            if (enemy.DistanceToPlayer > enemy.MAlertRange)
            {
                enemy.MyRigidbody.velocity = Vector3.zero;
            }
        }
        // Ranged enemies
        else if (enemy.type == 1)
        {
            // If enemy is between alert and attack range
            if (enemy.DistanceToPlayer > enemy.RAttackRange && enemy.DistanceToPlayer <= enemy.RAlertRange)
            {
                // Move enemy closer to player
                enemy.MyRigidbody.velocity = (enemy.PlayerPosition() - enemy.transform.position).normalized * speed;
            }
            // If enemy is outside alert range
            if (enemy.DistanceToPlayer > enemy.RAlertRange)
            {
                enemy.MyRigidbody.velocity = Vector3.zero;
            }
        }
        // Boss enemies
        else if (enemy.type == 2)
        {
            // If enemy is between RangedAlert and RangedAttack range, or between MeleeAlert and MeleeAttack
            if ((enemy.DistanceToPlayer <= enemy.RAlertRange-0.1f && enemy.DistanceToPlayer > enemy.RAttackRange+0.1f) ||
                (enemy.DistanceToPlayer > enemy.MAttackRange+0.1f && enemy.DistanceToPlayer <= enemy.MAlertRange-0.1f))
            {
                // Move enemy closer to player
                enemy.MyRigidbody.velocity = (enemy.PlayerPosition() - enemy.transform.position).normalized * speed;
            }
            // If enemy is outside alert range 
            else if (enemy.DistanceToPlayer > enemy.RAlertRange)
            {
                enemy.MyRigidbody.velocity = Vector3.zero;
            }
        }


    }
    private void Update()
    {
        if (enemy.allMoves.Exists(m => m != this && m.IsActive))
        {
            EndMove();
        }
        else
        {
            TryStartMove();
        }
    }
}
