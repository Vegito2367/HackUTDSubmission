using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using System.IO;
public enum ItemType { Jetpack,Grapple}
public enum ConversationType { Dialogue,Monologue}
public static class SaveToBinary 
{
    public static void SavePlayerData()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        string path = Path.Combine(Application.persistentDataPath, "SaveFile.Zer0");
        FileStream fileStream = new FileStream(path, FileMode.Create);
        PlayerData playerData = new PlayerData();
        binaryFormatter.Serialize(fileStream, playerData);
        fileStream.Close();
    }
    public static PlayerData LoadPlayerData()
    {
        string path = Path.Combine(Application.persistentDataPath, "SaveFile.Zer0");
        if(File.Exists(path))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(path,FileMode.Open);
            PlayerData playerdata = bf.Deserialize(stream) as PlayerData;
            stream.Close();
            return playerdata;
        }
        else
        {
            Debug.Log($"Not found save file in : {path}");
            return null;
        }
    }
}
