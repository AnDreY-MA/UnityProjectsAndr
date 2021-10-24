using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SavePlayer(PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.dat";

        SavePlayerData dataPlayer = new SavePlayerData(player);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, dataPlayer);
        }
    }

    public static void SaveEnemy(Enemy enemy)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/enemy.dat";

        SaveEnemyData dataEnemy = new SaveEnemyData(enemy);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, dataEnemy);
        }
    }

    public static SavePlayerData LoadPlayer()
    {
        string path = Application.persistentDataPath + "/player.dat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SavePlayerData dataPlayer = formatter.Deserialize(stream) as SavePlayerData;
            stream.Close();

            return dataPlayer;
        }
        else
            return null;
    }
    public static SaveEnemyData LoadEnemy()
    {
        string path = Application.persistentDataPath + "/enemy.dat";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            SaveEnemyData dataEnemy = formatter.Deserialize(stream) as SaveEnemyData;
            stream.Close();

            return dataEnemy;
        }
        else
            return null;
    }
}
