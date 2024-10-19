using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyRevolver : EnemyParent
{
    public override void OnTriggerStay(Collider other)
    {
        Debug.Log("������ ����������� � ��'�����: " + other.gameObject.name);

        if (other.gameObject.CompareTag("Player") && !isDetected)
        {
            Debug.Log("������� � ������");
            DetectionSlider.value += 1 * Time.deltaTime; // ������ ���������
            if (DetectionSlider.value >= DetectionSlider.maxValue)
            {
                isDetected = true; // ���� ������� ������������� ��������, �������, �� ������� ���������
                Debug.Log("������� ���������");
            }
        }
    }
}
