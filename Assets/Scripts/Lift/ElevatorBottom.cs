using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorBottom : MonoBehaviour
{
    public Elevator Script;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Script.player = other.transform;
            Script.CamWeapons = other.gameObject.transform.Find("CameraMain/CameraHands").gameObject;
            Script.CamMain = other.gameObject.transform.Find("CameraMain");
            Script.PC = other.GetComponent<PlayerController>();
            Script.playerAtBottom = true;
            Script.playerAtTop = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Script.playerAtBottom = false;
            Script.playerAtTop = false;
        }
    }
}
