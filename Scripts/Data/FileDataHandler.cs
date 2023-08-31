using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private String path = "";
    private String FileName = "";

    public FileDataHandler(String path, String FileName)
    {
        this.path = path;
        this.FileName = FileName;
    }

    public GameData Load()
    {
        //usa o Path por conta de diferentes sistemas operacionais
        var _fullPath = Path.Combine(path, FileName);

        GameData _loadedData = null;
        if (File.Exists(_fullPath))
        {
            var _dataToLoad = "";
            try
            {
                using (StreamReader reader = new StreamReader(new FileStream(_fullPath, FileMode.Open)))
                {
                    _dataToLoad = reader.ReadToEnd();
                }

            }
            catch (Exception e)
            {
                Debug.LogError(e + " FileDataHandler Load Exception at " + path);
            }

            _loadedData = JsonUtility.FromJson<GameData>(_dataToLoad);
        }
        return _loadedData;
    }

    public void Save(GameData data)
    {
        //usa o Path por conta de diferentes sistemas operacionais
        var _fullPath = Path.Combine(path, FileName);

        try
        {
            //cria o diretório no path, se nãoexistir
            Directory.CreateDirectory(Path.GetDirectoryName(_fullPath));

            //passa o codigo pra JSON
            var _dataToStore = JsonUtility.ToJson(data, true);

            //cria e/ou escreve o arquivo
            using (StreamWriter writter = new StreamWriter(new FileStream(_fullPath, FileMode.Create)))
            {
                writter.Write(_dataToStore);
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e + "FileDataHandler Save Exception at " + path);
        }
    }
}
