using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// Handles the pool of enemies.
/// </summary>
public class EnemyPool : MonoBehaviour
{
    [SerializeField] GameObject enemyPrefab;

    [SerializeField] private EnemyDetectPlayer detectData;

    private ObjectPool<EnemyPatrol> enemyPools;

    /// <summary>
    /// Creates a new enemy instance by instantiating the enemy prefab and initializing it with the provided data.
    /// </summary>
    /// <returns>The created enemy instance.</returns>
    private EnemyPatrol CreateEnemy()
    {
        var enemy = Instantiate(enemyPrefab).GetComponent<EnemyPatrol>();

        enemy.Init(detectData);

        return enemy;
    }

    /// <summary>
    /// Activates the specified enemy game object.
    /// </summary>
    /// <param name="enemy">The enemy to activate.</param>
    private void GetEnemy(EnemyPatrol enemy)
    {
        enemy.gameObject.SetActive(true);
    }

    /// <summary>
    /// Deactivates the specified enemy game object.
    /// </summary>
    /// <param name="enemy">The enemy to deactivate.</param>
    private void ReleaseEnemy(EnemyPatrol enemy)
    {
        enemy.gameObject.SetActive(false);
    }

    /// <summary>
    /// Destroys the specified enemy game object.
    /// </summary>
    /// <param name="enemy">The enemy to destroy.</param>
    private void DestroyEnemy(EnemyPatrol enemy)
    {
        Destroy(enemy.gameObject);
    }

    /// <summary>
    /// Retrieves an enemy instance from the enemy pool.
    /// </summary>
    /// <returns>An enemy instance from the pool.</returns>
    public EnemyPatrol GetEnemyPool()
    {
        return enemyPools.Get();
    }

    /// <summary>
    /// Initializes the enemy pools using the specified creation, activation, deactivation, and destruction methods.
    /// </summary>
    public void Init()
    {
        enemyPools = new ObjectPool<EnemyPatrol>(CreateEnemy, GetEnemy, ReleaseEnemy, DestroyEnemy);
    }
}