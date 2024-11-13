using TMPro;
using UnityEngine;

public class PlayerMove : Character
{
    [SerializeField] private float m_speed = 2f; // 이동 속도
    private Rigidbody m_rigidbody;

    [Header("플레이어 체력UI")]
    public TMP_Text UI_PlayerHP;

    private void Start()
    {
        GameManager.Instance.player = this;
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move(); // WASD로 이동
    }

    public override void Move()
    {
        // WASD 입력을 받음
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical"); // W/S
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical);

        if (inputDirection.sqrMagnitude == 0)
        {
            m_animator.SetFloat("Move", 0);
        }
        else
        {
            m_animator.SetFloat("Move", 1); // 일반 이동 애니메이션
        }

        // 대각선 이동 시 속도 문제를 해결하기 위해 벡터 정규화
        if (inputDirection != Vector3.zero)
        {
            inputDirection.Normalize(); // 대각선에서의 속도 증가 방지

            // 카메라 방향을 기준으로 이동
            inputDirection = Camera.main.transform.TransformDirection(inputDirection);
            inputDirection.y = 0; // Y축 이동 제한

            // 이동 벡터 계산
            Vector3 moveVector = inputDirection * m_speed * Time.deltaTime;

            // 이동 방향으로 회전
            Quaternion toRotation = Quaternion.LookRotation(inputDirection); // 이동 방향으로 회전
            m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, toRotation, Time.deltaTime * 10f); // 부드럽게 회전

            // 실제 이동
            m_rigidbody.MovePosition(m_rigidbody.position + moveVector);
        }
        else
        {
            // inputDirection이 zero일 때도 카메라의 방향으로 회전
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f; // Y축 회전만 반영하도록 함
            cameraForward.Normalize(); // 정규화하여 방향 벡터로 사용

            Quaternion toRotation = Quaternion.LookRotation(cameraForward); // 카메라의 앞 방향으로 회전
            m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, toRotation, Time.deltaTime * 10f); // 부드럽게 회전
        }
        // 마우스 오른쪽 클릭을 누르면 마우스 위치를 바라보게 함
        if (Input.GetMouseButton(1)) // 오른쪽 마우스 버튼
        {
            m_animator.SetBool("IsAim", true);
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // 플레이어의 z값을 설정해줍니다.
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition); // 화면상의 마우스 위치를 월드 좌표로 변환

            // 마우스 위치를 바라보게 회전
            Vector3 direction = worldMousePosition - transform.position;
            direction.y = 0; // y축 회전만 반영하도록 설정
            Quaternion lookRotation = Quaternion.LookRotation(direction); // 마우스 방향으로 회전
            m_rigidbody.rotation = lookRotation;
        }
        else
        {
            m_animator.SetBool("IsAim", false);
        }
    }
    public override void TakeDamage(float damage)
    {
        // 데미지를 입었습니다.
        health -= damage;
        GameManager.Instance.playerCityData.UIUpdate(health, UI_PlayerHP);
        if (health <= 0)
        {
            GameLose();
        }

    }
    public void GameLose()
    {
        GameManager.Instance.UI_InGame.GameLose();
    }
}