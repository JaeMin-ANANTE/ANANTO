using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private float speed = 1.0f; // 적의 이동 속도
    private Transform player; // 플레이어의 위치 정보
    private Vector3 diagonalDirection; // 대각선 이동 방향
    private bool isDiagonalSet = false; // 대각선 방향이 설정되었는지 확인

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (player.position.y < transform.position.y)
        {
            // 플레이어가 적보다 아래에 있을 경우, 대각선 이동 방향을 재설정합니다.
            isDiagonalSet = false;
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            transform.position += directionToPlayer * speed * Time.deltaTime;
            RotateTowards(player.position); // Rotate towards the player
        }
        else if (!isDiagonalSet)
        {
            // 플레이어가 적보다 높이 있을 경우, 대각선 방향을 결정합니다.
            float horizontalDirection = (player.position.x > transform.position.x) ? 1 : -1;
            float randomSlope = UnityEngine.Random.Range(0.5f, 1f);
            diagonalDirection = new Vector3(horizontalDirection, -randomSlope, 0).normalized;
            isDiagonalSet = true;
            RotateTowards(transform.position + diagonalDirection); // Rotate in diagonal direction
        }

        // 대각선 방향으로 이동합니다.
        if (isDiagonalSet)
        {
            transform.position += diagonalDirection * speed * Time.deltaTime;
            RotateTowards(transform.position + diagonalDirection); // Continue rotating in diagonal direction
        }

        if (IsOutSidePosition())
        {
            Destroy(gameObject);
        }
    }

    bool IsOutSidePosition()
    {
        Vector3 viewportPosition = Camera.main.WorldToViewportPoint(transform.position);
        return viewportPosition.x < 0 || viewportPosition.x > 1 || viewportPosition.y < 0 || viewportPosition.y > 1;
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle - 120);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Item")
        {
            UnityEngine.Debug.Log("아이템과 적과 접촉했습니다");
            Destroy(gameObject);
        }
    }
}
