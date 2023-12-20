using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public AudioSource bgmAudio;
    public AudioSource clickSound;
    // Start is called before the first frame update

    void Awake() {
        UpdateLanguage();
    }

    private void UpdateLanguage() {
        bool language = DataManager.Instance.GetLanguage();
        Text titleText = GameObject.Find("TitleText").GetComponent<Text>();
        Text gameText = GameObject.Find("GameStartText").GetComponent<Text>();
        Text itemText = GameObject.Find("ItemBtnText").GetComponent<Text>();
        Text optionText = GameObject.Find("OptionBtnText").GetComponent<Text>();

        if (language) {
            titleText.text = "레이디버그";
            gameText.text = "게임 시작";
            itemText.text = "아이템";
            optionText.text = "설정 하기";
        }
        else {
            titleText.text = "-LadyBug-";
            gameText.text = "Start";
            itemText.text = "Item";
            optionText.text = "Option";
        }
    }
    void Start()
    {
        AudioManager.Instance.OnBackGroundVolumeChanged += UpdateBackGroundVolume;
        UpdateBackGroundVolume(AudioManager.Instance.BackGroundVolume);

        AudioManager.Instance.OnEffectVolumeChanged += UpdateEffectVolume;
        UpdateEffectVolume(AudioManager.Instance.EffectVolume);
    }

    void UpdateBackGroundVolume(float volume)
    {
        if (bgmAudio != null)
        {
            bgmAudio.volume = volume;
        }
    }

    void UpdateEffectVolume(float volume)
    {
        if (clickSound != null)
        {
            clickSound.volume = volume;
        }
    }

    private void OnDestroy()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.OnBackGroundVolumeChanged -= UpdateBackGroundVolume;
            AudioManager.Instance.OnEffectVolumeChanged -= UpdateEffectVolume;
        }
    }
    public void GameScreenBtn()
    {
        if (clickSound != null)
        {
            clickSound.Play();
        }
        SceneManager.LoadScene("GameScreen");
    }

    public void ItemScreenBtn()
    {
        if (clickSound != null)
        {
            clickSound.Play();
        }
        SceneManager.LoadScene("NewItemScreen");
    }

    public void OptionScreenBtn()
    {
        if (clickSound != null)
        {
            clickSound.Play();
        }
        else UnityEngine.Debug.Log("ClickSound is null");
        SceneManager.LoadScene("NewOptionScreen");
    }

    // Update is called once per frame
    void Update()
    {
        if (bgmAudio.volume != AudioManager.Instance.BackGroundVolume)
            UpdateBackGroundVolume(AudioManager.Instance.BackGroundVolume);
        if (clickSound.volume != AudioManager.Instance.EffectVolume)
            UpdateEffectVolume(AudioManager.Instance.EffectVolume);
    }
}
