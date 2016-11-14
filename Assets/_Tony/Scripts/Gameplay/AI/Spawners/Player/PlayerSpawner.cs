using UnityEngine;
using System.Collections;

namespace Gameplay.AI.Spawners.Player
{
	public class PlayerSpawner : MonoBehaviour 
	{
		[SerializeField] private Transform _playerSpawnPositionRoot;
		[SerializeField] private GameObject _playerPrefab;

		private void Start()
		{
			if (_playerPrefab == null)
				return;

			SpawnPlayer ();
		}

		private void SpawnPlayer()
		{
			GameObject playerClone = Instantiate (_playerPrefab);
			playerClone.transform.SetParent (this.transform);

			Transform randomSpawnPosition = GetRandomSpawnPoint ();

			playerClone.transform.localPosition = randomSpawnPosition.position;
		}

		private Transform GetRandomSpawnPoint()
		{
			Transform[] availiableSpawnPosition = _playerSpawnPositionRoot.GetComponentsInChildren<Transform> ();
			return availiableSpawnPosition[Random.Range(0, availiableSpawnPosition.Length)];
		}

	}
}


