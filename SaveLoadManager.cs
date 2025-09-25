using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SaveLoadManager : MonoBehaviour
{
    public static SaveLoadManager Instance;

    private string saveFilePath;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            saveFilePath = Application.persistentDataPath + "/playerData.json";
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Funzione per salvare i dati
    public void SavePlayerData(PlayerData data)
    {
        string jsonData = JsonUtility.ToJson(data, true);
        File.WriteAllText(saveFilePath, jsonData);
        Debug.Log("Dati salvati!");
    }

    // Funzione per caricare i dati
    public PlayerData LoadPlayerData()
    {
        if (File.Exists(saveFilePath))
        {
            string jsonData = File.ReadAllText(saveFilePath);
            PlayerData data = JsonUtility.FromJson<PlayerData>(jsonData);
            Debug.Log("Dati caricati!");
            return data;
        }
        else
        {
            Debug.Log("Nessun file di salvataggio trovato.");
            return null;
        }
    }
}
