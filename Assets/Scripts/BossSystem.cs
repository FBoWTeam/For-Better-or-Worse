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
	public struct PatternProbability
	{
		public BossPattern pattern;
		[Range(0.0f, 1.0f)]
		public float probability;
	}

	#endregion

	[Header("[Base Params]")]
	public int baseHP;
	public int hp;

	public int actualPhase;
	[Range(0.0f, 1.0f)]
	public float phase2Threshold;
	[Range(0.0f, 1.0f)]
	public float phase3Threshold;
	[Range(0.0f, 1.0f)]
	public float phase4Threshold;

	[Header("[Probability Tables]")]
	public List<PatternProbability> phase1;
	public List<PatternProbability> phase2;
	public List<PatternProbability> phase3;
	public List<PatternProbability> phase4;
	private List<PatternProbability> probabilityTable;

	[Header("[Attack Params]")]
	GameObject aimedPlayer;
	public float minWaitTime;
	public float maxWaitTime;
	float nextAttack;
	bool isAttacking;

	//======================================================================================== AWAKE AND UPDATE

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
		if(!GameManager.gameManager.isPaused && !isAttacking)
		{
			checkPhaseTransition();

			if (Time.time >= nextAttack)
			{
				SetFocus();
				LaunchPattern(RandomPattern());
				nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
			}
		}
    }

	//======================================================================================== SET FOCUS

	/// <summary>
	/// set the focus on one of the player at random
	/// </summary>
	public void SetFocus()
	{
		int rand = Random.Range(0, 2);
		if(rand == 0)
		{
			Debug.Log("Aim Player 1");
			aimedPlayer = GameManager.gameManager.player1;
		}
		else
		{
			Debug.Log("Aim Player 2");
			aimedPlayer = GameManager.gameManager.player2;
		}
	}

	//======================================================================================== CHECK PHASE TRANSITION

	/// <summary>
	/// check the health based on the actual phase to see if we need to change to the next one
	/// </summary>
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
				if(hp <= phase2Threshold * baseHP)
				{
					actualPhase++;
					Debug.Log("Passage phase 2");
					probabilityTable = phase2;
					nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
					//infinite mystic line same side / level shrink
				}
				break;
			case 2:
				if (hp <= phase3Threshold * baseHP)
				{
					actualPhase++;
					Debug.Log("Passage phase 3");
					probabilityTable = phase3;
					nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
					//infinite mystic line separation / etc
				}
				break;
			case 3:
				if (hp <= phase4Threshold * baseHP)
				{
					actualPhase++;
					Debug.Log("Passage phase 4");
					probabilityTable = phase4;
					nextAttack = Time.time + Random.Range(minWaitTime, maxWaitTime);
					//fall to ground / level shrink / rock fall activation
				}
				break;
			case 4:
				if (hp <= 0.0f)
				{
					Debug.Log("DED");
					//ded
				}
				break;
		}
	}

	//======================================================================================== RANDOM PATTERN

	/// <summary>
	/// pick a pattern at random based on the actual probability table
	/// </summary>
	/// <returns></returns>
	public BossPattern RandomPattern()
	{
		float randomPick = Random.Range(0.0f, 1.0f);
		float actualProb = 0.0f;

		foreach(PatternProbability patternProb in probabilityTable)
		{
			actualProb += patternProb.probability;
			if(randomPick <= actualProb)
			{
				return patternProb.pattern;
			}
		}

		return BossPattern.MysticLine;
	}

	//======================================================================================== LAUNCH PATTERN

	/// <summary>
	/// launch the selected pattern
	/// </summary>
	/// <param name="pattern"></param>
	public void LaunchPattern(BossPattern pattern)
	{
		switch(pattern)
		{
			case BossPattern.MysticLine:
				StartCoroutine(MysticLineCoroutine());
				break;
			case BossPattern.FireBall:
				StartCoroutine(FireBallCoroutine());
				break;
			case BossPattern.ElectricZone:
				StartCoroutine(ElectricZoneCoroutine());
				break;
			case BossPattern.ShrinkMysticLines:
				StartCoroutine(ShrinkMysticLinesCoroutine());
				break;
			case BossPattern.ElectricCone:
				StartCoroutine(ElectricConeCoroutine());
				break;
			case BossPattern.Charge:
				StartCoroutine(ChargeCoroutine());
				break;
			case BossPattern.ElectricAOE:
				StartCoroutine(ElectricAOECoroutine());
				break;
		}
	}

	#region pattern coroutines

	//======================================================================================== MYSTIC LINE

	public IEnumerator MysticLineCoroutine()
	{
		isAttacking = true;
		Debug.Log("Mystic Line");

		//canalisation + feedbacks
		yield return new WaitForSeconds(1.0f);
		//boom

		isAttacking = false;
	}

	//======================================================================================== FIREBALL

	public IEnumerator FireBallCoroutine()
	{
		isAttacking = true;

		Debug.Log("Fire Ball");

		//canalisation + feedbacks
		yield return new WaitForSeconds(1.0f);
		//boom

		isAttacking = false;
	}

	//======================================================================================== ELECTRIC ZONE

	public IEnumerator ElectricZoneCoroutine()
	{
		isAttacking = true;

		Debug.Log("Electric Zone");

		//canalisation + feedbacks
		yield return new WaitForSeconds(1.0f);
		//boom

		isAttacking = false;
	}

	//======================================================================================== SHRINK MYSTIC LINES

	public IEnumerator ShrinkMysticLinesCoroutine()
	{
		isAttacking = true;

		Debug.Log("Shrink MysticLines");

		//canalisation + feedbacks
		yield return new WaitForSeconds(1.0f);
		//boom

		isAttacking = false;
	}

	//======================================================================================== ELECTRIC CONE

	public IEnumerator ElectricConeCoroutine()
	{
		isAttacking = true;

		Debug.Log("Electric Cone");

		//canalisation + feedbacks
		yield return new WaitForSeconds(1.0f);
		//boom

		isAttacking = false;
	}

	//======================================================================================== CHARGE

	public IEnumerator ChargeCoroutine()
	{
		isAttacking = true;

		Debug.Log("Charge");

		//canalisation + feedbacks
		yield return new WaitForSeconds(1.0f);
		//boom

		isAttacking = false;
	}

	//======================================================================================== ELECTRIC AOE

	public IEnumerator ElectricAOECoroutine()
	{
		isAttacking = true;

		Debug.Log("Electric AOE");

		//canalisation + feedbacks
		yield return new WaitForSeconds(1.0f);
		//boom

		isAttacking = false;
	}

	#endregion
}
