using UnityEngine;
using System.Collections;

public class EnemyWarper : MonoBehaviour 
{
	[SerializeField] private GameObject[] _enemies;
	[SerializeField] private int _enemySpawNumber;

	private void Start()
	{

		for (int i = 0; i < _enemySpawNumber ; i++)
		{
			GameObject enemyClone = Instantiate (_enemies [Random.Range (0, _enemies.Length)]);
			enemyClone.transform.SetParent (this.transform);
			enemyClone.transform.localPosition = new Vector3(Random.Range (-25.0f, 25.0f), 0, Random.Range (-25.0f, 25.0f));
		}
	}



}
