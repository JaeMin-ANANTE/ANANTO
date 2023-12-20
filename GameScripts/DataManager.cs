using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

public class DataManager
{
    private static DataManager instance;
    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new DataManager();
            }
            return instance;
        }
    }

    private int gold;
    private bool language;
    private string dataFilePath;

    private DataManager()
    {
        dataFilePath = Path.Combine(Application.persistentDataPath, "shareData.json");
        UnityEngine.Debug.Log(dataFilePath);
        LoadData();
    }

    public void SaveData()
    {
        ShareData data = new ShareData();
        data.gold = gold;
        data.language = language;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(dataFilePath, json);
    }

    public void LoadData()
    {
        if (File.Exists(dataFilePath))
        {
            string json = File.ReadAllText(dataFilePath);
            ShareData data = JsonUtility.FromJson<ShareData>(json);
            gold = data.gold;
            language = data.language;
        }
        else
        {
            gold = 0;
            language = true;
            SaveData();
        }
    }

    public int GetGold()
    {
        return gold;
    }

    public bool GetLanguage()
    {
        return language;
    }

    public void AddGold(int amount)
    {
        gold += amount;
        SaveData();
    }

    public void DeductGold(int amount)
    {
        if (gold >= amount)
        {
            gold -= amount;
            SaveData();
        }
    }

    public void ChangeLanguage()
    {
        language = !language;
        SaveData();
    }
}
