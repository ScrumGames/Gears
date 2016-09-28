using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgentController))]
public class EnemyWarper : MonoBehaviour 
{
	[SerializeField] NavMeshAgentController _navMeshAgentController;
	[SerializeField] GameObject[] _enemies;

	private void Awake()
	{
		_navMeshAgentController = GetComponent<NavMeshAgentController> ();
	}

	private void Start()
	{
		WarperEnemies ();
	}

	private void WarperEnemies()
	{
		for (int i = 0; i >= _enemies.Length; i++) 
		{
			
		}
	}

}
