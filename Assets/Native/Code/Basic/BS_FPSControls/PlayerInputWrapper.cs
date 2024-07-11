using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputWrapper : MonoBehaviour, IUsesEntityStats
{
    PlayerInputAcitons actions;

    public float MoveSpeed = 8.0f;

    //Чувствительность обзора бэйсикли
    public float RotationSensivity = 1.6f;

    //Ускорение и замедление
    public float SpeedChangeRate = 10.0f;

    public float JumpHeight = 1.2f;
    public float Gravity = -15.0f;

    public float JumpTimeout = 0.1f;
    public float FallTimeout = 0.15f;

    public float DashTimeOut = 1.0f;

    [Header("Параметры приземления")]
    public bool Grounded = true;
    public float GroundedOffset = -0.14f;
    public float GroundedRadius = 0.5f;
    public LayerMask GroundLayers;

    [Header("Камера Cinemachine")]
    public GameObject CinemachineCameraTarget;
    public float TopClamp = 90.0f;
    public float BottomClamp = -90.0f;
    public bool cursorLocked = true;

    private Inventory inventory;
    private PlayerWeaponHandler weaponHandler;

    private float _cinemachineTargetPitch;
    private float _speed;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private float _terminalVelocity = 53.0f;

    private float jumpTimeoutDelta;
    private float fallTimeoutDelta;
    private float dashTimeoutDelta;

    private Vector2 move;
    private Vector2 look;

    //Система ввода
    private CharacterController controller;
    private GameObject mainCamera;
    private DashMove dashComponent;

    private Vector3 motion;
    

    public BasicEntityStatsComponent StatsComponent { get; set; }

    //Испольузется, чтобы мышь не кукурузило
    private const float threshold = 0.01f;


    private void Start()
    {
        dashComponent = GetComponent<DashMove>();
        controller = GetComponent<CharacterController>();
        mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        inventory = GetComponent<Inventory>();
        weaponHandler = GetComponent<PlayerWeaponHandler>();
        jumpTimeoutDelta = JumpTimeout;
        fallTimeoutDelta = FallTimeout;
        AccessStats();
    }

    void Awake()
    {
        actions = new PlayerInputAcitons();

        actions.Player.PrimaryFire.started += OnPrimaryFireDown;
        actions.Player.PrimaryFire.canceled += OnPrimaryFireUp;
        actions.Player.Heal.performed += OnHeal;
        actions.Player.Dash.performed += OnDash;
        actions.Player.SecondaryFire.performed += OnSecondaryFire;
        actions.Player.Interact.performed += OnInteract;
        actions.Player.Reload.performed += OnReload;
    }

    void Update()
    {
        JumpAndGravity();
        GroundedCheck();
        Move();
        ActionCooldowns();
        CountiniousActions();
    }

    private void LateUpdate()
    {
        CameraRotation();
    }

    private void CameraRotation()
    {
        look = actions.Player.Look.ReadValue<Vector2>();
        if (look.sqrMagnitude >= threshold)
        {

            float deltaTimeMultiplier = 1.0f;

            _cinemachineTargetPitch += look.y * RotationSensivity * deltaTimeMultiplier;
            _rotationVelocity = look.x * RotationSensivity * deltaTimeMultiplier;


            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);


            CinemachineCameraTarget.transform.localRotation = Quaternion.Euler(_cinemachineTargetPitch, 0.0f, 0.0f);

            transform.Rotate(Vector3.up * _rotationVelocity);
        }
    }

    private void JumpAndGravity()
    {
        if (Grounded)
        {
            fallTimeoutDelta = FallTimeout;


            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            actions.Player.Jump.performed += ctx =>
            {
                if (jumpTimeoutDelta <= 0.0f)
                {
                    // the square root of H * -2 * G = how much velocity needed to reach desired height
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);
                }
            };

            if (jumpTimeoutDelta >= 0.0f)
            {
                jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            jumpTimeoutDelta = JumpTimeout;

            if (fallTimeoutDelta >= 0.0f)
            {
                fallTimeoutDelta -= Time.deltaTime;
            }
        }

        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }

    private void GroundedCheck()
    {
        Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);
    }

    private void Move()
    {
        float stat_MoveSpeed = MoveSpeed * StatsComponent.stats.movementSpeedPercent;

        float currentHorizontalSpeed = new Vector3(controller.velocity.x, 0.0f, controller.velocity.z).magnitude;

        float speedOffset = 0.1f;

        if (currentHorizontalSpeed < stat_MoveSpeed - speedOffset || currentHorizontalSpeed > stat_MoveSpeed + speedOffset)
        {
            _speed = Mathf.Lerp(currentHorizontalSpeed, stat_MoveSpeed, Time.deltaTime * SpeedChangeRate);

            _speed = Mathf.Round(_speed * 1000f) / 1000f;
        }
        else
        {
            _speed = stat_MoveSpeed;
        }

        move = actions.Player.Move.ReadValue<Vector2>();
        Vector3 inputDirection = new Vector3(move.x, 0.0f, move.y).normalized;

        if (move != Vector2.zero)
        {
            inputDirection = transform.right * move.x + transform.forward * move.y;
        }


        motion = inputDirection.normalized * (_speed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime;
        controller.Move(motion);
    }

    

    private void OnSecondaryFire(InputAction.CallbackContext context)
    {
        if (weaponHandler.currentWeapon is IWPN_HasSecondaryFire)
        {
            IWPN_HasSecondaryFire wp = (IWPN_HasSecondaryFire)weaponHandler.currentWeapon;
            wp.SecondaryFire();
        }
    }

    private void OnHeal(InputAction.CallbackContext context)
    {
        Debug.Log("Лечение...");
    }

    private void OnReload(InputAction.CallbackContext context)
    {
        if (weaponHandler.currentWeapon is IWPN_Reloadable)
        {
            IWPN_Reloadable wp = (IWPN_Reloadable)weaponHandler.currentWeapon;
            wp.Reload();
        }
    }
    private void OnInteract(InputAction.CallbackContext context)
    {
        Debug.Log("Взаимодействие..");
    }

    private void OnDash(InputAction.CallbackContext context)
    {
        if (dashTimeoutDelta <= 0 && move != Vector2.zero)
        {
            Debug.Log("Рывок..");
            Vector3 horizontalMotion = new Vector3(motion.x, 0, motion.z);
            StartCoroutine(dashComponent.Dash(controller, horizontalMotion));
            dashTimeoutDelta = DashTimeOut;
        }
    }

    bool firing;

    

    private void OnPrimaryFireDown(InputAction.CallbackContext context)
    {
        firing = true;
    }
    private void OnPrimaryFireUp(InputAction.CallbackContext context)
    {
        firing = false;
    }

    private void CountiniousActions()
    {
        if (firing)
            weaponHandler.currentWeapon.PrimaryFire(weaponHandler.FPS_ScreenCenter, weaponHandler.FPS_ScreenDirection);
    }
    private void ActionCooldowns()
    {
        //Кулдаун рывка
        if (dashTimeoutDelta >= 0.0f)
        {
            dashTimeoutDelta -= Time.deltaTime;
        }
    }

    private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
    {
        if (lfAngle < -360f) lfAngle += 360f;
        if (lfAngle > 360f) lfAngle -= 360f;
        return Mathf.Clamp(lfAngle, lfMin, lfMax);
    }

    //От этого рано или позно нужно будет избавиться
    //Заглушька для тестирования
    private void OnApplicationFocus(bool hasFocus)
    {
        SetCursorState(cursorLocked);
    }

    private void SetCursorState(bool newState)
    {
        Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
    }

    //Заготовка для звука шагов
    //Вызывается через События Анимации

    //public AudioClip[] FootstepAudioClips;
    //[Range(0, 1)] public float FootstepAudioVolume = 0.5f;
    //private void OnFootstep(AnimationEvent animationEvent)
    //{
    //    if (animationEvent.animatorClipInfo.weight > 0.5f)
    //    {
    //        if (FootstepAudioClips.Length > 0)
    //        {
    //            var index = Random.Range(0, FootstepAudioClips.Length);
    //            AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
    //        }
    //    }
    //}

    void OnEnable()
    {
        actions.Player.Enable();
    }
    void OnDisable()
    {
        actions.Player.Disable();
    }

    public void AccessStats()
    {
        StatsComponent = GetComponent<BasicEntityStatsComponent>();
    }
}
