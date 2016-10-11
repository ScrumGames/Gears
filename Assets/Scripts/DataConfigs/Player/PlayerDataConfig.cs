using UnityEngine;
using System.Collections;
using UnityEditor;

public class PlayerDataConfig : ScriptableObject
{
	[SerializeField] private PlayerLevelSpecification[] _playerProgression;

	public PlayerLevelSpecification[] PlayerProgression { get { return _playerProgression; } }


	[MenuItem("Assets/Create/Create Player Configuration")]
	public static void CreateAsset()
	{
		ScriptableObjectUtility.CreateAsset<PlayerDataConfig> ();
	}
}
