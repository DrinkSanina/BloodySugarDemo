using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class FPSWalker : MonoBehaviour, IUsesEntityStats
{
    PlayerInputAcitons actions;

    public float walkSpeed = 6f;

    public bool limitDiagonalSpeed = true;

    public float jumpSpeed = 8f;

    public float gravity = 20f;

    public float fallingDamageThreshold = 10f;

    public bool slideWhenOverSlopeLimit;

    public bool slideOnTaggedObjects;

    public float slideSpeed = 12f;

    public bool airControl;

    public float antiBumpFactor = 0.75f;

    public int antiBunnyHopFactor = 1;

    private Vector3 moveDirection = Vector3.zero;

	public bool sent_flying;

	
    public bool grounded;

    private CharacterController controller;

    private Transform myTransform;

    private float speed;

    private RaycastHit hit;

    private float fallStartLevel;

    private bool falling;

    private float slideLimit;

    private float rayDistance;

    private Vector3 contactPoint;

    private bool playerControl;

    private int jumpTimer;

    public AudioClip[] footstepSounds;

    public AudioClip[] hardFootstepSounds;

    public AudioSource audioSource;

    public GameObject inGameUI;

    public GameObject pauseUI;

    private Animator anim;

    private bool isWalkSoundPlaying;

    private bool isOnHardSurface;

    public static bool isPaused;

    private float dashTimeoutDelta;
    private DashMove dashComponent;
    public float DashTimeOut = 1.0f;
	[SerializeField]
	private bool firing;
	private bool interacting;
    private PlayerWeaponHandler weaponHandler;
	private Cleaner cleaner;

	

    private void Start()
	{
		this.controller = base.GetComponent<CharacterController>();
		this.myTransform = base.transform;
		this.speed = this.walkSpeed;
		this.rayDistance = this.controller.height * 0.5f + this.controller.radius;
		this.slideLimit = this.controller.slopeLimit - 0.1f;
		this.jumpTimer = this.antiBunnyHopFactor;
		//this.anim = base.GetComponent<Animator>();
		dashComponent = GetComponent<DashMove>();
        weaponHandler = GetComponent<PlayerWeaponHandler>();
		cleaner = GetComponentInChildren<Cleaner>();
        AccessStats();
		StatsComponent.stats.EntityIsDead += OnPlayerDeath;

		
	}

	void OnPlayerDeath()
	{
		firing = false;
		interacting = false;
		weaponHandler.currentWeapon.RestartWeapon();
		weaponHandler.PlayerDied();
	}

    void Awake()
    {
        actions = new PlayerInputAcitons();
        actions.Player.Dash.performed += OnDash;
        actions.Player.SecondaryFire.performed += OnSecondaryFire;
        actions.Player.PrimaryFire.started += OnPrimaryFireDown;
        actions.Player.PrimaryFire.canceled += OnPrimaryFireUp;

		actions.Player.Interact.started += OnInteractDown;
		actions.Player.Interact.canceled += OnInteractUp;


        //actions.Player.Heal.performed += OnHeal;
        //actions.Player.Interact.performed += OnInteract;
        //actions.Player.Reload.performed += OnReload;
    }

    void OnEnable()
    {
        actions.Player.Enable();
    }
    void OnDisable()
    {
        actions.Player.Disable();
    }

	Vector2 movevec;

    public BasicEntityStatsComponent StatsComponent { get ; set; }

	bool ragdoll_cooldown = false;
	bool cooldownCoroutineWorking = false;
	
	IEnumerator RagdollCooldown()
	{
		cooldownCoroutineWorking = true;
		yield return new WaitForSeconds(0.7f);
		ragdoll_cooldown = true;
		cooldownCoroutineWorking = false;
	}

    private void FixedUpdate()
	{
		if (StatsComponent.stats.isDead)
			return;

		if(sent_flying)
		{
			if(!ragdoll_cooldown)
			{
				if (!cooldownCoroutineWorking)
					StartCoroutine(RagdollCooldown());

				return;
			}

            RaycastHit hit;
            if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f))
            {
                sent_flying = false;
				Destroy(GetComponent<Rigidbody>());
				//GetComponent<Rigidbody>().isKinematic = true;
                controller.enabled = true;
				ragdoll_cooldown = false;
            }
			return;
        }

		movevec = actions.Player.Move.ReadValue<Vector2>();

		float axis = movevec.x;
		float axis2 = movevec.y;

		float num = (axis == 0f || axis2 == 0f || !this.limitDiagonalSpeed) ? 1f : 0.7071f;
		if ((axis != 0f || axis2 != 0f) && !Input.GetMouseButton(0))
		{
			//this.anim.SetBool("isMoving", true);
		}
		else if (axis == 0f && axis2 == 0f)
		{
			//this.anim.SetBool("isMoving", false);
		}
		if ((axis != 0f || axis2 != 0f) && this.grounded)
		{
			if (!this.isWalkSoundPlaying)
			{
				//base.StartCoroutine("playFootstep");
				//this.isWalkSoundPlaying = true;
			}
		}
		else
		{
			//base.StopCoroutine("playFootstep");
			//this.isWalkSoundPlaying = false;
		}
		if (this.grounded)
		{
			bool flag = false;
			if (Physics.Raycast(this.myTransform.position, -Vector3.up, out this.hit, this.rayDistance))
			{
				if (Vector3.Angle(this.hit.normal, Vector3.up) > this.slideLimit)
				{
					flag = true;
				}
			}
			else
			{
				Physics.Raycast(this.contactPoint + Vector3.up, -Vector3.up, out this.hit);
				if (Vector3.Angle(this.hit.normal, Vector3.up) > this.slideLimit)
				{
					flag = true;
				}
			}
			if (this.falling)
			{
				this.falling = false;
				if (this.myTransform.position.y < this.fallStartLevel - this.fallingDamageThreshold)
				{
					this.FallingDamageAlert(this.fallStartLevel - this.myTransform.position.y);
				}
			}
			this.speed = walkSpeed * StatsComponent.stats.movementSpeedPercent;
			if ((flag && this.slideWhenOverSlopeLimit) || (this.slideOnTaggedObjects && this.hit.collider.tag == "Slide"))
			{
				Vector3 normal = this.hit.normal;
				this.moveDirection = new Vector3(normal.x, -normal.y, normal.z);
				Vector3.OrthoNormalize(ref normal, ref this.moveDirection);
				this.moveDirection *= this.slideSpeed;
				this.playerControl = false;
			}
			else
			{
				this.moveDirection = new Vector3(axis * num, -this.antiBumpFactor, axis2 * num);
				this.moveDirection = this.myTransform.TransformDirection(this.moveDirection) * this.speed;
				this.playerControl = true;
			}

			actions.Player.Jump.performed += ctx =>
			{
				this.jumpTimer++;
			};

			if (this.jumpTimer >= this.antiBunnyHopFactor)
			{
				Jump(jumpSpeed);
			}
		}
		else
		{
			if (!this.falling)
			{
				this.falling = true;
				this.fallStartLevel = this.myTransform.position.y;
			}
			if (this.airControl && this.playerControl)
			{
				this.moveDirection.x = axis * this.speed * num;
				this.moveDirection.z = axis2 * this.speed * num;
				this.moveDirection = this.myTransform.TransformDirection(this.moveDirection);
			}
		}
		this.moveDirection.y = this.moveDirection.y - this.gravity * Time.deltaTime;
		this.grounded = ((this.controller.Move(this.moveDirection * Time.deltaTime) & CollisionFlags.Below) != CollisionFlags.None);
	}


    public void Jump(float jump_speed)
	{
        this.moveDirection.y = jump_speed;
        this.jumpTimer = 0;
    }

	private void Update()
	{
		if (this.grounded)
		{
			this.speed = walkSpeed * StatsComponent.stats.movementSpeedPercent;
		}
		ActionCooldowns();
		CountiniousActions();
    }

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		this.contactPoint = hit.point;
	}



	private void FallingDamageAlert(float fallDistance)
	{
		
	}

	private IEnumerator playFootstep()
	{
		yield return new WaitForSeconds(0.4f);
		this.audioSource.pitch = UnityEngine.Random.Range(0.85f, 1.15f);
		if (!this.isOnHardSurface)
		{
			this.audioSource.PlayOneShot(this.footstepSounds[UnityEngine.Random.Range(0, this.footstepSounds.Length)]);
		}
		else
		{
			this.audioSource.PlayOneShot(this.hardFootstepSounds[UnityEngine.Random.Range(0, this.hardFootstepSounds.Length)]);
		}
		base.StartCoroutine("playFootstep");
		yield break;
	}

    private void OnDash(InputAction.CallbackContext context)
    {
        if (dashTimeoutDelta <= 0 && movevec != Vector2.zero)
        {
            Debug.Log("Рывок..");
            Vector3 horizontalMotion = new Vector3(moveDirection.x, 0, moveDirection.z);
            StartCoroutine(dashComponent.Dash(controller, horizontalMotion));
            dashTimeoutDelta = DashTimeOut;
        }
    }

    private void ActionCooldowns()
    {
        //Кулдаун рывка
        if (dashTimeoutDelta >= 0.0f)
        {
            dashTimeoutDelta -= Time.deltaTime;
        }
    }

    public void AccessStats()
    {
        StatsComponent = GetComponent<BasicEntityStatsComponent>();
    }

    private void OnPrimaryFireDown(InputAction.CallbackContext context)
    {
        firing = true;
    }
    private void OnPrimaryFireUp(InputAction.CallbackContext context)
    {
        firing = false;
    }

    private void OnInteractDown(InputAction.CallbackContext context)
    {
        interacting = true;
    }
    private void OnInteractUp(InputAction.CallbackContext context)
    {
        interacting = false;
		cleaner.StopEmitting();
    }

    private void CountiniousActions()
    {
        if (firing && !StatsComponent.stats.isDead)
            weaponHandler.currentWeapon.PrimaryFire(weaponHandler.FPS_ScreenCenter, weaponHandler.FPS_ScreenDirection);

		if (interacting && !StatsComponent.stats.isDead)
			cleaner.PrimaryFire(weaponHandler.FPS_ScreenCenter, weaponHandler.FPS_ScreenDirection);
    }

    private void OnSecondaryFire(InputAction.CallbackContext context)
    {
        if (weaponHandler.currentWeapon is IWPN_HasSecondaryFire)
        {
            IWPN_HasSecondaryFire wp = (IWPN_HasSecondaryFire)weaponHandler.currentWeapon;
            wp.SecondaryFire();
        }
    }
}
