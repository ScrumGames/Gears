using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgentController))]
public class BaseEnemy : MonoBehaviour 
{
	private NavMeshAgentController navMeshAgentController;

	private void Awake()
	{
		navMeshAgentController = GetComponent<NavMeshAgentController> ();
	}

	private void Start()
	{
		OnReachDestination ();
	}

	private void OnEnable()
	{
		navMeshAgentController.OnReachDestination += OnReachDestination;
	}

	private void OnDisable()
	{
		navMeshAgentController.OnReachDestination -= OnReachDestination;
	}

	private void OnReachDestination()
	{
		Vector3 randomPosition = new Vector3(Random.Range(-25.0f,25.0f),0,Random.Range(-25.0f,25.0f));
		navMeshAgentController.SetDestination (randomPosition);
			
	}




}
