using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour 
{
	[SerializeField] private float _spawNewEnemyInterval = 5.0f;
	[SerializeField] private int _maxEnemies = 10;
	[SerializeField] private BaseEnemy _enemyPrefab;

	private List<BaseEnemy> _aliveEnemies = new List<BaseEnemy>();
	private bool _alive;

	private void Start()
	{
		StartCoroutine (SpawEnemyCoroutine ());
	}

	private IEnumerator SpawEnemyCoroutine()
	{
		_alive = true;

		while (_alive) 
		{
			if (CanSpawnMoreEnemies())
				SpawNewEnemy ();

			yield return new WaitForSeconds (_spawNewEnemyInterval);
		}

	}

	private bool CanSpawnMoreEnemies()
	{
		if (_aliveEnemies.Count < _maxEnemies)
			return true;

		return false;
	}

	private void SpawNewEnemy()
	{
		BaseEnemy enemyClone = Instantiate (_enemyPrefab);
		enemyClone.transform.SetParent (this.transform);
		enemyClone.transform.localPosition = Vector3.zero;

		_aliveEnemies.Add (enemyClone);


	}


}
