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
            // ȸ��
            transform.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);

            // ��� �ð� ������Ʈ
            elapsedTime += Time.deltaTime;

            yield return null; // ���� �����ӱ��� ���
        }

        // ȸ���� ������ ������ ��Ȱ��ȭ �Ǵ� ��Ÿ ó��
        Destroy(gameObject);
    }
}


