using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyScript : MonoBehaviour, IUsesEntityStats
{

    [Header("General Vars")]
    public float moveSpeed = 10f;

    public int attackDmg;

    public float attackSpeed = 1.2f;

    public float detectDistance;

    public float attackDistance;


    [Header("Effects")]
    public AudioClip[] growlClips;

    public AudioClip[] dieSFX;

    public AudioClip[] takeDamageSFX;

    public GameObject[] giblets;

    public GameObject explosion;

    public int gibbageAmount;

    [Header("Characteristics")]
    public bool isRobot;


    [HideInInspector]
    [Header("Other")]
    public bool isAttacking;

    public static GameObject target;

    private AudioSource audioSource;

    private Rigidbody rb;

    private bool isDead;

    private bool obstruction;

	public Action<float> DealtDamage;

    public BasicEntityStatsComponent StatsComponent { get; set; }

    private void Start()
	{
		foreach (GameObject gameObject in GameObject.FindGameObjectsWithTag("Player"))
		{
			if (!gameObject.name.Contains("(Clone)") && EnemyScript.target == null)
			{
				EnemyScript.target = gameObject;
			}
		}
		//UnityEngine.Debug.Log(EnemyScript.target.name + " " + EnemyScript.target.transform.position.ToString());
		this.audioSource = base.GetComponent<AudioSource>();
		this.rb = base.GetComponent<Rigidbody>();
		//base.StartCoroutine("GrowlSound");
		base.StartCoroutine("Attack");
		
	}

	void Awake()
	{
        AccessStats();
		StatsComponent.stats.EntityIsDead += Die;
	}

	private void Update()
	{
		Ray ray = new Ray(base.transform.position, EnemyScript.target.transform.position - base.transform.position);
		RaycastHit raycastHit;
		if (Physics.Raycast(ray, out raycastHit))
		{
			if (raycastHit.transform.tag == "Player")
			{
				this.obstruction = false;
			}
			else
			{
				this.obstruction = true;
			}
		}
		base.transform.LookAt(EnemyScript.target.transform);
		if (this.isDead)
		{
			base.StopCoroutine("Attack");
		}
		if (Vector3.Distance(EnemyScript.target.transform.position, base.transform.position) <= this.attackDistance)
		{
			if (!this.isDead)
			{
				this.isAttacking = true;
			}
		}
		else if (Vector3.Distance(EnemyScript.target.transform.position, base.transform.position) <= this.detectDistance && !this.obstruction)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, EnemyScript.target.transform.position, (float)this.moveSpeed * Time.deltaTime);
			this.isAttacking = false;
		}
	}

	private void Die()
	{
		this.KillEffect();
		UnityEngine.Object.Destroy(base.gameObject, 1.5f);
    }

	private void KillEffect()
	{
        for (int i = 0; i < this.gibbageAmount; i++)
        {
            GameObject gameObject = UnityEngine.Object.Instantiate(this.giblets[UnityEngine.Random.Range(0, this.giblets.Length)], base.transform.position, base.transform.rotation) as GameObject;
            gameObject.GetComponent<Rigidbody>().AddForce(UnityEngine.Random.insideUnitSphere * (float)UnityEngine.Random.Range(400, 600));
        }
        this.isDead = true;
        this.audioSource.PlayOneShot(this.dieSFX[UnityEngine.Random.Range(0, this.dieSFX.Length)]);
        base.transform.GetChild(0).gameObject.SetActive(false);
        //base.GetComponent<SphereCollider>().enabled = false;


        Collider[] colliders = GetComponentsInChildren<Collider>();
		for (int i = 0; i < colliders.Length; i++)
		{
			colliders[i].enabled = false;
		}

	}

	private IEnumerator GrowlSound()
	{
		yield return new WaitForSeconds((float)UnityEngine.Random.Range(5, 15));
		this.audioSource.PlayOneShot(this.growlClips[UnityEngine.Random.Range(0, this.growlClips.Length)]);
		base.StartCoroutine("GrowlSound");
		yield break;
	}

	private IEnumerator Attack()
	{
		if (this.isAttacking  && !this.isDead && !this.obstruction)
		{
			BasicEntityStatsComponent player = target.GetComponent<BasicEntityStatsComponent>();
			if(player == null)
			{
				player = target.GetComponentInParent<BasicEntityStatsComponent>();
			}

			if(player != null && !player.stats.isDead)
			{
				player.stats.RecieveDamage(this.attackDmg, DamageSource.enemy);
				DealtDamage?.Invoke(this.attackDmg);
            }

			
		}
		yield return new WaitForSeconds(this.attackSpeed);
		base.StartCoroutine("Attack");
		yield break;
	}

    public void AccessStats()
    {
		StatsComponent = GetComponent<BasicEntityStatsComponent>();
    }
}
