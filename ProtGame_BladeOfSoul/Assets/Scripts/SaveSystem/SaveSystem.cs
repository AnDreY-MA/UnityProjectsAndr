using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void Save(PlayerController player)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/player.dat";

        SavePlayerData dataPlayer = new SavePlayerData(player);

        using (FileStream stream = new FileStream(path, FileMode.Create))
        {
            formatter.Serialize(stream, dataPlayer);
        }
    }

    public static SavePlayerData Load()
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
}
