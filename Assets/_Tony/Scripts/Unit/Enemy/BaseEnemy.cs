using UnityEngine;
using System.Collections;
using Gameplay.AI.Movement;

namespace Unit.Enemy
{
	public class BaseEnemy : MonoBehaviour 
	{
		private NavMeshAgentController navMeshAgentController;

		private void Awake()
		{
			navMeshAgentController = GetComponent<NavMeshAgentController> ();
		}

		private void Start()
		{
			Initialize ();
		}

		private void Initialize()
		{
			Vector3 initialPosition = new Vector3 (Random.Range(-25.0f,25.0f),0,Random.Range(-25.0f,25.0f)); 
			navMeshAgentController.SetDestination (initialPosition);

			SeekDestination ();
		}

		private void OnEnable()
		{
			navMeshAgentController.OnReachDestination += SeekDestination;
		}

		private void OnDisable()
		{
			navMeshAgentController.OnReachDestination -= SeekDestination;
		}

		private void SeekDestination()
		{
			Vector3 randomPosition = new Vector3(Random.Range(-25.0f,25.0f),0,Random.Range(-25.0f,25.0f));
			navMeshAgentController.SetDestination (randomPosition);

		}

	}
}


