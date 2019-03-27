using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSystem : MonoBehaviour
{

	#region enum and struct
	public enum BossPattern
	{
		MysticLine,
		FireBall,
		ElectricZone,
		ShrinkMysticLines,
		ElectricCone,
		Charge,
		ElectricAOE
	}

	[System.Serializable]
	public struct PatterProbability
	{
		public BossPattern pattern;
		[Range(0.0f, 1.0f)]
		public float probability;
	}
	#endregion

	[Header("[Base Params]")]
	public int baseHP;
	int hp;

	public int actualPhase;
	[Range(0.0f, 1.0f)]
	public float phase2Treshold;
	[Range(0.0f, 1.0f)]
	public float phase3Treshold;
	[Range(0.0f, 1.0f)]
	public float phase4Treshold;

	[Header("[Probability Tables]")]
	public List<PatterProbability> phase1;
	public List<PatterProbability> phase2;
	public List<PatterProbability> phase3;
	public List<PatterProbability> phase4;
	private List<PatterProbability> probabilityTable;

	[Header("[Attack Params]")]
	public float minWaitTime;
	public float maxWaitTime;
	float nextAttack;
	bool isAttacking;

	// Start is called before the first frame update
	void Awake()
    {
		hp = baseHP;
		isAttacking = false;
		actualPhase = 0;
		PhaseTransition();
		nextAttack = Random.Range(minWaitTime, maxWaitTime);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void PhaseTransition()
	{
		actualPhase++;
		switch (actualPhase)
		{
			case 1:
				probabilityTable = phase1;
				//scenaristic start
				break;
			case 2:
				probabilityTable = phase2;
				//infinite mystic line same side / level shrink
				break;
			case 3:
				probabilityTable = phase3;
				//infinite mystic line separation / etc
				break;
			case 4:
				probabilityTable = phase4;
				//fall to ground / level shrink
				break;
		}
	}
}
