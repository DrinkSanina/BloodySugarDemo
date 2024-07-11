using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IWPN_Reloadable
{
    public int ClipSize { get; set; }
    public int MaxAmmo { get; set; }

    public void Reload()
    {
        Debug.Log("Reloading...");
    }
}
