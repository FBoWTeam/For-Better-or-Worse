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
	
	void Awake()
    {
		hp = baseHP;
		isAttacking = false;
		actualPhase = 0;
		checkPhaseTransition();
    }

    // Update is called once per frame
    void Update()
    {
		if(!GameManager.gameManager.isPaused)
		{
			checkPhaseTransition();

			if (Time.time == nextAttack)
			{
				DoPattern();
			}


		}
    }

	public void checkPhaseTransition()
	{
		switch (actualPhase)
		{
			case 0:
				actualPhase++;
				Debug.Log("Passage phase 1");
				probabilityTable = phase1;
				nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
				//scenaristic start
				break;
			case 1:
				if(hp <= phase2Treshold)
				{
					actualPhase++;
					Debug.Log("Passage phase 2");
					probabilityTable = phase2;
					nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
					//infinite mystic line same side / level shrink
				}
				break;
			case 2:
				if (hp <= phase3Treshold)
				{
					actualPhase++;
					Debug.Log("Passage phase 3");
					probabilityTable = phase3;
					nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
					//infinite mystic line separation / etc
				}
				break;
			case 3:
				if (hp <= phase4Treshold)
				{
					actualPhase++;
					Debug.Log("Passage phase 4");
					probabilityTable = phase4;
					nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
					//fall to ground / level shrink
				}
				break;
			case 4:
				if (hp <= 0.0f)
				{
					Debug.Log("DED");
				}
				break;
		}
	}

	public void DoPattern()
	{

	}
}
