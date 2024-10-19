using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRevolver : EnemyParent
{
    public override void OnTriggerStay(Collider other)
    {
        Debug.Log("“ригер активований з об'Їктом: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Player") && !isDetected)
        {
            Debug.Log("√равець в тригер≥");
            DetectionSlider.value += 1 * Time.deltaTime; // ѕлавне зб≥льшенн€
            if (DetectionSlider.value >= DetectionSlider.maxValue)
            {
                isDetected = true; // якщо дос€гли максимального значенн€, вважаЇмо, що гравець ви€влений
                Debug.Log("√равець ви€влений");
            }
        }
    }
}
