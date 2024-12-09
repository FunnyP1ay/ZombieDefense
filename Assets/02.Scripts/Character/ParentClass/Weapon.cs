using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Weapon : MonoBehaviour
{
    public float FireRate;
    public float Damage;
    public float detectionRange = 10f;  // Ž�� �Ÿ�
    public Transform FirePosition;
    public ParticleSystem FireEffect;

    public virtual void Using(Transform player)
    {
        ShootRay(player);
        FireEffect.Play();
    }
    private void ShootRay(Transform player)
    {
        // �÷��̾� ��ġ�� ������ ���� �������� ���� ���
        Vector3 rayOrigin = player.position + Vector3.up * 1.5f; // ���� ���� ��ġ (�÷��̾� ������ ����)
        Ray ray = new Ray(rayOrigin, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, detectionRange))
        {
            // ���� ������Ʈ�� Character ������Ʈ�� ������ �ִ��� Ȯ��
            if (hit.collider.gameObject.TryGetComponent(out Character target))
            {
                Debug.Log($"���� {target.name} ����!");
                target.TakeDamage(Damage);
            }
        }

        Debug.DrawRay(rayOrigin, transform.forward * detectionRange, Color.red, 1f);
    }
}
