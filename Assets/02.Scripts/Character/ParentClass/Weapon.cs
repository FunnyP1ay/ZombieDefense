using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public  class Weapon : MonoBehaviour
{
    public float FireRate;
    public float Damage;
    public Transform FirePosition;
    public ParticleSystem FireEffect;
    public Bullet bulletPrefab; // ≈∫»Ø «¡∏Æ∆’
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
        rb.linearVelocity = FirePosition.forward * bulletSpeed; // ≈∫»Ø ¿Ãµø
        
    }
}
