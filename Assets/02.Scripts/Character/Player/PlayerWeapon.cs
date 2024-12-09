using System.Collections;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    private Animator m_animator;
    public LayerMask enemyLayer; // 적 레이어 설정
    public Weapon currentWeapon;

    private Coroutine fireCoroutine; // 발사 코루틴을 저장하는 변수
    private float lastFireTime; // 마지막 발사 시점

    private void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    private void Update()
    {
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
                m_animator.SetTrigger("Attack"); // 공격 애니메이션
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