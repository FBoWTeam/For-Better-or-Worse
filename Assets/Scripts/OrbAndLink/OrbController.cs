﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbController : MonoBehaviour
{
    [Header("[Orb Statistics]")]
    public int damage;
    public float speed;
    public float minSpeed;
    public float maxSpeed;

    [Header("[Valid Targets]")]
    public bool canHitEnemy;
    public bool canHitPlayer;

    [Header("[Fix Coefficient]")]
    public float veryLowFixedCoefficient;
    public float lowFixedCoefficient;
    public float highFixedCoefficient;
    public float veryHighFixedCoefficient;

    float fixedSpeedCoefficient;

    [Header("[Direction]")]
    public bool toPlayer2;

    float progression;
    float step;

    void Start()
    {
        toPlayer2 = true;
        progression = 0.5f;
        transform.position = BezierCurve.CalculateCubicBezierPoint(progression);
    }

    void FixedUpdate()
    {
        setFixedSpeedCoefficient();
        checkSpeed();
        float fixedSpeed = speed * fixedSpeedCoefficient; ;

        step = (fixedSpeed / BezierCurve.GetPlayersDistance()) * Time.fixedDeltaTime;
        progression = toPlayer2 ? progression + step : progression - step;
        progression = Mathf.Clamp01(progression);
        transform.position = BezierCurve.CalculateCubicBezierPoint(progression);

        if (progression == 1.0f || progression == 0.0f)
            toPlayer2 = !toPlayer2;
    }

    /// <summary>
    /// for each player, return 1 if the player moves away from the other player, -1 if he gets closer, 0 otherwise
    /// </summary>
    /// <returns></returns>
    (int, int) getMovementsInfo()
    {
        int player1Movement = 0;
        int player2Movement = 0;

        if (GameManager.gameManager.player1.GetComponent<PlayerController>().direction.magnitude >= 0.25f)
        {
            Vector3 player1Reference = (GameManager.gameManager.player2.transform.position - GameManager.gameManager.player1.transform.position).normalized;
            Vector3 player1Direction = GameManager.gameManager.player1.GetComponent<PlayerController>().direction;
            float player1Angle = Vector3.Angle(player1Direction, player1Reference);

            if (player1Angle >= -45.0f && player1Angle <= 45.0f)
                player1Movement = -1;
            else if ((player1Angle >= -180.0f && player1Angle <= -135.0f) || (player1Angle >= 135.0f && player1Angle <= 180.0f))
                player1Movement = 1;
        }

        if (GameManager.gameManager.player2.GetComponent<PlayerController>().direction.magnitude >= 0.25f)
        {
            Vector3 player2Reference = (GameManager.gameManager.player1.transform.position - GameManager.gameManager.player2.transform.position).normalized;
            Vector3 player2Direction = GameManager.gameManager.player2.GetComponent<PlayerController>().direction;
            float player2Angle = Vector3.Angle(player2Direction, player2Reference);

            if (player2Angle >= -45.0f && player2Angle <= 45.0f)
                player2Movement = -1;
            else if ((player2Angle >= -180.0f && player2Angle <= -135.0f) || (player2Angle >= 135.0f && player2Angle <= 180.0f))
                player2Movement = 1;
        }

        return (player1Movement, player2Movement);
    }

    /// <summary>
    /// set the speed coefficient to fix the speed when the link is shrinked/expanded
    /// </summary>
    void setFixedSpeedCoefficient()
    {
        (int, int) playersMovements = getMovementsInfo();
        (int, bool, int) movementsInfo = (playersMovements.Item1, toPlayer2, playersMovements.Item2);

        switch (movementsInfo)
        {

            case var c1 when c1.Item1 == 1 && c1.Item2 == false && c1.Item3 == -1:
            case var c2 when c2.Item1 == -1 && c2.Item2 == true && c2.Item3 == 1:
                fixedSpeedCoefficient = veryLowFixedCoefficient;
                break;
            case var c1 when c1.Item1 == 1 && c1.Item2 == false && c1.Item3 == 0:
            case var c2 when c2.Item1 == -1 && c2.Item2 == true && c2.Item3 == 0:
            case var c3 when c3.Item1 == 0 && c3.Item2 == true && c3.Item3 == 1:
            case var c4 when c4.Item1 == 0 && c4.Item2 == false && c4.Item3 == -1:
            case var c5 when c5.Item1 == -1 && c5.Item3 == -1:
            case var c6 when c6.Item1 == 1 && c6.Item3 == 1:
                fixedSpeedCoefficient = lowFixedCoefficient;
                break;
            case var c when c.Item1 == 0 && c.Item3 == 0:
                fixedSpeedCoefficient = 1.0f;
                break;
            case var c1 when c1.Item1 == 1 && c1.Item2 == true && c1.Item3 == 0:
            case var c2 when c2.Item1 == -1 && c2.Item2 == false && c2.Item3 == 0:
            case var c3 when c3.Item1 == 0 && c3.Item2 == false && c3.Item3 == 1:
            case var c4 when c4.Item1 == 0 && c4.Item2 == true && c4.Item3 == -1:
                fixedSpeedCoefficient = highFixedCoefficient;
                break;
            case var c1 when c1.Item1 == 1 && c1.Item2 == true && c1.Item3 == -1:
            case var c2 when c2.Item1 == -1 && c2.Item2 == false && c2.Item3 == 1:
                fixedSpeedCoefficient = veryHighFixedCoefficient;
                break;
        }
    }

    void checkSpeed()
    {
        if (speed < minSpeed)
        {
            speed = minSpeed;
        }
        if (speed > maxSpeed)
        {
            speed = maxSpeed;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && canHitPlayer == true)
        {
            GameManager.gameManager.takeDamage(damage);
            speed = minSpeed;
        }
        else if (other.CompareTag("Enemy") && canHitEnemy == true)
        {
            other.GetComponent<Enemy>().hp -= damage;
        }
    }
}
