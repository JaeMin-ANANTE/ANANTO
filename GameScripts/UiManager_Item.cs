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
using UnityEngine.Video;
using Unity.Collections;

public class UiManager_Item : MonoBehaviour
{
    public Text goldText;
    public AudioSource bgmAudio;
    public AudioSource clickSound;
    public AudioSource sucessSound;
    public AudioSource failedSound;
    private Dictionary<int, string> nameDict = new Dictionary<int, string>();
    private Dictionary<int, GameObject> itemDict = new Dictionary<int, GameObject>();

    private List<int> useList = new List<int>{
        0, 1, 2, 7
    };

    public GameObject itemPrefab;
    public Sprite[] iconImageList;
    public GameObject blockEffect;
    public GameObject rainforcePanel;

    private ItemTextList itemTexts;
    // Start is called before the first frame update
    void Awake()
    {
        blockEffect.SetActive(false);
        rainforcePanel.SetActive(false);
        TextAsset jsonText = Resources.Load<TextAsset>("ItemTextData");
        itemTexts = JsonUtility.FromJson<ItemTextList>(jsonText.text);
        nameDict.Clear();
        //DataManager.Instance.ChangeLanguage();
        foreach (var item in itemTexts.ItemTextData)
        {
            if (DataManager.Instance.GetLanguage())
                nameDict.Add(item.ID, item.Name_Korean);
            else
                nameDict.Add(item.ID, item.Name_English);
        }

        ViewItem();

        Text titleText = GameObject.Find("ItemTitleText").GetComponent<Text>();
        bool language = DataManager.Instance.GetLanguage();
        if (language) {
            titleText.text = "아이템";
        }
        else titleText.text = "Item";
    }

    private bool IsValueInList<T>(List<T> list, T value)
    {
        return list.Contains(value);
    }

    private void ViewItem()
    {
        int gold = DataManager.Instance.GetGold();
        goldText.text = gold.ToString();
        float yOffset = 0;
        foreach (int key in nameDict.Keys)
        {
            bool existence = IsValueInList(useList, key);
            if (!existence) continue;
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
        int currentLevel = ItemDataManager.Instance.GetItemLevel(id);

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
               ItemDataManager.Instance.GetItemLevel(id) + 1 , id, type);
            
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
                if (nextValue == -1) {
                    statusText.text = string.Format("{0}:{1}", typeString, currentValue);
                }
            }
            else
            {
                typeString = type;
                statusText.text += string.Format("[Now]{0}:{1} -> [Next]{0}:{2}\n", typeString, currentValue
                    , nextValue);
                if (nextValue == -1) {
                     statusText.text = string.Format("{0}:{1}", typeString, currentValue);
                }
            }
            if (nextValue == -1) {
                nameText.text = string.Format("{0} lv.MAX", nameDict[id]);
            }
        }

        Button rainforceBtn = itemBox.GetComponent<Button>();
        rainforceBtn.onClick.RemoveAllListeners();
        rainforceBtn.onClick.AddListener(() => RainForceBtn(id));
    }
    
    public void ViewRainForceBtn(int id) {
        bool language = DataManager.Instance.GetLanguage();
        Text titleText = rainforcePanel.transform.Find("TitleText").GetComponent<Text>();
        titleText.color = new Color(127f / 255f, 195f / 255f, 44f / 255f);
        Text currentText = rainforcePanel.transform.Find("CurrentStateText").GetComponent<Text>();
        Text nextText = rainforcePanel.transform.Find("NextStateText").GetComponent<Text>();
        Text goldText = rainforcePanel.transform.Find("UseGoldText").GetComponent<Text>();

        Text noText = rainforcePanel.transform.Find("NoBtn/NoBtnText").GetComponent<Text>();
        Text yesText = rainforcePanel.transform.Find("YesBtn/YesBtnText").GetComponent<Text>();

        string typeString = itemTexts.ItemTextData[id].Type;

        int currentValue = ItemDataManager.Instance.GetCurrentItemType(id, typeString);
        int nextValue = ItemDataManager.Instance.GetSelectItemType( ItemDataManager.Instance.GetItemLevel(id) + 1 , id, typeString);
        if (language) {
            titleText.text = string.Format("<{0}>을\n강화하겠습니까?", nameDict[id]);
            if (nextValue == -1) titleText.text = "\n최대레벨입니다";
            switch (typeString)
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
            currentText.text = string.Format("[현재]{0}:{1}", typeString, currentValue);
            if (nextValue == -1) currentText.text = string.Format("{0}:{1}", typeString, currentValue);
            nextText.text = string.Format("[다음]{0}:{1}\n", typeString, nextValue);
            goldText.text = "필요비용 : 100골드";
            noText.text = "아니요";
            yesText.text = "예";
        }
        else {
            titleText.text = string.Format("strengthen\n<{0}>?", nameDict[id]);
            if (nextValue == -1) titleText.text = "\nMAX Level";
            currentText.text = string.Format("[Now]{0}:{1}", typeString, currentValue);
            if (nextValue == -1) currentText.text = string.Format("{0}:{1}", typeString, currentValue);
            nextText.text = string.Format("[Next]{0}:{1}\n", typeString, nextValue);
            goldText.text = "Necessary : 100G";
            noText.text = " No";
            yesText.text = "Yes";
        }
        if (nextValue == -1) {
            nextText.text = string.Format("");
            goldText.text = "";
        }
    }

    public void RainForceBtn(int id) {
        string typeString = itemTexts.ItemTextData[id].Type;
        int nextValue = ItemDataManager.Instance.GetSelectItemType(2 , id, typeString);

        blockEffect.SetActive(true);
        rainforcePanel.SetActive(true);
        ViewRainForceBtn(id);

        Button yesBtn = rainforcePanel.transform.Find("YesBtn").GetComponent<Button>();
        if (nextValue == -1) {
            yesBtn.interactable = false;
        }
        else {
            yesBtn.onClick.RemoveAllListeners();
            yesBtn.onClick.AddListener(() => YesBtn(id));
        }

        Button noBtn = rainforcePanel.transform.Find("NoBtn").GetComponent<Button>();
        noBtn.onClick.RemoveAllListeners();
        noBtn.onClick.AddListener(() => NoBtn());
    }

    private void LevelUpText(int id) {
        bool language = DataManager.Instance.GetLanguage();
        Text titleText = rainforcePanel.transform.Find("TitleText").GetComponent<Text>();
            titleText.color = new Color(127f / 255f, 195f / 255f, 44f / 255f);
            if (language) titleText.text = string.Format("현재레벨{0}\n강화성공", ItemDataManager.Instance.GetItemLevel(id));
            else titleText.text = string.Format("Level{0}\nSucess!!", ItemDataManager.Instance.GetItemLevel(id));
    }

    public void YesBtn(int id) {
        bool language = DataManager.Instance.GetLanguage();
        string typeString = itemTexts.ItemTextData[id].Type;

        int currentValue = ItemDataManager.Instance.GetCurrentItemType(id, typeString);
        int nextValue = ItemDataManager.Instance.GetSelectItemType( ItemDataManager.Instance.GetItemLevel(id) + 1 , id, typeString);
        if (nextValue == -1) {
            if (nextValue != -1) LevelUpText(id);  
            else {
                Text titleText = rainforcePanel.transform.Find("TitleText").GetComponent<Text>();
                titleText.color = new Color(127f / 255f, 195f / 255f, 44f / 255f);
                if (language) titleText.text = string.Format("\n최대레벨입니다", ItemDataManager.Instance.GetItemLevel(id));
                else titleText.text = string.Format("\nMAX LEVEL!", ItemDataManager.Instance.GetItemLevel(id));
            }
            return;
        }
        if (DataManager.Instance.GetGold() >= 100) {
            
            if (sucessSound != null) sucessSound.Play();
            DataManager.Instance.DeductGold(100);
            int gold = DataManager.Instance.GetGold();
            goldText.text = gold.ToString();
            ItemDataManager.Instance.AddItemLevel(id);
            ViewItemText(id);
            ViewRainForceBtn(id);
            if (nextValue != -1) LevelUpText(id);  
            else {
                Text titleText = rainforcePanel.transform.Find("TitleText").GetComponent<Text>();
                titleText.color = new Color(127f / 255f, 195f / 255f, 44f / 255f);
                if (language) titleText.text = string.Format("\n최대레벨입니다", ItemDataManager.Instance.GetItemLevel(id));
                else titleText.text = string.Format("\nMAX LEVEL!", ItemDataManager.Instance.GetItemLevel(id));
            }
        }
        else {
            if (failedSound != null) {
                failedSound.Play();
            }
            Text titleText = rainforcePanel.transform.Find("TitleText").GetComponent<Text>();
            titleText.color = Color.red;
            if (language) titleText.text = "돈이없어서\n강화실패";
            else titleText.text = "No money\nFailed!!";
        }
    }

    public void NoBtn() {
        blockEffect.SetActive(false);
        rainforcePanel.SetActive(false);
    }
    public void MainScreenBtn()
    {
        if (clickSound != null) clickSound.Play();
        SceneManager.LoadScene("MainScreen");
        UnityEngine.Debug.Log("메인스크린으로 이동합니다");
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
            clickSound.volume = volume;
        if (sucessSound != null) sucessSound.volume = volume;
        if (failedSound != null) failedSound.volume = volume;
    }

    // Update is called once per frame
    void Update()
    {
        if (bgmAudio.volume != AudioManager.Instance.BackGroundVolume)
            UpdateBackGroundVolume(AudioManager.Instance.BackGroundVolume);
        if (clickSound.volume != AudioManager.Instance.EffectVolume)
            UpdateEffectVolume(AudioManager.Instance.EffectVolume);
        if (sucessSound.volume != AudioManager.Instance.EffectVolume)
            UpdateEffectVolume(AudioManager.Instance.EffectVolume);
        if (failedSound.volume != AudioManager.Instance.EffectVolume)
            UpdateEffectVolume(AudioManager.Instance.EffectVolume);
    }
}
