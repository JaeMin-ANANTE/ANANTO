using System;
using System.Collections;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class ItemController : MonoBehaviour
{
    private float speed;
    private Vector3 direction;
    private float randomness;
    public int id;

    void Start()
    {
        randomness = 0.05f;
        speed = 1.0f;
        // �ʱ� �밢�� ���� ����
        direction = new Vector3(1, 1, 0).normalized;
    }

    void Update()
    {
        // �밢������ �̵�
        transform.position += direction * speed * Time.deltaTime;

        // ī�޶� �ٿ�� üũ �� �ݻ�
        CheckBoundsAndReflect();
    }

    void CheckBoundsAndReflect()
    {
        Vector3 minViewport = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane));
        Vector3 maxViewport = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, Camera.main.nearClipPlane));

        if (transform.position.x < minViewport.x || transform.position.x > maxViewport.x)
        {
            direction.x = -direction.x + UnityEngine.Random.Range(-randomness, randomness);
        }
        if (transform.position.y < minViewport.y || transform.position.y > maxViewport.y)
        {
            direction.y = -direction.y + UnityEngine.Random.Range(-randomness, randomness);
        }

        direction.Normalize(); // ���� ���͸� ����ȭ�Ͽ� ���̸� 1�� ����
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            UnityEngine.Debug.Log("�÷��̾�� �������� �����߽��ϴ�");
            GameManager.UpdateItemCounter();
            ActivateItem();
            Destroy(gameObject);
        }
    }

    void ActivateItem()
    {
        GameObject newItem = ItemFactoryManager.CreateItem(id, transform.position);
    }
}

