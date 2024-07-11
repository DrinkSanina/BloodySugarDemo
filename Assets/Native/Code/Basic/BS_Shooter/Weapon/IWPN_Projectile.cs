using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWPN_Projectile
{

    public GameObject Bullet { get; set; }

    public float ShootForce { get; set; }

    public float UpwardForce { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="origin"></param>
    /// <param name="direction"></param>
    /// <param name="verticalDirection">Camera.transform.up</param>
    public GameObject SpawnProjectile(Vector3 origin, Vector3 direction, Vector3 verticalDirection)
    {
        GameObject currentBullet = GameObject.Instantiate(Bullet, origin, Quaternion.Euler(0,0,90));
        currentBullet.transform.right = direction.normalized;

        //Add forces to bullet
        currentBullet.GetComponent<Rigidbody>().AddForce(direction.normalized * ShootForce, ForceMode.Impulse);
        currentBullet.GetComponent<Rigidbody>().AddForce(verticalDirection * UpwardForce, ForceMode.Impulse);

        return currentBullet;
    }
    
}
