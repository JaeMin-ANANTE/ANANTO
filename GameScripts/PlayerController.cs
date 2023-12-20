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
            // 마우스 클릭 위치로 플레이어를 이동
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
            UnityEngine.Debug.Log("플레이어가 적과 접촉했습니다");
            if (dieSound != null) dieSound.Play();
            GameManager.Instance.GameOver();
        }
    }
}