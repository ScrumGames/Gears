using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Gameplay.AI.Spawners.Enemy
{
	public class EnemySpawner : MonoBehaviour 
	{
		[SerializeField] private float _spawNewEnemyInterval = 5.0f;
		[SerializeField] private int _maxEnemies = 10;
		[SerializeField] private GameObject[] _enemies;

		private List<GameObject> _aliveEnemies = new List<GameObject>();
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
			GameObject enemyClone = Instantiate (_enemies[Random.Range(0,_enemies.Length)]);
			enemyClone.transform.SetParent (this.transform);
			enemyClone.transform.localPosition = Vector3.zero;

			_aliveEnemies.Add (enemyClone);

		}
	}
}

