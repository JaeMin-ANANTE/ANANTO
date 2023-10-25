using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class ItemController : MonoBehaviour
{
    private SpriteRenderer render;

    public int id;

    // DataManager의 참조를 저장하는 변수를 추가합니다.
    private DataManager data;

    void Start()
    {
        data = DataManager.Instance;
        render = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other) // Collider를 파라미터로 사용합니다.
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.tag = "Item";
            render.sprite = data.items[id].image;
            UnityEngine.Debug.Log("성공적으로 아이템이 활성화되었습니다");
        }
    }
}
