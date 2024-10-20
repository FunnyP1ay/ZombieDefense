using UnityEngine;

public class Player : Character
{
    [SerializeField] private FloatingJoystick m_joystick;
    [SerializeField] private float m_speed = 2f;
    private Vector3 m_moveVector;
    private void Start()
    {
        GameManager.Instance.player = this;
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
        base.m_rigidbody.MovePosition(base.m_rigidbody.position + m_moveVector);

        if (m_moveVector.sqrMagnitude == 0)
            return;
        Quaternion dirQuat = Quaternion.LookRotation(m_moveVector);
        Quaternion moveQuat = Quaternion.Slerp(base.m_rigidbody.rotation, dirQuat, 0.3f);
        m_rigidbody.MoveRotation(moveQuat);
    }
    private void LateUpdate()
    {
        base.m_animator.SetFloat("Move", m_moveVector.sqrMagnitude);
    }
}
