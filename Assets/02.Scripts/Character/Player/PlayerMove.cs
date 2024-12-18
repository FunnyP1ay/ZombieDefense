using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : Character
{
    [SerializeField] private float m_speed = 2f; // �̵� �ӵ�
    private Rigidbody m_rigidbody;
    private PlayerWeapon m_weapon;
    [Header("�÷��̾� ü��UI")]
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
        Move(); // WASD�� �̵�
        AimAndRotate(); // ȭ�� �߾� �������� ���� �� ȸ��
    }

    public override void Move()
    {
        // WASD �Է��� ����
        float horizontal = Input.GetAxis("Horizontal"); // A/D
        float vertical = Input.GetAxis("Vertical"); // W/S
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical);

        if (inputDirection.sqrMagnitude == 0)
        {
            m_animator.SetFloat("Move", 0);
            return;
        }

        m_animator.SetFloat("Move", 1); // �Ϲ� �̵� �ִϸ��̼�

        // �̵� ���� ���
        inputDirection.Normalize();
        inputDirection = mainCamera.transform.TransformDirection(inputDirection);
        inputDirection.y = 0; // Y�� �̵� ����
        Vector3 moveVector = inputDirection * m_speed * Time.deltaTime;

        // ���� �̵�
        m_rigidbody.MovePosition(m_rigidbody.position + moveVector);
    }

    private void AimAndRotate()
    {
        // ī�޶��� ���� ������ �������� ȸ��
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0; // Y�� ȸ���� �ݿ��ϵ��� ����
        cameraForward.Normalize(); // ����ȭ�Ͽ� ���� ���ͷ� ���

        // �÷��̾��� ȸ�� ����
        Quaternion lookRotation = Quaternion.LookRotation(cameraForward);
        m_rigidbody.rotation = lookRotation;
        // ���� �ִϸ��̼� ����
        if (Input.GetMouseButton(1) || Input.GetMouseButton(0)) // ���콺 Ŭ�� �� ���� ����
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
        // �������� �Ծ����ϴ�.
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