using System.Collections;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Animator m_animator;
    public LayerMask enemyLayer; // �� ���̾� ����
    public Weapon currentWeapon;

    private Coroutine fireCoroutine; // �߻� �ڷ�ƾ�� �����ϴ� ����
    private float lastFireTime; // ������ �߻� ����

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // ���콺 ���� ��ư�� ������ �߻� �ڷ�ƾ ����
        if (Input.GetMouseButtonDown(0) && fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireContinuously());
        }

        // ���콺 ���� ��ư�� ���� �߻� �ڷ�ƾ ����
        if (Input.GetMouseButtonUp(0))
        {
            StopFireCoroutine();
        }
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            float timeSinceLastFire = Time.time - lastFireTime;

            // FireRate���� ���� �߻���� �ʵ��� �ð� �˻�
            if (timeSinceLastFire >= currentWeapon.FireRate)
            {
                lastFireTime = Time.time; // �߻� �ð� ����
                currentWeapon.Using(transform);
                m_animator.SetTrigger("Attack"); // ���� �ִϸ��̼�
            }

            yield return null; // �� ������ �˻�
        }
    }

    private void StopFireCoroutine()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine); // �ڷ�ƾ ����
            fireCoroutine = null; // ���� �ʱ�ȭ
        }
    }
}