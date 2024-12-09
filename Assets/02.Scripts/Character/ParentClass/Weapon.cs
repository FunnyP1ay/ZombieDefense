using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public  class Weapon : MonoBehaviour
{
    public float FireRate;
    public float Damage;
    public float detectionRange = 10f;  // Å½Áö °Å¸®
    public Transform FirePosition;
    public ParticleSystem FireEffect;
    public Bullet bulletPrefab; // ÅºÈ¯ ÇÁ¸®ÆÕ
    public float bulletSpeed = 10f;


    public virtual void Using(Transform player)
    {
        ShootBullet(player);
        FireEffect.Play();
    }
    private void ShootBullet(Transform player)
    {
        Quaternion bulletRotation = Quaternion.LookRotation(FirePosition.forward);
        Bullet bullet = LeanPool.Spawn(bulletPrefab, FirePosition.position, bulletRotation);
        bullet.Damage = Damage;
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = FirePosition.forward * bulletSpeed; // ÅºÈ¯ ÀÌµ¿
        
    }
}
