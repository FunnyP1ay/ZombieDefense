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

    // ���� ���� �� �߻� ����
    public void EquipWeapon()
    {

        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
        }
        fireCoroutine = StartCoroutine(FireWeaponRoutine());

    }

    //�߻� ��ƾ(���� Ž�� �� �߻�)
    private IEnumerator FireWeaponRoutine()
    {
        while (true)
        {
            if (IsEnemyInRange()) // ���� �Ÿ� ���� ���� �ִ��� Ȯ��
            {
                currentWeapon?.Using(target); // ���� ���� ��� ���� �߻�
            }
            yield return new WaitForSecondsRealtime(currentWeapon.FireRate);
        }
    }

    //���� �Ÿ� ���� ���� �ִ��� Ȯ���ϴ� �Լ�
    private bool IsEnemyInRange()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, currentWeapon.detectionRange, enemyLayer);
        foreach (var hitCollider in hitColliders)
        {

            Vector3 directionToZombie = (hitCollider.transform.position - transform.position).normalized;
            float angle = Vector3.Angle(transform.forward, directionToZombie);

            // 45�� ���� ������ ���鿡 ��ġ�� ���� ����
            if (angle < 45f)
            {
                target = hitCollider.GetComponent<Character>();
                return true;
            }
        }
        return false;
    }
}