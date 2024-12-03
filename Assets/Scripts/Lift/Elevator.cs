using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Transform bottomPosition;
    public Transform topPosition;
    public Transform player;
    public PlayerController PC;
    public float speed = 2f;

    public bool playerAtBottom = false;
    public bool playerAtTop = false;
    public GameObject CamWeapons;
    public Transform CamMain;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (playerAtBottom)
            {
                StartCoroutine(MovePlayer(topPosition.position));
            }
            else if (playerAtTop)
            {
                StartCoroutine(MovePlayer(bottomPosition.position));
            }
        }
    }
    IEnumerator MovePlayer(Vector3 targetPosition)
    {
        CamMain.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0f, 0, 0f), 20);
        player.rotation = Quaternion.Lerp(Quaternion.identity, Quaternion.Euler(0f,-90f,0f), 20);      
        if (CamWeapons != null)
        {
            CamWeapons.SetActive(false);
            PC.enabled = false;
        }
        while (Vector3.Distance(player.transform.position, targetPosition) > 0.1f)
        {
            player.transform.position = Vector3.MoveTowards(player.transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        if (CamWeapons != null)
        {
            CamWeapons.SetActive(true);
            PC.enabled = true;
        }
        player.transform.position = targetPosition;
    }
}

