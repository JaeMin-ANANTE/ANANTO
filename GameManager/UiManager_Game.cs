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
        Time.timeScale = 0; // �ð� �帧�� ����ϴ�.
        isPaused = true;
        blockEffect.SetActive(true);
        // ���⿡ UI ������Ʈ�� ��Ÿ �ʿ��� ������ �߰��� �� �ֽ��ϴ�.
    }

    public void GameBeginBtn()
    {
        blockEffect.SetActive(false);

        Time.timeScale = 1; // �ð� �帧�� �������� �����մϴ�.
        isPaused = false;
    }

    IEnumerator StartCountdown()
    {
        RectTransform countRect = countNumber.GetComponent<RectTransform>();
        int i = 0;
        countRect.sizeDelta = new Vector2(300, 450);
        foreach (var sprite in startNumber)
        {
            countNumber.sprite = sprite; // ���� ��������Ʈ�� ����
            if (i == 2) countRect.sizeDelta = new Vector2(1200, 300);
            countNumber.enabled = true; // SpriteRenderer Ȱ��ȭ
            yield return new WaitForSeconds(1);
            countNumber.enabled = false; // SpriteRenderer ��Ȱ��ȭ
            i++;
        }

        // ���⿡ ���� ������ ���� �߰� ������ ���� �� �ֽ��ϴ�.
    }

}
