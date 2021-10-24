using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SavePlayerData 
{
    [Header("Saving")]
    public int level;
    public float healthPlayer;
    public float[] positionPlayer;

    public SavePlayerData(PlayerController player)
    {
        healthPlayer = player.currentHealth;

        positionPlayer = new float[3];
        positionPlayer[0] = player.transform.position.x;
        positionPlayer[1] = player.transform.position.y;
        positionPlayer[2] = player.transform.position.z;

    }
}
