using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;
using UnityEngine.UI;

public  class Weapon : MonoBehaviour
{
    public float FireRate;
    public float Damage;
    public Sprite WeaponImage;
    public Transform FirePosition;
    public ParticleSystem UsingEffect;
    public Bullet bulletPrefab; 
    public float bulletSpeed = 10f;


    public virtual void Using(Transform player)
    {
        UsingEvent(player);
        UsingEffect.Play();
    }

    public virtual void UsingEvent(Transform player)
    {
        // 메인 카메라에서 화면 중앙으로 레이 발사
        Ray cameraRay = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        // 최대 거리 설정
        float maxDistance = 50f; // 카메라 레이와 플레이어 레이의 최대 거리
        Vector3 targetPosition;
        targetPosition = cameraRay.GetPoint(maxDistance);


        // 방향 계산 (총구에서 목표 위치까지)
        Vector3 direction = (targetPosition - FirePosition.position).normalized;

        // 탄환 발사
        Quaternion bulletRotation = Quaternion.LookRotation(direction);
        Bullet bullet = LeanPool.Spawn(bulletPrefab, FirePosition.position, bulletRotation,null);
        bullet.Damage = Damage;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.linearVelocity = direction * bulletSpeed; // 탄환 이동
    }
}
