using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

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
        if (Instance != null)
        {
            Debug.Log("Found more than one Data Persistence Manager in the scene. Destroying the newest one.");
            Destroy(this.gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
    }
    public void OnEnable()
    {
        SceneManager.sceneLoaded += OnScenceLoaded;

    }
    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnScenceLoaded;
    }
    public void OnScenceLoaded(Scene scene, LoadSceneMode mode)
    {
        dataPresistanceObjects = GetDataPresistanceObjects();
        LoadPlayerData();
        Debug.Log("Scene Loaded: " + scene.name);
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
        SavePlayerData();
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
