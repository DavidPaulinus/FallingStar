using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    public static DataManager instance { get; private set; }

    [Header("DATA STORAGE CONFIG")]
    [SerializeField] private string fileName;

    private GameData gameData;
    private List<IDataPersistence> persistenceOBJ;
    private FileDataHandler fileDataHandler;

    private void Awake()
    {
        if (instance != null) Debug.LogError("Erro na instancia DataManager. Já existe uma instancia quando não devia");
        instance = this;
    }

    private void Start()
    {
        this.fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        this.persistenceOBJ = FindAllDataPersistenceObjects();
        LoadGame();
    }


    public void NewGame()
    {
        this.gameData = new GameData();
    }
    public void LoadGame()
    {
        this.gameData = fileDataHandler.Load();

        if (this.gameData == null)
        {
            Debug.LogError("Erro ao carregar SAVE. Não há save para ser carregado");
            NewGame();
        }

        foreach (IDataPersistence item in persistenceOBJ)
        {
            item.LoadData(gameData);
        }
    }
    public void SaveGame()
    {
        foreach (IDataPersistence item in persistenceOBJ)
        {
            item.SaveData(gameData);
        }

        fileDataHandler.Save(gameData);

    }
    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> _dataPersistenveObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(_dataPersistenveObjects);
    }
}
