using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Diagnostics;

public class ItemController : MonoBehaviour
{
    private SpriteRenderer render;

    public int id;

    // DataManager�� ������ �����ϴ� ������ �߰��մϴ�.
    private DataManager data;

    void Start()
    {
        data = DataManager.Instance;
        render = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    void OnTriggerEnter(Collider other) // Collider�� �Ķ���ͷ� ����մϴ�.
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.tag = "Item";
            render.sprite = data.items[id].image;
            UnityEngine.Debug.Log("���������� �������� Ȱ��ȭ�Ǿ����ϴ�");
        }
    }
}
