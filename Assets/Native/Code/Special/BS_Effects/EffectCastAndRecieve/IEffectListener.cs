using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEffectListener
{
    public EffectReciever EffectReciever { get; set; }

    public void OnEffectAdded();

    public void OnEffectRemoved();

}
