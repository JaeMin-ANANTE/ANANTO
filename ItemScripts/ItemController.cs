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
        // 초기 대각선 방향 설정
        direction = new Vector3(1, 1, 0).normalized;
    }

    void Update()
    {
        // 대각선으로 이동
        transform.position += direction * speed * Time.deltaTime;

        // 카메라 바운드 체크 및 반사
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

        direction.Normalize(); // 방향 벡터를 정규화하여 길이를 1로 유지
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Player")
        {
            UnityEngine.Debug.Log("플레이어와 아이템이 접촉했습니다");
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

