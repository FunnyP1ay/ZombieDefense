using TMPro;
using UnityEngine;

public class PlayerMove : Character
{
    [SerializeField] private float m_speed = 2f; // �̵� �ӵ�
    private Rigidbody m_rigidbody;

    [Header("�÷��̾� ü��UI")]
    public TMP_Text UI_PlayerHP;

    private void Start()
    {
        GameManager.Instance.player = this;
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move(); // WASD�� �̵�
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
        }
        else
        {
            m_animator.SetFloat("Move", 1); // �Ϲ� �̵� �ִϸ��̼�
        }

        // �밢�� �̵� �� �ӵ� ������ �ذ��ϱ� ���� ���� ����ȭ
        if (inputDirection != Vector3.zero)
        {
            inputDirection.Normalize(); // �밢�������� �ӵ� ���� ����

            // ī�޶� ������ �������� �̵�
            inputDirection = Camera.main.transform.TransformDirection(inputDirection);
            inputDirection.y = 0; // Y�� �̵� ����

            // �̵� ���� ���
            Vector3 moveVector = inputDirection * m_speed * Time.deltaTime;

            // �̵� �������� ȸ��
            Quaternion toRotation = Quaternion.LookRotation(inputDirection); // �̵� �������� ȸ��
            m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, toRotation, Time.deltaTime * 10f); // �ε巴�� ȸ��

            // ���� �̵�
            m_rigidbody.MovePosition(m_rigidbody.position + moveVector);
        }
        else
        {
            // inputDirection�� zero�� ���� ī�޶��� �������� ȸ��
            Vector3 cameraForward = Camera.main.transform.forward;
            cameraForward.y = 0f; // Y�� ȸ���� �ݿ��ϵ��� ��
            cameraForward.Normalize(); // ����ȭ�Ͽ� ���� ���ͷ� ���

            Quaternion toRotation = Quaternion.LookRotation(cameraForward); // ī�޶��� �� �������� ȸ��
            m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, toRotation, Time.deltaTime * 10f); // �ε巴�� ȸ��
        }
        // ���콺 ������ Ŭ���� ������ ���콺 ��ġ�� �ٶ󺸰� ��
        if (Input.GetMouseButton(1)) // ������ ���콺 ��ư
        {
            m_animator.SetBool("IsAim", true);
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = Camera.main.WorldToScreenPoint(transform.position).z; // �÷��̾��� z���� �������ݴϴ�.
            Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition); // ȭ����� ���콺 ��ġ�� ���� ��ǥ�� ��ȯ

            // ���콺 ��ġ�� �ٶ󺸰� ȸ��
            Vector3 direction = worldMousePosition - transform.position;
            direction.y = 0; // y�� ȸ���� �ݿ��ϵ��� ����
            Quaternion lookRotation = Quaternion.LookRotation(direction); // ���콺 �������� ȸ��
            m_rigidbody.rotation = lookRotation;
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