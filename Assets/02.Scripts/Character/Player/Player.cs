using UnityEngine;

public class Player : Character
{
    [SerializeField] private FloatingJoystick m_joystick;
    [SerializeField] private float m_speed = 2f;
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
    }

    [SerializeField] private float m_rotationSpeed = 2f;

    public override void Move()
    {
        float x = m_joystick.Horizontal;
        float z = m_joystick.Vertical;
        Vector3 inputDirection = new Vector3(x, 0, z);

        if (inputDirection.sqrMagnitude == 0)
            return;

        inputDirection = transform.TransformDirection(inputDirection);
        m_moveVector = inputDirection * m_speed * Time.deltaTime;

        Quaternion targetRotation = Quaternion.LookRotation(m_moveVector);
        float angle = Vector3.SignedAngle(transform.forward, m_moveVector, Vector3.up);

        if (angle < 120 && angle > -120) // 조이스틱이 앞일 때
        {
            m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, targetRotation, m_rotationSpeed * Time.deltaTime);
            m_rigidbody.MovePosition(m_rigidbody.position + m_moveVector);
        }
        else 
        {
            //m_rigidbody.rotation = Quaternion.Slerp(m_rigidbody.rotation, targetRotation, m_rotationSpeed/2 * Time.deltaTime);
            //TODO 뒤로 걷는 애니메이션
            m_rigidbody.MovePosition(m_rigidbody.position + m_moveVector);
        }
    }

    private void LateUpdate()
    {
        base.m_animator.SetFloat("Move", m_moveVector.sqrMagnitude);
    }
}