using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public AudioSource dieSound;
    private float speed = 10.0f;

    public void Start()
    {
        AudioManager.Instance.OnEffectVolumeChanged += UpdateEffectVolume;
        UpdateEffectVolume(AudioManager.Instance.EffectVolume);
    }

    void UpdateEffectVolume(float volume)
    {
        if (dieSound != null) {
            dieSound.volume = volume;
        }
    }

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;

            RotateTowards(mousePosition);
            // ���콺 Ŭ�� ��ġ�� �÷��̾ �̵�
            transform.position = Vector2.MoveTowards(transform.position, mousePosition, speed * Time.deltaTime);
        }
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 120);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            UnityEngine.Debug.Log("�÷��̾ ���� �����߽��ϴ�");
            if (dieSound != null) dieSound.Play();
            GameManager.Instance.GameOver();
        }
    }
}