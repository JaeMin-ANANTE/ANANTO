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
        // JSON 파일 경로 설정 (예: Application.persistentDataPath에 저장)
        dataFilePath = Path.Combine(UnityEngine.Application.persistentDataPath, "currencyData.json");
        // 데이터 불러오기
        LoadCurrencyData();
    }

    // 재화 데이터 저장
    public void SaveCurrencyData()
    {
        CurrencyData data = new CurrencyData();
        data.coins = coin;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(dataFilePath, json);
    }

    // 재화 데이터 불러오기
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
            // 저장된 데이터가 없을 경우 초기화
            coin = 0;
        }
    }

    // 재화 양 반환
    public int GetCoins()
    {
        return coin;
    }

    // 재화 양 증가
    public void AddCoins(int amount)
    {
        coin += amount;
        SaveCurrencyData(); // 변경된 데이터 저장
    }

    // 재화 양 감소
    public void DeductCoins(int amount)
    {
        if (coin >= amount)
        {
            coin -= amount;
            SaveCurrencyData(); // 변경된 데이터 저장
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


