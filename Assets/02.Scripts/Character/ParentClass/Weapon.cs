using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Weapon : MonoBehaviour
{
    public float FireRate;
    public float Damage;
    public float detectionRange = 10f;  // 탐지 거리
    public Transform FirePosition;
    public ParticleSystem FireEffect;

    public virtual void Using(Transform player)
    {
        ShootRay(player);
        FireEffect.Play();
    }
    private void ShootRay(Transform player)
    {
        // 플레이어 위치와 포워드 방향 기준으로 레이 쏘기
        Vector3 rayOrigin = player.position + Vector3.up * 1.5f; // 레이 시작 위치 (플레이어 눈높이 정도)
        Ray ray = new Ray(rayOrigin, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange))
        {
            // 맞은 오브젝트가 Character 컴포넌트를 가지고 있는지 확인
            if (hit.collider.gameObject.TryGetComponent(out Character target))
            {
                Debug.Log($"좀비 {target.name} 맞음!");
                target.TakeDamage(Damage);
            }
        }

        Debug.DrawRay(rayOrigin, transform.forward * detectionRange, Color.red, 1f);
    }
}
