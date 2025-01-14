using Cinemachine;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMove : Character
{
    [SerializeField] private float m_speed = 2f;
    private Rigidbody m_rigidbody;
    private PlayerWeapon m_weapon;
    [Header("플레이어 체력 UI")]
    public TMP_Text UI_PlayerHP;
    [Header("마우스 감도")]
    public Quaternion nextRotation;
    public float rotationPower = 3f;
    public float rotationLerp = 0.5f;
    private Vector2 inputDirection;
    private Vector2 lookInput;
    [Header("카메라 에임")]
    public GameObject followTransform;
    [Header("카메라 들")]
    public GameObject defaultCamera;
    public GameObject AimCamera;
    private Camera mainCamera;
    // Input System 변수
    public bool isRunning = false;
    public bool isAiming = false;
    public bool isFKey = false;

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
    private void LateUpdate()
    {
        LookAround();
    }
    public void OnMove(InputValue context)
    {
        inputDirection = context.Get<Vector2>();
        print(inputDirection);
    }
    public void OnRun(InputValue context)
    {
        if (context.isPressed)
            isRunning = true;
        else
            isRunning = false;
    }
    public void OnLook(InputValue context)
    {
        lookInput = context.Get<Vector2>();
    }

    public void OnAim(InputValue context)
    {
        if (context.isPressed)
            isAiming = true;
        else
            isAiming = false;
    }
    public void OnFKey(InputValue context)
    {
        if (context.isPressed && isFKey)
        {
            isFKey = false;
            GameManager.Instance.UI_InGame.fKeyPanel.SetActive(false);
            GameManager.Instance.storyManager.NextStory();
        }
    }

    public override void Move()
    {
        m_animator.SetFloat("MoveX", inputDirection.x);
        m_animator.SetFloat("MoveY", inputDirection.y);
        Vector3 direction = new Vector3(inputDirection.x, 0, inputDirection.y).normalized;
        Vector3 currentVelocity = m_rigidbody.linearVelocity;

        if (direction.sqrMagnitude > 0)
        {
            direction = mainCamera.transform.TransformDirection(direction);
            direction.y = 0;
            // 경사면에 맞춘 이동
            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 1.2f, LayerMask.GetMask("Ground")))
            {
                direction = Vector3.ProjectOnPlane(direction, hit.normal).normalized;
            }
            // 이동 속도 설정
            Vector3 moveVelocity = direction * m_speed;
            if (isRunning)
            {
                AudioManager.Instance.MoveSound(1);
                m_animator.SetBool("isRun", true);
                //isRunning 상태일 때 이동 속도 증가
                moveVelocity *= 1.5f; // 속도 1.5배 증가
            }
            else
            {
                AudioManager.Instance.MoveSound(0);
                m_animator.SetBool("isRun", false);
            }

            // 수평 속도만 변경하고, 수직 속도는 그대로 유지
            m_rigidbody.linearVelocity = new Vector3(moveVelocity.x, currentVelocity.y, moveVelocity.z);
        }
        else
        {
            m_animator.SetFloat("MoveX", 0);
            m_animator.SetFloat("MoveY", 0);
            m_animator.SetBool("isRun", false);
            AudioManager.Instance.PlayerMove.Stop();
            // 수평 속도 제거, 중력만 유지
            m_rigidbody.linearVelocity = new Vector3(0, currentVelocity.y, 0);
        }
    }
    private void LookAround()
    {
        // 이코드 출처 https://youtu.be/537B1kJp9YQ?t=236
        float mouseX = lookInput.x;
        float mouseY = lookInput.y;


        // 캐릭터 회전
        followTransform.transform.rotation *= Quaternion.AngleAxis(mouseX * rotationPower, Vector2.up);

        followTransform.transform.rotation *= Quaternion.AngleAxis(-mouseY * rotationPower, Vector2.right);

        var angles = followTransform.transform.localEulerAngles;
        angles.z = 0;
        var angle = followTransform.transform.localEulerAngles.x;
        if (angle > 180 && angle < 340)
        {
            angles.x = 340;
        }
        else if (angle < 180 && angle > 40)
        {
            angles.x = 40;
        }

        followTransform.transform.localEulerAngles = angles;
        nextRotation = Quaternion.Lerp(followTransform.transform.rotation, nextRotation, Time.deltaTime * rotationLerp);


        if (isAiming)
        {
            if (m_weapon.isGun)
            {
                transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
                followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
                m_animator.SetBool("IsAim", true);
                AimCamera.gameObject.SetActive(true);
                defaultCamera.gameObject.SetActive(false);
                return;
            }
            else
            {
                m_animator.SetBool("IsAim", false);
                defaultCamera.gameObject.SetActive(true);
                AimCamera.gameObject.SetActive(false);
            }

        }
        defaultCamera.gameObject.SetActive(true);
        AimCamera.gameObject.SetActive(false);
        m_animator.SetBool("IsAim", false);
        transform.rotation = Quaternion.Euler(0, followTransform.transform.rotation.eulerAngles.y, 0);
        followTransform.transform.localEulerAngles = new Vector3(angles.x, 0, 0);
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