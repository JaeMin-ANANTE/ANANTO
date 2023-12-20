using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;

public class UiManager_Game : MonoBehaviour
{
    public Text scoreText;
    public Text pauseText;
    public Text yesText;
    public Text noText;

    public Text restartText;
    public Text exitText;

    public AudioSource bgmAudio;
    public AudioSource clickSound;
    public GameObject blockEffect;
    public GameObject pauseBox;
    public bool isPaused = false;

    // Start is called before the first frame update
    void Start()
    {
        blockEffect.SetActive(false);
        AudioManager.Instance.OnBackGroundVolumeChanged += UpdateBackGroundVolume;
        UpdateBackGroundVolume(AudioManager.Instance.BackGroundVolume);

        AudioManager.Instance.OnEffectVolumeChanged += UpdateEffectVolume;
        UpdateEffectVolume(AudioManager.Instance.EffectVolume);
        UpdateLanguageText();
    }

    private void UpdateLanguageText() {
        bool language = DataManager.Instance.GetLanguage();
        if (language) {
            scoreText.text = "현재점수";
            pauseText.text = "게임을 종료하고, 메인으로 돌아가나요?";
            yesText.text = "예";
            noText.text = "아니오";
            restartText.text = "재시작";
            exitText.text = "나가기";
        }
        else{
            scoreText.text = "NowScore";
            pauseText.text = "Do you want to quit the game and return to the main screen?";
            yesText.text = "Yes";
            noText.text = "No";
            restartText.text = "ReStart";
            exitText.text = "Exit";
        } 
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

    public void TogglePause()
    {
        if (isPaused)
        {
            GameBeginBtn();
        }
        else
        {
            GamePauseBtn();
        }
    }

    public void GamePauseBtn()
    {
        if (clickSound != null) clickSound.Play();
        blockEffect.SetActive(true);
        pauseBox.SetActive(true);

        Time.timeScale = 0; // 시간 흐름을 멈춥니다.
        isPaused = true;
        // 여기에 UI 업데이트나 기타 필요한 로직을 추가할 수 있습니다.
    }

    public void GameExitBtn()
    {
        if (clickSound != null) clickSound.Play();
        blockEffect.SetActive(false);
        Time.timeScale = 1;

        isPaused = false;
        SceneManager.LoadScene("MainScreen");
    }

    public void GameBeginBtn()
    {
        if (clickSound != null) clickSound.Play();
        blockEffect.SetActive(false);
        pauseBox.SetActive(false);

        Time.timeScale = 1; // 시간 흐름을 정상으로 복원합니다.
        isPaused = false;
    }

    public void ReStart() {
        if (clickSound != null)
        {
            clickSound.Play();
        }
        Time.timeScale = 1;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }
}
