using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HeartView : MonoBehaviour, IHeartView
{
    public TextMeshProUGUI heartsText;
    public void UpdateHeartAmount(int amount)
    {

        if (heartsText != null)
        {
            heartsText.text = "Hearts: " + amount + " / 3";
    }
}
}
