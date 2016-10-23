using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class EnemyEspecification
{
	public string name;
	public int damage;
	public int health;
	public float movementSpeed;
	public EnemyType enemyType;
}