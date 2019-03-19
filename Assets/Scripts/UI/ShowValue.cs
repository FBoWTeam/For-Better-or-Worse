using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowValue : MonoBehaviour
{
    Text percentageText;

    private void Start()
    {
        percentageText = GetComponent<Text>();
    }

    public void UpdateText(float value)
    {
        percentageText.text = Mathf.RoundToInt(100 * value) + "%";
    }
}
