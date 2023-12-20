using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public GameObject blockEffect;
    public static GameManager Instance { get; private set; }
    public ItemTableData itemTableData;
    public GameObject enemyPrefab;
    public GameObject itemPrefab;
  
    private static int itemCount;
    private int scoreCount;

    public Text scoreText;
    public GameObject gameoverBox;
    private float level;

    List<int> itemList = new List<int>
    {
        0, 1, 2, 7
    };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        level = 0.01f;
    }

    void Start()
    {
        ItemTable[] itemTable = itemTableData.itemTableArray;
        ItemFactoryManager.Initialize(itemTable);
        DataManager data = DataManager.Instance;

        itemCount = 0;

        // ���� �ð����� SpawnEnemy �ڷ�ƾ�� ȣ��
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpwanItemRoutine());

        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (true)
        {
            float spwanInterval = UnityEngine.Random.Range(0.0f, 3.0f - level);
            // ī�޶��� ����Ʈ�� �������� ������ ��ġ ����
            float randomX = UnityEngine.Random.Range(0.0f, 1.0f);
            Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(randomX, 1.0f, 10.0f));

            // �� ����
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

            // ���� �ð� ��� �� �ٽ� ����
            yield return new WaitForSeconds(spwanInterval);
            if (level < 2.8f) level += 0.05f;
        }
    }

    IEnumerator SpwanItemRoutine()
    {
        while (true)
        {
            float spawnInterval = UnityEngine.Random.Range(3.0f, 9.0f);

            if (itemCount < 3)
            {
                float randomX = UnityEngine.Random.Range(0.0f, 1.0f);
                Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(randomX, 0.99f, 10.0f));
                GameObject item = Instantiate(itemPrefab, spawnPosition, Quaternion.identity);
                ItemController itemComponent = item.GetComponent<ItemController>();

                int randomIndex = UnityEngine.Random.Range(0, itemList.Count);
                //int randomID = UnityEngine.Random.Range(0, 9);
                int randomID = itemList[randomIndex];
                if (itemComponent != null)
                {
                    itemComponent.id = randomID;
                }

                ItemTable itemData = itemTableData.GetItemData(randomID);
                if (itemTableData == null)
                {
                    UnityEngine.Debug.LogError("itemTableData is null");
                    yield break;
                }
                if (itemData == null)
                {
                    UnityEngine.Debug.LogError("itemData is null");
                    yield break;
                }
                if (itemData != null)
                {
                    SpriteRenderer spriteRenderer = item.GetComponent<SpriteRenderer>();
                    spriteRenderer.sprite = itemData.itemInActiveSprite;
                }
                itemCount++;
            }

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public static void UpdateItemCounter()
    {
        itemCount--;
    }

    public void UpdateScoreCounter()
    {
        scoreCount++;
        scoreText.text = scoreCount.ToString();
    }

    void Update()
    {
        // ���� ������Ʈ ���� �߰�
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        blockEffect.SetActive(true);
        bool language = DataManager.Instance.GetLanguage();
        gameoverBox.SetActive(true);
        Text current = gameoverBox.transform.Find("CurrentScoreText").GetComponent<Text>();
        Text next = gameoverBox.transform.Find("NextScoreText").GetComponent<Text>();
        Text gold = gameoverBox.transform.Find("GetGoldText").GetComponent<Text>();
        if (language) {
            current.text = string.Format("현재점수 : {0}점", scoreCount);
            //next.text = string.Format("최고점수 : {0}점", ScoreManager.Instance.GetGameScore());
            gold.text = string.Format("획득골드 : {0}골드", scoreCount);
        }
        else {
            current.text = string.Format("NowScore : {0}", scoreCount);
            //next.text = string.Format("HighScore : {0}", ScoreManager.Instance.GetGameScore());
            gold.text = string.Format("GetGold : {0}G", scoreCount);
        }
        if (scoreCount > ScoreManager.Instance.GetGameScore()) ScoreManager.Instance.AddGameScore(scoreCount);
        DataManager.Instance.AddGold(scoreCount);   
    }
}
