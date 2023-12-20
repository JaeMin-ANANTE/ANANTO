using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UiManager_Option : MonoBehaviour
{
    public AudioSource bgmAudio;
    public AudioSource clickSound;
    // Start is called before the first frame update

    void Awake() {
        UpdateLanguage();
    }

    private void UpdateLanguage() {
        bool language = DataManager.Instance.GetLanguage();
        Text titleText = GameObject.Find("OptionTitleText").GetComponent<Text>();
        Text creditText = GameObject.Find("CreditText").GetComponent<Text>();
        Text languageText = GameObject.Find("LanguageText").GetComponent<Text>();
        Text soundText = GameObject.Find("SoundText").GetComponent<Text>();
        Text bgmText = GameObject.Find("SoundBackGroundSliderText").GetComponent<Text>();
        Text effectText = GameObject.Find("SoundEffectSliderText").GetComponent<Text>();
        
        Button koreanBtn = GameObject.Find("KoreanLanguageBtn").GetComponent<Button>();
        Button englishBtn = GameObject.Find("EnglishLanguageBtn").GetComponent<Button>();
        
        if (language) {
            titleText.text = "설정";
            creditText.text = "-제작자-";
            languageText.text = "-언어-";
            soundText.text = "-소리-";
            bgmText.text = "배경음";
            effectText.text = "효과음";

            koreanBtn.interactable = false;
            englishBtn.interactable = true;
        }
        else {
            titleText.text = "Option";
            creditText.text = "-CREDIT-";
            languageText.text = "-LANGUAGE-";
            soundText.text = "-SOUND-";
            bgmText.text = "BGM";
            effectText.text = "Effect";

            englishBtn.interactable = false;
            koreanBtn.interactable = true;;
        }
    }

    public void ChangeLanguageBtn() {
        if (clickSound != null) {
            clickSound.Play();
        }
        DataManager.Instance.ChangeLanguage();
        UpdateLanguage();
    }

    public void BackBtn() {
        if (clickSound != null) {
            clickSound.Play();
        }
        SceneManager.LoadScene("MainScreen");
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

    // Update is called once per frame
    void Update()
    {
        if (bgmAudio.volume != AudioManager.Instance.BackGroundVolume)
            UpdateBackGroundVolume(AudioManager.Instance.BackGroundVolume);
        if (clickSound.volume != AudioManager.Instance.EffectVolume)
            UpdateEffectVolume(AudioManager.Instance.EffectVolume);
    }
}
