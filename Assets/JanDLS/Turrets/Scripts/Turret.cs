using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Turret : MonoBehaviour, IWPN_Projectile
{
    private GameObject target;
    private Transform turningPoint;

    public Transform barrel;

    [field: SerializeField]
    public GameObject Bullet { get; set; }

    [field: SerializeField]
    public float ShootForce { get; set; }

    [field: SerializeField]
    public float UpwardForce { get; set; }

    public float shootRate = 2f;

    public float chanceToShootEffectBullet = 0.5f;

    public List<EntityEffect> PossibleEffects = new List<EntityEffect>();

    bool canShoot = true;

    // Start is called before the first frame update
    void Start()
    {
        turningPoint = this.transform.Find("TurningPoint");
    }

    void Update()
    {
        if (target != null)
        {
            turningPoint.LookAt(target.transform);
            if(canShoot)
            {
                Shoot();
            }
        }
        
    }


    private void Shoot()
    {
        GameObject bullet = ((IWPN_Projectile)this).SpawnProjectile(barrel.position, barrel.forward, barrel.up);
        EffectCastingProjectile ECP = bullet.GetComponent<EffectCastingProjectile>();
        
        if (Random.Range(0, 1) < chanceToShootEffectBullet)
        {
            ECP.instigator.possibleEffects.Add(PossibleEffects[Random.Range(0, PossibleEffects.Count)]);
        }

        

        canShoot = false;
        StartCoroutine(Cooldown());
    }

    IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(shootRate);
        canShoot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
            target = (GameObject)other.gameObject;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
            target = null;
    }
}
