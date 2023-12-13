using System.Collections;
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


