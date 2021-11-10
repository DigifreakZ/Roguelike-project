using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;

public class SaveManager : MonoBehaviour
{

    public PlayerStats playerStats;
    public Inventory inventory;
    public InventoryUI inventoryUI;


    [TextArea(4,20)] public string JsonData;
    private string filename = "Savedata.game";
    private string path;

    public GameObject defaultSpawn;
    private bool firstLoad = true;

    private void OnEnable()
    {
        path = Application.persistentDataPath + $"\\{filename}";
        if (File.Exists(path))
        {
            LoadPlayerData();
        }
        if (firstLoad)
        {
            playerStats.gameObject.transform.position = defaultSpawn.transform.position;
            firstLoad = false;
        }
    }

    public void LoadPlayerData()
    {
        using (FileStream stream = File.OpenRead(path))
        {
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            char[] data = new char[buffer.Length];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (char)buffer[i];
            }
            JsonData = new string(data);
            PlayerData loadData = JsonUtility.FromJson<PlayerData>(JsonData);
            playerStats.defaultPlayerCharacterStats = loadData.playerCharacter;
            playerStats.maxHealth = loadData.maxHealth;
            playerStats.currentHealth = loadData.currentHealth;
            playerStats.speed = loadData.speed;
            playerStats.currentScene = loadData.currentScene;
            inventory.currentActivePower = loadData.playerCurrentActivePower;
            inventory.passivePowersInventory = loadData.playerPassivePowersInventory;
            try
            {
                playerStats.lastCheckpoint = loadData.lastCheckpoint;
                playerStats.transform.position = loadData.lastCheckpoint;
            }
            catch (MissingReferenceException)
            {
                playerStats.lastCheckpoint = GameObject.Find("DefaultSpawn").transform.position;
                playerStats.transform.position = GameObject.Find("DefaultSpawn").transform.position;
                //playerStats.transform.position = loadData.Position;

            }
            catch (NullReferenceException)
            {
                playerStats.lastCheckpoint = GameObject.Find("DefaultSpawn").transform.position;
                playerStats.transform.position = GameObject.Find("DefaultSpawn").transform.position;
                //playerStats.transform.position = loadData.Position;

            }
        }
    }
    public void SavePlayerData()
    {
        print(path);
        PlayerData playerData = new PlayerData
        {
            playerCharacter = playerStats.defaultPlayerCharacterStats,
            maxHealth = playerStats.maxHealth,
            currentHealth = playerStats.currentHealth,
            speed = playerStats.speed,
            playerCurrentActivePower = inventory.currentActivePower,
            playerPassivePowersInventory = inventory.passivePowersInventory,
            Position = playerStats.transform.position,
            currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
            lastCheckpoint = playerStats.lastCheckpoint
        };
        JsonData = JsonUtility.ToJson(playerData, true);
        File.WriteAllText(path, "");
        using (FileStream stream = File.OpenWrite(path))
        {
            byte[] buffer = JsonData.Select(@char => (byte)@char).ToArray();
            stream.Write(buffer, 0, JsonData.Length);
        }
    }
}

[Serializable]
public class PlayerData 
{
    public PlayerBase playerCharacter;
    public float maxHealth;
    public float currentHealth;
    public float speed;
    public ActivePower playerCurrentActivePower;
    public List<PassivePower> playerPassivePowersInventory;
    public Vector3 Position;
    public string currentScene;
    public Vector3 lastCheckpoint;
}
