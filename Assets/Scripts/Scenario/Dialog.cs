using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class Dialog : ScriptableObject
{
	public enum BonusCharacter
	{
		None,
		Entity,
		Alan
	};

	public enum Character
	{
		Mia,
		Raka,
		Entity,
		Alan
	};

	[Serializable]
	public struct DialogElement
	{
		public Character characterTalking;
		public string replica;
	}

	public BonusCharacter bonusCharacter;
	public List<DialogElement> dialogElementList;
}
