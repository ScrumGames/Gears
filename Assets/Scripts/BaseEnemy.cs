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




}
