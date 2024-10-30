using UnityEngine;

public class PlayerMovemnet : Character
{
    [SerializeField] private FloatingJoystick m_leftJoystick; // ���� ���̽�ƽ (�̵���)
    [SerializeField] private FloatingJoystick m_rightJoystick; // ������ ���̽�ƽ (ȸ����)
    [SerializeField] private float m_speed = 2f;
    [SerializeField] private float m_rotationSpeed = 2f;
    private Vector3 m_moveVector;
    private Rigidbody m_rigidbody;

    private void Start()
    {
        GameManager.Instance.player = this;
        m_rigidbody = GetComponent<Rigidbody>();
        m_animator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate(); // ������ ���̽�ƽ���� ȸ��
    }

    public override void Move()
    {
        float x = m_leftJoystick.Horizontal;
        float z = m_leftJoystick.Vertical;
        Vector3 inputDirection = new Vector3(x, 0, z);

        if (inputDirection.sqrMagnitude == 0)
        {
            m_animator.SetFloat("Move", 0);
            return;
        }
        m_animator.SetFloat("Move", 1);

        inputDirection = transform.TransformDirection(inputDirection);
        m_moveVector = inputDirection * m_speed * Time.deltaTime;

        m_rigidbody.MovePosition(m_rigidbody.position + m_moveVector);
    }

    private void Rotate()
    {
        // ������ ���̽�ƽ�� Horizontal ���� �������� �¿� ȸ��
        float horizontalRotation = m_rightJoystick.Horizontal;

        if (Mathf.Abs(horizontalRotation) > 0.1f) // ���̽�ƽ�� �������� ��
        {
            Quaternion deltaRotation = Quaternion.Euler(new Vector3(0, horizontalRotation * m_rotationSpeed, 0));
            m_rigidbody.MoveRotation(m_rigidbody.rotation * deltaRotation);
        }
    }
}