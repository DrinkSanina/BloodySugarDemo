using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUsesEntityStats
{
    public BasicEntityStatsComponent StatsComponent { get; set; }
    public void AccessStats();
}
