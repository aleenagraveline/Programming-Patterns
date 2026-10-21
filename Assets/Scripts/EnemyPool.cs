using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
	[SerializeField] private EnemyController enemyPrefab;
	[SerializeField] private int startSize;
	[SerializeField] private float spawnTime;
	[SerializeField] private float spawnRadius = 5f;
	
	private Transform player;
	private List<EnemyController> pool = new List<EnemyController>();
	private Quaternion spawnRotation;
	private float timer;

	private void Awake()
	{
		spawnRotation = Quaternion.Euler(0f, 0f, 0f);
		player = GameObject.FindGameObjectWithTag("Player").transform;

		// Add initial enemies
		for (int i = 0; i < startSize; i++)
		{
			AddEnemy();
			GetEnemy();
		}

		Initialize();
	}

    private void Update()
    {
		timer += Time.deltaTime;

		if (timer >= spawnTime)
		{
			GetEnemy();

			timer = 0f;
		}

		bool poolIsEmpty = true;
		for(int i=0; i<pool.Count; i++)
        {
			if(pool[i].gameObject.activeSelf)
            {
				poolIsEmpty = false;
            }
        }
		if(poolIsEmpty)
        {
			for (int i = 0; i < startSize; i++)
			{
				GetEnemy();
			}
		}
	}

    public void Initialize()
    {
		Player.OnDeath += DisableAllEnemies;
    }

	public void Dispose()
    {
		Player.OnDeath -= DisableAllEnemies;
    }

	public void DisableAllEnemies()
    {
		for(int i=0; i<pool.Count; i++)
        {
			pool[i].Clear();
        }

		Dispose();
		Destroy(this.gameObject);
    }

    // Creates an enemy and add it to the pool
    private EnemyController AddEnemy()
	{
		EnemyController newEnemy = Instantiate(enemyPrefab);
		newEnemy.gameObject.SetActive(false);
		pool.Add(newEnemy);
		return newEnemy;
	}

	// Get inactive enemy from pool, or create new enemy if needed
	public EnemyController GetEnemy()
	{
		Vector3 position = SetSpawnPosition();

		for (int i = 0; i < pool.Count; i++)
		{
			if (!pool[i].gameObject.activeSelf)
			{
				pool[i].transform.SetPositionAndRotation(position, spawnRotation);
				pool[i].Spawn();
				return pool[i];
			}
		}

		EnemyController newEnemy = AddEnemy();
		newEnemy.transform.SetPositionAndRotation(position, spawnRotation);
		newEnemy.Spawn();
		return newEnemy;
	}

	public Vector3 SetSpawnPosition()
    {
		float angle = Random.Range(0f, 360f) * Mathf.Deg2Rad;

		Vector3 spawnPos = player.position + new Vector3(
			Mathf.Cos(angle) * spawnRadius,
			0f,
			Mathf.Sin(angle) * spawnRadius
		);

		spawnPos.x = Mathf.Clamp(spawnPos.x, -10f, 10f);
		spawnPos.z = Mathf.Clamp(spawnPos.z, -10f, 10f);

		return spawnPos;
	}
}