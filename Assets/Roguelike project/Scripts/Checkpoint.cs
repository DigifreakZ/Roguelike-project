using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public SaveManager saveManager;
    public int checkpointID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && collision is CircleCollider2D)
        {
            collision.GetComponent<PlayerStats>().lastCheckpoint = checkpointID;
            saveManager.SavePlayerData();
        }
    }
}
