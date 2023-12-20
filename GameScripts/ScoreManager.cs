using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    private static ScoreManager instance;
    public static ScoreManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ScoreManager();
            }
            return instance;
        }
    }

    private int score;
    string dataFilePath;

    public ScoreManager()
    {
        dataFilePath = Path.Combine(Application.persistentDataPath, "scoreData.json");
        LoadData();
    }

    private void LoadData()
    {
       if (File.Exists(dataFilePath))
        {
            string json = File.ReadAllText(dataFilePath);
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);
            score = data.score;
        }
       else
        {
            score = 0;
            SaveData();
        }
    }

    private void SaveData()
    {
        ScoreData data = new ScoreData();
        string json = JsonUtility.ToJson(data);
        data.score = score;
        File.WriteAllText(dataFilePath, json);
    }

    public int GetGameScore()
    {
        return score;
    }

    public void AddGameScore(int num)
    {
        if (num > score)
        {
            score = num;
            SaveData();
        }
    }
}
