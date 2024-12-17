using UnityEngine;
using System.Collections;
public class PlayerWeapon : MonoBehaviour
{
    private Animator m_animator;
    public LayerMask enemyLayer; // �� ���̾� ����
    public Weapon currentWeapon;
    public Weapon weapon1; // ���� 1
    public Weapon weapon2; // ���� 2
    public bool isGun = true;
    private Coroutine fireCoroutine; // �߻� �ڷ�ƾ�� �����ϴ� ����
    private float lastFireTime; // ������ �߻� ����

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        currentWeapon = weapon1; // �ʱ� ���⸦ ���� 1�� ����
    }

    private void Update()
    {
        // ���� ����
        HandleWeaponSwitch();

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

    private void HandleWeaponSwitch()
    {
        // 1�� Ű�� ������ �� ���� 1�� ����
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(weapon1);
            isGun = true;
        }

        // 2�� Ű�� ������ �� ���� 2�� ����
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(weapon2);
            isGun = false;
        }
    }

    private void SwitchWeapon(Weapon newWeapon)
    {
        currentWeapon.gameObject.SetActive(false);
        currentWeapon = newWeapon; // ���� ���⸦ ���ο� ����� ����
        currentWeapon.gameObject.SetActive(true);
        GameManager.Instance.UI_InGame.WeaponImage.sprite = currentWeapon.WeaponImage;
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
                if (isGun)
                    m_animator.SetTrigger("Attack");
                else
                    m_animator.SetTrigger("Swing");
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