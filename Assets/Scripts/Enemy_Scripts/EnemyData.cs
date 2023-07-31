using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
/// <summary>
/// Represents data for an enemy.
/// </summary>
public class EnemyData
{
    public List<Transform> targets = new List<Transform>();
}