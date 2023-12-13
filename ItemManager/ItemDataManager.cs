using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;
using System;

public class ItemDataManager
{
    private static ItemDataManager instance;
    public static ItemDataManager Instance
    {
        get
        {
            if (instance == null)
                instance = new ItemDataManager();
            return instance;
        }
    }

    private string dataFilePath;
    private Dictionary<int, ItemUserEntry> itemDict = new Dictionary<int, ItemUserEntry>();
    private ItemLevelList levelList;

    public ItemDataManager()
    {
        TextAsset jsonData = Resources.Load<TextAsset>("ItemLevelData");
        if (jsonData != null)
        {
            levelList = JsonUtility.FromJson<ItemLevelList>(jsonData.text);
            if (levelList != null) {
                UnityEngine.Debug.Log("levelList is not null.");
                if (levelList.ItemLevelData != null) UnityEngine.Debug.Log("levelList.itemLevelData is not null.");
                else UnityEngine.Debug.Log("levelList.itemLevelData is null.");
            }
            else UnityEngine.Debug.Log("levelList는 null입니다");
            foreach (var levelData in levelList.ItemLevelData[0].Attributes.DurationByLevel) {
                UnityEngine.Debug.Log("Level: " + levelData.Level + " Value: " + levelData.Value);
            }
        }
        else UnityEngine.Debug.Log("jsonData is null");

        dataFilePath = Path.Combine(Application.persistentDataPath, "itemData.json");
        UnityEngine.Debug.Log(dataFilePath);
        LoadData();
    }

    private void LoadData()
    {
        if (File.Exists(dataFilePath))
        {
            string json = File.ReadAllText(dataFilePath);
            ItemUserList list = JsonUtility.FromJson<ItemUserList>(json);
            foreach (var item in list.itemUserData)
            {
                itemDict[item.ID] = item;
            }
        }
        else
        {
            for (int i = 0; i < 9; i++)
            {
                itemDict.Add(i, new ItemUserEntry
                {
                    ID = i,
                    Level = 1,
                    duration = GetCurrentItemType(i, "duration"),
                    speed = GetCurrentItemType(i, "speed"),
                    count = GetCurrentItemType(i, "count")
                });
            }
            SaveData();
        }
    }

    private void SaveData()
    {
        try
        {
            ItemUserList list = new ItemUserList();
            foreach (var entry in itemDict.Values)
            {
                list.itemUserData.Add(entry);
            }
            string json = JsonUtility.ToJson(list);
            File.WriteAllText(dataFilePath, json);
            UnityEngine.Debug.Log("Your data has been saved.");
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("Your data has not been saved: " + e.Message);
        }
    }

    public int GetItemLevel(int id)
    {
        if (itemDict.ContainsKey(id)) {
            return itemDict[id].Level;
        }
        return 1;
    }

    public int GetCurrentItemType(int id, string type) {
        int lv = GetItemLevel(id);
        int value = GetSelectItemType(lv, id, type);
        return value;
    }

    public int GetSelectItemType(int lv, int id, string type) {
        foreach (var item in levelList.ItemLevelData) {
            if (item.ID == id) {
                int level = lv;
                switch (type) 
                {
                    case "duration":
                        if (level >= 0 && level < item.Attributes.DurationByLevel.Length)
                            return item.Attributes.DurationByLevel[level].Value;
                        else break;
                    case "speed":
                        if (level >= 0 && level < item.Attributes.SpeedByLevel.Length)
                            return item.Attributes.SpeedByLevel[level].Value;
                        break;
                    case "count":
                        if (level >= 0 && level < item.Attributes.CountByLevel.Length)
                            return item.Attributes.CountByLevel[level].Value;
                        break;
                    default: return -1;
                }
            }
        }
        return -1;
    }
}
