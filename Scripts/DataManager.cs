using UnityEngine;
using System.IO;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;

public class DataManager : MonoBehaviour
{
    public Item[] items;
    private static DataManager instance;

    public static DataManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject gameObject = new GameObject("DataManager");
                instance = gameObject.AddComponent<DataManager>();
                //DontDestroyOnLoad(gameObject);
            }
            return instance;
        }
    }

    private int coin;
    private string dataFilePath;

    private void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        // JSON ���� ��� ���� (��: Application.persistentDataPath�� ����)
        dataFilePath = Path.Combine(UnityEngine.Application.persistentDataPath, "currencyData.json");
        // ������ �ҷ�����
        LoadCurrencyData();
    }

    // ��ȭ ������ ����
    public void SaveCurrencyData()
    {
        CurrencyData data = new CurrencyData();
        data.coins = coin;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(dataFilePath, json);
    }

    // ��ȭ ������ �ҷ�����
    public void LoadCurrencyData()
    {
        if (File.Exists(dataFilePath))
        {
            string json = File.ReadAllText(dataFilePath);
            CurrencyData data = JsonUtility.FromJson<CurrencyData>(json);
            coin = data.coins;
        }
        else
        {
            // ����� �����Ͱ� ���� ��� �ʱ�ȭ
            coin = 0;
        }
    }

    // ��ȭ �� ��ȯ
    public int GetCoins()
    {
        return coin;
    }

    // ��ȭ �� ����
    public void AddCoins(int amount)
    {
        coin += amount;
        SaveCurrencyData(); // ����� ������ ����
    }

    // ��ȭ �� ����
    public void DeductCoins(int amount)
    {
        if (coin >= amount)
        {
            coin -= amount;
            SaveCurrencyData(); // ����� ������ ����
        }
        else
        {
            //Debug.LogWarning("Not enough coins to deduct.");
        }
    }
}

[System.Serializable]
public class CurrencyData
{
    public int coins;
    public List<ItemData> itemDatas;
}

[System.Serializable]
public struct ItemData
{
    public string name;
    public int level;
}

[CreateAssetMenu(menuName = "Item")]
public class Item : ScriptableObject
{
    public string name;
    public Sprite image;
    [SerializeField]
    private int[] duration;
    public int[] speed;
    public int[] count;
}


