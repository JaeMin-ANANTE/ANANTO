using System.Collections;
using System.Diagnostics;
using System.Security.Cryptography;
using UnityEngine;

public class FlowerBombItem : MonoBehaviour
{
    private float rotationSpeed = 90.0f;
    private float duration;

    //public override void Activate()
        //duration = ItemDataManager.Instance.GetCurrentItemType(0, "duration");

    void Awake()
    {
        duration = ItemDataManager.Instance.GetCurrentItemType(0, "duration");
        transform.localScale *= 1.75f;
    }

    void Start()
    {
        StartCoroutine(RotateForDuration());
    }

    IEnumerator RotateForDuration()
    {
        float elapsedTime = 0.0f;

        while (elapsedTime < duration)
        {
            // 회전
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            // 경과 시간 업데이트
            elapsedTime += Time.deltaTime;

            yield return null; // 다음 프레임까지 대기
        }

        // 회전이 끝나면 아이템 비활성화 또는 기타 처리
        Destroy(gameObject);
    }
}

public class FlowerShieldItem : MonoBehaviour
{
    private float duration;
    private GameObject player;
    void Awake()
    {
        duration = ItemDataManager.Instance.GetCurrentItemType(1, "duration");
        player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            UnityEngine.Debug.LogError("Player not found");
            return;
        }

        StartCoroutine(DeactivateAfterDuration());
    }

    void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
        }
    }

    IEnumerator DeactivateAfterDuration()
    {
        yield return new WaitForSeconds(duration);
        Destroy(gameObject);
    }
}

public class BeeItem : MonoBehaviour
{
    private float speed;

    void Awake()
    {
        speed = ItemDataManager.Instance.GetCurrentItemType(2, "speed");
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        
        if (IsOutSidePosition())
        {
            Destroy(gameObject);
        }
    }

    bool IsOutSidePosition()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPosition.y > 1;
    }
}

public class MiniBugItem : MonoBehaviour
{
    private float speed;
    private GameObject player;
    private float count;

    void Awake()
    {
        speed = 7.0f;
        count = ItemDataManager.Instance.GetCurrentItemType(3, "count");
        player = GameObject.FindWithTag("Player");

    }

    void Update()
    {
        //if (player != null)
            //Vector2 playerDirection = player.transform.up;
        //Vector2 diagonalComponent = new Vector2(0.1f, 0.1f);
        //Vector2 moveDirection = (playerDirection + diagonalComponent).normalized;

        // 계산된 방향으로 이동합니다.
        //transform.position += (Vector3)(moveDirection * speed * Time.deltaTime);
    }
}

public class BeanWormItem : MonoBehaviour
{

}


public class MiniBug2Item : MonoBehaviour
{
}

public class CosmosItem : MonoBehaviour { 

}

public class Cosmos2Item : MonoBehaviour
{
    private float speed = 1.5f;
    private float count;

    void Awake()
    {
        count = ItemDataManager.Instance.GetCurrentItemType(7, "count");
    }

    void Update()
    {
        transform.position += transform.up * speed * Time.deltaTime;
        
        if (IsOutSidePosition())
        {
            Destroy(gameObject);
        }
    }

    bool IsOutSidePosition()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        if (viewportPosition.y > 1 | viewportPosition.y < 0) {
            count--;
            speed *= -1;
        }
        if (count == 0) return true;
        else return false;
    }
}
