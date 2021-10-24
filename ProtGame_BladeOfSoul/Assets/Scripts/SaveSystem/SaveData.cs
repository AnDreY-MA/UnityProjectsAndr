using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public float healthPlayer;
    public float[] positionPlayer;

    public float[] positionEnemy;

    public SaveData(PlayerController player)
    {
        healthPlayer = player.currentHealth;
        positionPlayer = new float[3];
        positionPlayer[0] = player.transform.position.x;
        positionPlayer[1] = player.transform.position.y;
        positionPlayer[3] = player.transform.position.z;

        
    }

    public SaveData(Enemy enemy)
    {
        positionEnemy = new float[3];
        positionEnemy[0] = enemy.transform.position.x;
        positionEnemy[1] = enemy.transform.position.y;
        positionEnemy[2] = enemy.transform.position.z;
    }
}
