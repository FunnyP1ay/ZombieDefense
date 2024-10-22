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
    public override void Move()
    {
        float x = -m_joystick.Horizontal;
        float z = -m_joystick.Vertical;

        m_moveVector = new Vector3(x, 0, z) * m_speed * Time.deltaTime;
        m_rigidbody.MovePosition(m_rigidbody.position + m_moveVector);

        if (m_moveVector.sqrMagnitude == 0)
            return;
        Quaternion dirQuat = Quaternion.LookRotation(m_moveVector);
        Quaternion moveQuat = Quaternion.Slerp(m_rigidbody.rotation, dirQuat, 0.8f);
        m_rigidbody.MoveRotation(moveQuat);
    }
    private void LateUpdate()
    {
        base.m_animator.SetFloat("Move", m_moveVector.sqrMagnitude);
    }
}
