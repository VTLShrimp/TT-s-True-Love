using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;
public class DataPersistenceManager : MonoBehaviour
{
    public static DataPersistenceManager instance { get; private set; }

    [SerializeField] private string fileName;
    private GameData gameData;
    private List<IDataPersistence> dataPersistenceObjects;
    private FileDataHandler dataHandler;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("There is more than one instance of DataPersistenceManager");
            Destroy(gameObject);
        }
        instance = this;
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    public void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("Scene Loaded: " + scene.name);
        dataPersistenceObjects = FindAllDataPersistenceObjects();
        LoadGame();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
        SaveGame();
    }
    public void SaveGame()
    {
        if (this.gameData == null)
        {
            return;
        }
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.SaveData(ref gameData);
        }
        dataHandler.Save(gameData);
    }
    public void LoadGame()
    {
        this.gameData = dataHandler.Load();
        if (this.gameData == null)
        {
            Debug.LogError("No saved game data found.");
            return;
        }
        Debug.Log("Game data loaded successfully.");
        foreach (IDataPersistence dataPersistenceObject in dataPersistenceObjects)
        {
            dataPersistenceObject.LoadData(gameData);
        }
    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistenceObjects = FindObjectsOfType<MonoBehaviour>(true)
            .OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistenceObjects);
    }

    public GameData GetCurrentGameData()
    {
        return this.gameData;
    }
}
