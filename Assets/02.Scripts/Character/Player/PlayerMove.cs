using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : Character
{
    [SerializeField] private float m_speed = 2f; // 이동 속도
    private Rigidbody m_rigidbody;
    private PlayerWeapon m_weapon;
    [Header("플레이어 체력UI")]
    public TMP_Text UI_PlayerHP;

    private Camera mainCamera;
    private void Start()
    {
        GameManager.Instance.player = this;
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
        m_weapon = GetComponent<PlayerWeapon>();
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Move(); // WASD로 이동
        AimAndRotate(); // 화면 중앙 기준으로 조준 및 회전
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
            return;
        }

        m_animator.SetFloat("Move", 1); // 일반 이동 애니메이션

        // 이동 벡터 계산
        inputDirection.Normalize();
        inputDirection = mainCamera.transform.TransformDirection(inputDirection);
        inputDirection.y = 0; // Y축 이동 제한
        Vector3 moveVector = inputDirection * m_speed * Time.deltaTime;

        // 실제 이동
        m_rigidbody.MovePosition(m_rigidbody.position + moveVector);
    }

    private void AimAndRotate()
    {
        // 카메라의 정면 방향을 기준으로 회전
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0; // Y축 회전만 반영하도록 설정
        cameraForward.Normalize(); // 정규화하여 방향 벡터로 사용

        // 플레이어의 회전 설정
        Quaternion lookRotation = Quaternion.LookRotation(cameraForward);
        m_rigidbody.rotation = lookRotation;
        // 조준 애니메이션 설정
        if (Input.GetMouseButton(1) || Input.GetMouseButton(0)) // 마우스 클릭 시 조준 상태
        {
            if (m_weapon.isGun)
                m_animator.SetBool("IsAim", true);
            else
                m_animator.SetBool("IsAim", false);
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