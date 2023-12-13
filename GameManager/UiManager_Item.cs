using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.Versioning;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiManager_Item : MonoBehaviour
{
    private Dictionary<int, string> nameDict = new Dictionary<int, string>();
    private Dictionary<int, GameObject> itemDict = new Dictionary<int, GameObject>();

    public GameObject itemPrefab;
    public Sprite[] iconImageList;

    private ItemTextList itemTexts;
    // Start is called before the first frame update
    void Awake()
    {
        TextAsset jsonText = Resources.Load<TextAsset>("ItemTextData");
        itemTexts = JsonUtility.FromJson<ItemTextList>(jsonText.text);
        nameDict.Clear();
        DataManager.Instance.ChangeLanguage();
        foreach (var item in itemTexts.ItemTextData)
        {
            if (DataManager.Instance.GetLanguage())
                nameDict.Add(item.ID, item.Name_Korean);
            else
                nameDict.Add(item.ID, item.Name_English);
        }

        ViewItem();
    }

    private void ViewItem()
    {
        float yOffset = 0;
        foreach (int key in nameDict.Keys)
        {
            GameObject itemParents = Instantiate(itemPrefab);
            Transform itemTransform = itemParents.transform.Find("ItemListBoard");
            RectTransform itemRect = itemParents.GetComponent<RectTransform>();

            //Vector2 newRect = new Vector2(); //초기위치수정
            //itemRect.anchoredPosition = newRect;
            
            Vector3 currentPosition = itemTransform.position;
            Vector3 newPosition = new Vector3(currentPosition.x, currentPosition.y - yOffset, currentPosition.z);

            itemTransform.position = newPosition;

            yOffset += 245;

            GameObject itemBox = itemTransform.gameObject;
            itemDict[key] = itemBox;

            Image iconImage = itemBox.transform.Find("ItemIconImage").GetComponent<Image>();
            iconImage.sprite = iconImageList[key];

            Text psText = itemBox.transform.Find("ItemPsText").GetComponent<Text>();
            if (DataManager.Instance.GetLanguage())
                psText.text = itemTexts.ItemTextData[key].PS_Korean;
            else
                psText.text = itemTexts.ItemTextData[key].PS_English;
            ViewItemText(key);
        }
    }

    private void ViewItemText(int id)
    {

        GameObject itemBox = itemDict[id];
        //int currentLevel = itemDataManager.Instance.GetItemLevel(id); //아이템의 레벨을 불러옴.
        int currentLevel = 1;

        Text nameText = itemBox.transform.Find("ItemNameText").GetComponent<Text>();
        nameText.text = string.Format("{0} lv.{1}", nameDict[id], currentLevel);

        Text statusText = itemBox.transform.Find("ItemStatusText").GetComponent<Text>();
        statusText.text = "";

        //string typeString = "";
        string typeString = itemTexts.ItemTextData[id].Type;
        string[] typeList = typeString.Split(' ');

        foreach (string type in typeList)
        {
            UnityEngine.Debug.Log(type);
            int currentValue = ItemDataManager.Instance.GetCurrentItemType(id, type);
            int nextValue = ItemDataManager.Instance.GetSelectItemType(
               2 , id, type);
            if (DataManager.Instance.GetLanguage())
            {
                switch (type)
                {
                    case "duration":
                        typeString = "지속시간";
                        break;
                    case "count":
                        typeString = "생성개수";
                        break;
                    case "speed":
                        typeString = "이동속도";
                        break;
                    default: break;
                }
                statusText.text += string.Format("[현재]{0}:{1} -> [다음]{0}:{2}\n", typeString, currentValue
                    , nextValue);
            }
            else
            {
                typeString = type;
                statusText.text += string.Format("[Now]{0}:{1} -> [Next]{0}:{2}\n", typeString, currentValue
                    , nextValue);
            }
        }
    }

    public void MainScreenBtn()
    {
        SceneManager.LoadScene("MainScreen");
        UnityEngine.Debug.Log("메인스크린으로 이동합니다");
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
