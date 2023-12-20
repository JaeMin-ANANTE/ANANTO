using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    private string dataFilePath;
    private AudioData audioData;

    private float bgmVolume;
    private float efVolume;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Initialize()
    {
        dataFilePath = Path.Combine(Application.persistentDataPath, "audioData.json");
        LoadData();
    }

    private void LoadData()
    {
        try
        {
            if (File.Exists(dataFilePath))
            {
                string json = File.ReadAllText(dataFilePath);
                audioData = JsonUtility.FromJson<AudioData>(json);
            }
            else
            {
                audioData = new AudioData()
                {
                    BackGroundVolume = 3,
                    EffectVolume = 3
                };
                SaveData();
            }
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("오디오데이터를 불러오는데 실패했습니다 : " + e.Message);
        }

        bgmVolume = audioData.BackGroundVolume;
        efVolume = audioData.EffectVolume;
    }

    private void SaveData()
    {
        try
        {
            string json = JsonUtility.ToJson(audioData, true);
            File.WriteAllText(dataFilePath, json);
        }
        catch (Exception e)
        {
            UnityEngine.Debug.LogError("오디오 데이터를 저장하는데 실패했습니다 : " + e.Message);
        }
    }

    public delegate void VolumeChangeHandler(float volume);
    public event VolumeChangeHandler OnBackGroundVolumeChanged;

    public float BackGroundVolume
    {
        get { return bgmVolume; }
        set
        {
            bgmVolume = value;
            audioData.BackGroundVolume = value;
            OnBackGroundVolumeChanged?.Invoke(bgmVolume);
            SaveData();
        }
    }

    public event VolumeChangeHandler OnEffectVolumeChanged;

    public float EffectVolume
    {
        get { return efVolume; }
        set
        {
            efVolume = value;
            audioData.EffectVolume = value;
            OnEffectVolumeChanged?.Invoke(efVolume);
            SaveData();
        }
    }
}
