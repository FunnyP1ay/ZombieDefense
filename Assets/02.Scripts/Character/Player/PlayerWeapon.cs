using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerWeapon : MonoBehaviour
{
    private Animator m_animator;
    private Coroutine m_fireCoroutine;
    private Character m_target;
    public LayerMask enemyLayer;
    public Weapon currentWeapon;
    private void Start()
    {
        m_animator = GetComponent<Animator>();
        EquipWeapon();
    }

    // ���� ���� �� �߻� ����
    public void EquipWeapon()
    {

        if (m_fireCoroutine != null)
        {
            StopCoroutine(m_fireCoroutine);
        }
        m_fireCoroutine = StartCoroutine(FireWeaponRoutine());

    }

    //�߻� ��ƾ(���� Ž�� �� �߻�)
    private IEnumerator FireWeaponRoutine()
    {
        while (true)
        {
            if (IsEnemyInRange()) // ���� �Ÿ� ���� ���� �ִ��� Ȯ��
            {
                print("���� Ž�� ��");
                currentWeapon?.Using(m_target); // ���� ���� ��� ���� �߻�
                m_animator.SetBool("Attack", true);
            }
            else
            {
                m_animator.SetBool("Attack", false);
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
            if (angle < 25f)
            {
                m_target = hitCollider.GetComponent<Character>();
                return true;
            }
        }
        return false;
    }
}