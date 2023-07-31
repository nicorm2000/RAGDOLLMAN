using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the spawn of the enemies.
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    [Header ("Enemy Pool Configuaration")]
    [SerializeField] private EnemyPool enemyPools;

    [SerializeField] private List<EnemyData> enemiesData;

    /// <summary>
    /// Starts the enemy spawner by initializing the enemy pools and calling the Spawner method.
    /// </summary>
    private void Start()
    {
        enemyPools.Init();

        Spawner();
    }

    /// <summary>
    /// Spawns enemies by retrieving an enemy from the enemy pool and setting its patrol points and initial position.
    /// </summary>
    private void Spawner()
    {
        for (int i = 0; i < enemiesData.Count; i++)
        {
            EnemyPatrol enemy = enemyPools.GetEnemyPool();

            enemy.points = enemiesData[i].targets;

            enemy.transform.position = enemy.points[0].position;
        }
    }
}