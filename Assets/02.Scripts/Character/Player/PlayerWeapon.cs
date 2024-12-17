using UnityEngine;
using System.Collections;
public class PlayerWeapon : MonoBehaviour
{
    private Animator m_animator;
    public LayerMask enemyLayer; // 적 레이어 설정
    public Weapon currentWeapon;
    public Weapon weapon1; // 무기 1
    public Weapon weapon2; // 무기 2
    public bool isGun = true;
    private Coroutine fireCoroutine; // 발사 코루틴을 저장하는 변수
    private float lastFireTime; // 마지막 발사 시점

    private void Start()
    {
        m_animator = GetComponent<Animator>();
        currentWeapon = weapon1; // 초기 무기를 무기 1로 설정
    }

    private void Update()
    {
        // 무기 변경
        HandleWeaponSwitch();

        // 마우스 왼쪽 버튼을 누르면 발사 코루틴 시작
        if (Input.GetMouseButtonDown(0) && fireCoroutine == null)
        {
            fireCoroutine = StartCoroutine(FireContinuously());
        }

        // 마우스 왼쪽 버튼을 떼면 발사 코루틴 중지
        if (Input.GetMouseButtonUp(0))
        {
            StopFireCoroutine();
        }
    }

    private void HandleWeaponSwitch()
    {
        // 1번 키를 눌렀을 때 무기 1로 변경
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SwitchWeapon(weapon1);
            isGun = true;
        }

        // 2번 키를 눌렀을 때 무기 2로 변경
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SwitchWeapon(weapon2);
            isGun = false;
        }
    }

    private void SwitchWeapon(Weapon newWeapon)
    {
        currentWeapon.gameObject.SetActive(false);
        currentWeapon = newWeapon; // 현재 무기를 새로운 무기로 변경
        currentWeapon.gameObject.SetActive(true);
        GameManager.Instance.UI_InGame.WeaponImage.sprite = currentWeapon.WeaponImage;
    }

    private IEnumerator FireContinuously()
    {
        while (true)
        {
            float timeSinceLastFire = Time.time - lastFireTime;

            // FireRate보다 빨리 발사되지 않도록 시간 검사
            if (timeSinceLastFire >= currentWeapon.FireRate)
            {
                lastFireTime = Time.time; // 발사 시간 갱신
                currentWeapon.Using(transform);
                if (isGun)
                    m_animator.SetTrigger("Attack");
                else
                    m_animator.SetTrigger("Swing");
            }

            yield return null; // 매 프레임 검사
        }
    }

    private void StopFireCoroutine()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine); // 코루틴 중지
            fireCoroutine = null; // 참조 초기화
        }
    }
}