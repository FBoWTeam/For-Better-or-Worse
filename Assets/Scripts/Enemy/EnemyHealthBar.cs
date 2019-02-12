using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
	Image healthRemainingBar;
	Enemy enemy;

    // Start is called before the first frame update
    void Start()
    {
		healthRemainingBar = transform.GetChild(0).GetChild(0).GetComponent<Image>();
		enemy = GetComponentInParent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
		transform.LookAt(Camera.main.transform.position);
		healthRemainingBar.fillAmount = (float)enemy.hp / (float)enemy.baseHP;
    }
}
