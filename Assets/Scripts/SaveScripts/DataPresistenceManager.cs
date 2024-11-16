using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

public class DataPresistenceManager : MonoBehaviour
{
    [Header("File Storage Config")]
    [SerializeField] private string fileName;
    private PlayerData playerData;
    public static DataPresistenceManager Instance { get; private set; }
    private List<IDataPersistence> dataPresistanceObjects;
    private FileDataHandler fileDataHandler;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            Debug.LogWarning("Multiple DataPresistenceManager instances detected. Destroying the new one.");
        }
    }
    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPresistanceObjects = GetDataPresistanceObjects();
        LoadPlayerData();
    }
    public void SavePlayerData()
    {
        foreach (IDataPersistence dataPresistanceObject in dataPresistanceObjects)
        {
            dataPresistanceObject.SavePlayerData(playerData);
        }

        fileDataHandler.SaveData(playerData);
    }
    public void LoadPlayerData()
    {
        playerData = fileDataHandler.LoadData(); // Assign loaded data to playerData
        if (playerData == null)
        {
            NewGame();
            Debug.Log("No save data found. Creating new game.");
        }
        else
        {
            foreach (IDataPersistence dataPersistenceObject in dataPresistanceObjects)
            {
                dataPersistenceObject.LoadPlayerData(playerData);
            }
        }

    }
    public void NewGame()
    {
        playerData = new PlayerData();
    }
    private List<IDataPersistence> GetDataPresistanceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
           .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }
    void OnApplicationQuit()
    {
        SavePlayerData();
    }
}
