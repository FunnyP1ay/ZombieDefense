//using Cinemachine;
//using TMPro;
//using UnityEngine;
//using UnityEngine.InputSystem;

//public class PlayerMove : Character
//{
//    [SerializeField] private float m_speed = 2f; 
//    private Rigidbody m_rigidbody;
//    private PlayerWeapon m_weapon;
//    [Header("플레이어 체력 UI")]
//    public TMP_Text UI_PlayerHP;

//    private Camera mainCamera;
//    private void Start()
//    {
//        GameManager.Instance.player = this;
//        m_rigidbody = GetComponent<Rigidbody>();
//        m_rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
//        m_animator = GetComponent<Animator>();
//        m_weapon = GetComponent<PlayerWeapon>();
//        mainCamera = Camera.main;
//    }
//    private void FixedUpdate()
//    {
//        Move(); 
//    }

//    private void Update()
//    {
//        AimAndRotate(); 
//    }
//    private void LateUpdate()
//    {
//        AimAndRotate();
//    }
//    public override void Move()
//    {
//        float horizontal = Input.GetAxis("Horizontal");
//        float vertical = Input.GetAxis("Vertical");
//        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;

//        Vector3 currentVelocity = m_rigidbody.velocity; // 기존 속도

//        if (inputDirection.sqrMagnitude > 0)
//        {
//            m_animator.SetFloat("MoveX", horizontal);
//            m_animator.SetFloat("MoveY", vertical);
//            inputDirection = mainCamera.transform.TransformDirection(inputDirection);
//            inputDirection.y = 0;

//            // 경사면에 맞춘 이동
//            RaycastHit hit;
//            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f, LayerMask.GetMask("Ground")))
//            {
//                inputDirection = Vector3.ProjectOnPlane(inputDirection, hit.normal).normalized;
//            }

//            Vector3 moveVelocity = inputDirection * m_speed;
//            Vector3 currentHorizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
//            Vector3 velocityChange = moveVelocity - currentHorizontalVelocity;

//            if (Input.GetKey(KeyCode.LeftShift))
//            {
//                AudioManager.Instance.MoveSound(1);
//                m_animator.SetBool("isRun", true);
//                m_rigidbody.AddForce(velocityChange * 2f, ForceMode.VelocityChange);
//            }
//            else
//            {
//                AudioManager.Instance.MoveSound(0);
//                m_animator.SetBool("isRun", false);
//                m_rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
//            }
//        }
//        else
//        {
//            m_animator.SetFloat("MoveX", 0);
//            m_animator.SetFloat("MoveY", 0);
//            AudioManager.Instance.PlayerMove.Stop();

//            // 수평 속도 제거, 중력만 유지
//            m_rigidbody.velocity = new Vector3(0, currentVelocity.y, 0);
//        }
//    }
//    private void AimAndRotate()
//    {

//        Vector3 cameraForward = mainCamera.transform.forward;
//        cameraForward.y = 0; 
//        cameraForward.Normalize(); 

//        Quaternion lookRotation = Quaternion.LookRotation(cameraForward);
//        m_rigidbody.rotation = lookRotation;

//        if (Input.GetMouseButton(1) || Input.GetMouseButton(0)) 
//        {
//            if (m_weapon.isGun)
//                m_animator.SetBool("IsAim", true);
//            else
//                m_animator.SetBool("IsAim", false);
//        }
//        else
//        {
//            m_animator.SetBool("IsAim", false);
//        }
//    }

//    public override void TakeDamage(float damage)
//    {

//        health -= damage;
//        GameManager.Instance.playerCityData.UIUpdate(health, UI_PlayerHP);
//        if (health <= 0)
//        {
//            GameLose();
//        }
//    }

//    public void GameLose()
//    {
//        GameManager.Instance.UI_InGame.GameLose();
//    }
//}
using Cinemachine;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : Character
{
    [SerializeField] private float m_speed = 2f;
    private Rigidbody m_rigidbody;
    private PlayerWeapon m_weapon;
    [Header("플레이어 체력 UI")]
    public TMP_Text UI_PlayerHP;

    private Camera mainCamera;
    // Input System 변수
    private Vector2 inputDirection;
    public bool isRunning;
    public bool isAiming;

    private void Start()
    {
        GameManager.Instance.player = this;
        m_rigidbody = GetComponent<Rigidbody>();
        m_rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        m_animator = GetComponent<Animator>();
        m_weapon = GetComponent<PlayerWeapon>();
        mainCamera = Camera.main;
    }
    private void FixedUpdate()
    {
        Move();
    }
    private void Update()
    {
        AimAndRotate();
    }
    private void LateUpdate()
    {
        AimAndRotate();
    }
    public void OnMove(InputValue context)
    {
        inputDirection = context.Get<Vector2>();
        print(inputDirection);
    }

    public void OnRun(InputValue context)
    {
        if(context.isPressed)
            isRunning = true;
        else
            isRunning = false;
    }

    public void OnAim(InputValue context)
    {
        if(context.isPressed)
            isAiming = true;
        else
            isAiming = false;
    }

    public override void Move()
    {
        Vector3 direction =new Vector3(inputDirection.x, 0, inputDirection.y).normalized;

        Vector3 currentVelocity = m_rigidbody.velocity;

        if (direction.sqrMagnitude > 0)
        {
            m_animator.SetFloat("MoveX", inputDirection.x);
            m_animator.SetFloat("MoveY", inputDirection.y);
            direction = mainCamera.transform.TransformDirection(direction);
            direction.y = 0;

            // 경사면에 맞춘 이동
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f, LayerMask.GetMask("Ground")))
            {
                direction = Vector3.ProjectOnPlane(direction, hit.normal).normalized;
            }

            Vector3 moveVelocity = direction * m_speed;
            Vector3 currentHorizontalVelocity = new Vector3(currentVelocity.x, 0, currentVelocity.z);
            Vector3 velocityChange = moveVelocity - currentHorizontalVelocity;

            if (isRunning)
            {
                AudioManager.Instance.MoveSound(1);
                m_animator.SetBool("isRun", true);
                m_rigidbody.AddForce(velocityChange * 2f, ForceMode.VelocityChange);
            }
            else
            {
                AudioManager.Instance.MoveSound(0);
                m_animator.SetBool("isRun", false);
                m_rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }
        else
        {
            m_animator.SetFloat("MoveX", 0);
            m_animator.SetFloat("MoveY", 0);
            AudioManager.Instance.PlayerMove.Stop();

            // 수평 속도 제거, 중력만 유지
            m_rigidbody.velocity = new Vector3(0, currentVelocity.y, 0);
        }
    }

    private void AimAndRotate()
    {
        Vector3 cameraForward = mainCamera.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        Quaternion lookRotation = Quaternion.LookRotation(cameraForward);
        m_rigidbody.rotation = lookRotation;

        if (isAiming)
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