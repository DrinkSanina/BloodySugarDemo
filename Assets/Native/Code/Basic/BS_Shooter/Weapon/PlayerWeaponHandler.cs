using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponHandler : MonoBehaviour
{
    [HideInInspector]
    public Camera fpsCam;
    public AWeapon currentWeapon;

    public void Start()
    {
        fpsCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    public void PlayerDied()
    {
        currentWeapon.gameObject.SetActive(false);
        currentWeapon.springArm.GetComponent<Animator>().SetTrigger("Died");
    }

    public Vector3 FPS_ScreenCenter => fpsCam.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0.0f));

    public Vector3 FPS_ScreenDirection => fpsCam.transform.forward;

    
}
