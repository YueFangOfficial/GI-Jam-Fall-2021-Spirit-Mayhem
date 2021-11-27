﻿using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ProjectileMove : Move
{
    [Serializable]
    public class ProjectileStats
    {
        public float damage;
        public float range;
        public float speed;
        public float cooldown;
        public float windup;
        public float sizeX;
        public float sizeY;
        public Color color;

        public static ProjectileStats Generate()
        {
            var s = new ProjectileStats
            {
                damage = Random.Range(1, 100),
                range = Random.Range(3, 10),
                speed = Random.Range(5, 30),
                windup = Random.Range(0, .5f),
                sizeX = Random.Range(.5f, 1.5f),
                sizeY = Random.Range(.5f, 1.5f),
                color = new Color(Random.value, Random.value, Random.value, .5f + .5f * Random.value)
            };
            var power = s.damage * (1 + s.range * s.speed * s.sizeY / 1000) * (1 - s.windup);
            s.cooldown = .02f * power;
            return s;
        }
    }

    public Rigidbody2D projectilePrefab;
    public int castButton = 1;
    public ProjectileStats stats;

    public void Start()
    {
        cooldown = stats.cooldown;
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(castButton))
        {
            TryStartMove();
        }

        if (IsActive && Time.time > LastStartTime + stats.windup)
        {
            var projectile = Instantiate(projectilePrefab);
            var projectileComponent = projectile.GetComponent<Projectile>();
            var spriteComponent = projectile.GetComponentInChildren<SpriteRenderer>();
            var toMouse = Player.MousePos - Player.transform.position;
            toMouse.z = 0;

            projectile.transform.position = Player.transform.position;
            projectile.transform.Rotate(0, 0, Mathf.Atan2(toMouse.y, toMouse.x) * Mathf.Rad2Deg);
            projectile.transform.localScale = new Vector3(stats.sizeX, stats.sizeY, 1);
            projectile.velocity = stats.speed * toMouse.normalized;
            projectileComponent.damage = stats.damage;
            spriteComponent.color = stats.color;

            var lifetime = stats.range / stats.speed;
            Destroy(projectile.gameObject, lifetime);

            EndMove();
        }
    }
}