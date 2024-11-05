using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerWeapon : MonoBehaviour
{

    private Coroutine fireCoroutine;
    private Character target;
    public LayerMask enemyLayer;
    public Weapon currentWeapon;
    private void Start()
    {
        EquipWeapon();
    }

    // 무기 장착 및 발사 시작
    public void EquipWeapon()
    {

        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
        fireCoroutine = StartCoroutine(FireWeaponRoutine());

    }

    //발사 루틴(좀비 탐지 후 발사)
    private IEnumerator FireWeaponRoutine()
    {
        while (true)
        {
            if (IsEnemyInRange()) // 일정 거리 내에 좀비가 있는지 확인
            {
                currentWeapon?.Using(target); // 좀비가 있을 경우 무기 발사
            }
            yield return new WaitForSecondsRealtime(currentWeapon.FireRate);
        }
    }

    //일정 거리 내에 좀비가 있는지 확인하는 함수
    private bool IsEnemyInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, currentWeapon.detectionRange, enemyLayer);
        foreach (var hitCollider in hitColliders)
        {

            Vector3 directionToZombie = (hitCollider.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToZombie);

            // 45도 범위 내에서 정면에 위치한 좀비만 공격
            if (angle < 45f)
            {
                target = hitCollider.GetComponent<Character>();
                return true;
            }
        }
        return false;
    }
}