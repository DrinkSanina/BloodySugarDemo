using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Animator))]
public abstract class AWeapon : MonoBehaviour, IUsesEntityStats
{
    public GameObject muzzleflash;

    [Header("Общие свойства оружия")]
    public float gunDamage;
    public float fireRate;
    public float weaponRange;
    public float hitForce;
    public AudioClip[] gunSounds;

    public Transform gunBarrel;
    public Animator springArm;

    public float spread;

    protected WaitForSeconds shotDuration = new WaitForSeconds(0.07f);
    protected AudioSource gunAudio;
    
    protected LineRenderer laserLine;
    protected Animator animator;
    protected float nextFire;
    protected bool canShoot;

    [Header("Дебаг")]
    public bool enablePinkLine = false;

    public BasicEntityStatsComponent StatsComponent { get; set; }

    protected void Start()
    {
        laserLine = GetComponent<LineRenderer>();
        gunAudio = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        laserLine.enabled = false;
        canShoot = true;
        AccessStats();
    }

    protected RaycastHit CastRay(Vector3 origin, Vector3 direction, out Vector3 targetPoint)
    {
        StartCoroutine(ShotEffect());
        canShoot = false;
        StartCoroutine(ShootDelay());

        RaycastHit hit;

        if(enablePinkLine)
            laserLine.SetPosition(0, gunBarrel.position);

        targetPoint = new Vector3(0, 0, 0);
        if (Physics.Raycast(gunBarrel.position, direction, out hit, weaponRange))
            targetPoint = hit.point;
        else
            targetPoint = gunBarrel.position + (direction * weaponRange);

        Vector3 directionWithoutSpread = targetPoint - gunBarrel.position;

        float stat_spread;
        if (StatsComponent.stats.accuracyPercent <= 1.0f)
            stat_spread = spread + spread * (1.0f - StatsComponent.stats.accuracyPercent) * 10;
        else
        {
            stat_spread = spread - (spread * StatsComponent.stats.accuracyPercent * 0.1f);
            if (stat_spread < 0)
                stat_spread = 0;
        }

        float x = Random.Range(-stat_spread, stat_spread);
        float y = Random.Range(-stat_spread, stat_spread);

        Vector3 directionWithSpread = directionWithoutSpread + new Vector3(x, y, 0);
        if (Physics.Raycast(gunBarrel.position, directionWithSpread, out hit, weaponRange))
            targetPoint = hit.point;
        else
            targetPoint = gunBarrel.position + (directionWithSpread * weaponRange);
        
        if (enablePinkLine)
            laserLine.SetPosition(1, targetPoint);

        return hit;
    }

    protected RaycastHit[] CastMultipleRays(Vector3 origin, Vector3 direction, out Vector3[] targetPoints, int count, float multiple_spread)
    {
        RaycastHit[] hits = new RaycastHit[count];

        StartCoroutine(ShotEffect());
        canShoot = false;
        StartCoroutine(ShootDelay());

        if (enablePinkLine)
            laserLine.SetPosition(0, gunBarrel.position);

        Vector3[] directionsWithoutSpread = new Vector3[count];

        for (int i = 0; i < count; i++)
        {
            RaycastHit hit;
            Vector3 targetPoint;

            float x_m = Random.Range(-multiple_spread, multiple_spread);
            float y_m = Random.Range(-multiple_spread, multiple_spread);

            if (Physics.Raycast(gunBarrel.position, direction + new Vector3(x_m, y_m, 0), out hit, weaponRange))
                targetPoint = hit.point;
            else
                targetPoint = gunBarrel.position + ((direction + new Vector3(x_m, y_m, 0)) * weaponRange);

            directionsWithoutSpread[i] = targetPoint - gunBarrel.position;
        }

        float stat_spread;
        if (StatsComponent.stats.accuracyPercent <= 1.0f)
            stat_spread = spread + spread * (1.0f - StatsComponent.stats.accuracyPercent) * 10;
        else
        {
            stat_spread = spread - (spread * StatsComponent.stats.accuracyPercent * 0.1f);
            if (stat_spread < 0)
                stat_spread = 0;
        }

        float x = Random.Range(-stat_spread, stat_spread);
        float y = Random.Range(-stat_spread, stat_spread);

        Vector3[] directionsWithSpread = new Vector3[count];
        targetPoints = new Vector3[count];

        for(int i = 0; i < count; i++)
        {
            directionsWithSpread[i] = directionsWithoutSpread[i] + new Vector3(x, y, 0);

            if (Physics.Raycast(gunBarrel.position, directionsWithSpread[i], out hits[i], weaponRange))
                targetPoints[i] = hits[i].point;
            else
                targetPoints[i] = gunBarrel.position + (directionsWithSpread[i] * weaponRange);

            if (enablePinkLine)
                laserLine.SetPosition(1, targetPoints[i]);
        }

        

        return hits;
    }

    public abstract void PrimaryFire(Vector3 rayOrigin, Vector3 rayDirection);


    protected IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    protected IEnumerator ShotEffect()
    {
        if (muzzleflash != null)
            muzzleflash.SetActive(true);

        if(gunSounds != null)
            gunAudio.PlayOneShot(gunSounds[Random.Range(0, gunSounds.Length)]);

        if(enablePinkLine)
            laserLine.enabled = true;

        animator.SetTrigger("Shoot");
        springArm.SetTrigger("Shoot");
        yield return shotDuration;

        if(enablePinkLine)
            laserLine.enabled = false;

        if (muzzleflash != null)
            muzzleflash.SetActive(false);
    }

    public void AccessStats()
    {
        StatsComponent = GetComponentInParent<BasicEntityStatsComponent>();
    }

    public void RestartWeapon()
    {
        canShoot = true;
        muzzleflash.SetActive(false);
    }
}
