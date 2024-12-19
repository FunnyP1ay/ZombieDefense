using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : Character
{
    [SerializeField] private float m_speed = 2f; // �̵� �ӵ�
    private Rigidbody m_rigidbody;
    private PlayerWeapon m_weapon;
    [Header("플레이어 체력 UI")]
    public TMP_Text UI_PlayerHP;

    private Camera mainCamera;
    private void Start()
    {
        GameManager.Instance.player = this;
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.interpolation = RigidbodyInterpolation.Interpolate; // �ε巯�� �������� ���� ����
        m_animator = GetComponent<Animator>();
        m_weapon = GetComponent<PlayerWeapon>();
        mainCamera = Camera.main;
    }
    private void FixedUpdate()
    {
        Move(); // ���� ��� �������� FixedUpdate���� ȣ��
    }

    private void Update()
    {
        AimAndRotate(); // �Է� �� ������ Update���� ó��
    }
    private void LateUpdate()
    {
        AimAndRotate(); // ī�޶� ȸ�� ����
    }
    public override void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

        Vector3 currentVelocity = m_rigidbody.linearVelocity; // 현재 속도를 가져옴

        if (inputDirection.sqrMagnitude > 0)
        {
            m_animator.SetFloat("Move", 1);
            inputDirection = mainCamera.transform.TransformDirection(inputDirection);
            inputDirection.y = 0;

            // 기존의 Y축 속도를 유지
            Vector3 moveVelocity = inputDirection * m_speed;
            moveVelocity.y = currentVelocity.y;

            m_rigidbody.linearVelocity = moveVelocity;
        }
        else
        {
            m_animator.SetFloat("Move", 0);

            // Y축 속도만 유지, 나머지 속도는 0
            m_rigidbody.linearVelocity = new Vector3(0, currentVelocity.y, 0);
        }
    }
    private void AimAndRotate()
    {

        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0; 
        cameraForward.Normalize(); 

        Quaternion lookRotation = Quaternion.LookRotation(cameraForward);
        m_rigidbody.rotation = lookRotation;

        if (Input.GetMouseButton(1) || Input.GetMouseButton(0)) 
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