using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager_Game : MonoBehaviour
{
    public GameObject blockEffect;
    private bool isPaused = false;

    public Sprite[] startNumber;
    public Image countNumber;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartCountdown());
    }

    // Update is called once per frame
    void Update()
    {
        
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
        Time.timeScale = 0; // 시간 흐름을 멈춥니다.
        isPaused = true;
        blockEffect.SetActive(true);
        // 여기에 UI 업데이트나 기타 필요한 로직을 추가할 수 있습니다.
    }

    public void GameBeginBtn()
    {
        blockEffect.SetActive(false);

        Time.timeScale = 1; // 시간 흐름을 정상으로 복원합니다.
        isPaused = false;
    }

    IEnumerator StartCountdown()
    {
        RectTransform countRect = countNumber.GetComponent<RectTransform>();
        int i = 0;
        countRect.sizeDelta = new Vector2(300, 450);
        foreach (var sprite in startNumber)
        {
            countNumber.sprite = sprite; // 현재 스프라이트를 설정
            if (i == 2) countRect.sizeDelta = new Vector2(1200, 300);
            countNumber.enabled = true; // SpriteRenderer 활성화
            yield return new WaitForSeconds(1);
            countNumber.enabled = false; // SpriteRenderer 비활성화
            i++;
        }

        // 여기에 게임 시작을 위한 추가 로직을 넣을 수 있습니다.
    }

}
