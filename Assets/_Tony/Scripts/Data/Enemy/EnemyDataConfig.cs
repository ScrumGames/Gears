﻿using UnityEngine;
using System.Collections;
using UnityEditor;
using Utils.Data;

namespace Data.Enemy
{
	public class EnemyDataConfig : ScriptableObject
	{
		[SerializeField] private EnemyEspecification[] _enemyConfig;

		public EnemyEspecification[] EnemyConfig { get { return _enemyConfig; } }

		[MenuItem("Assets/Create/Create Enemy Configuration")]
		public static void CreateAsset()
		{
			ScriptableObjectUtility.CreateAsset<EnemyDataConfig> ();
		}
	}
}


