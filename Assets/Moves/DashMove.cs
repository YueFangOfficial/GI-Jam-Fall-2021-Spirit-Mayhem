﻿using UnityEngine;

public class DashMove : Move
{
    public float speed = 20;
    public float smoothing = .5f;
    public float duration = .1f;

    private void FixedUpdate()
    {
        if (!IsActive) return;
        var goalVelocity = player.LastNonzeroMoveDirection * speed;
        player.MyRigidbody.velocity = Vector2.Lerp(goalVelocity, player.MyRigidbody.velocity, smoothing);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryStartMove();
        }

        if (IsActive && Time.time > LastStartTime + duration)
        {
            EndMove();
        }
    }

    public override void OnStartMove()
    {
        foreach (var otherMove in player.allMoves)
        {
            if (otherMove != this)
            {
                otherMove.EndMove();
            }
        }
    }
}