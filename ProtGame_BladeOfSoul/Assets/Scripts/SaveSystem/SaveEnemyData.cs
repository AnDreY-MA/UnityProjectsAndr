using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveEnemyData 
{
    public float[] positionEnemy;

    public SaveEnemyData(Enemy enemy)
    {
        positionEnemy = new float[3];
        positionEnemy[0] = enemy.transform.position.x;
        positionEnemy[1] = enemy.transform.position.y;
        positionEnemy[3] = enemy.transform.position.z;
    }
}
