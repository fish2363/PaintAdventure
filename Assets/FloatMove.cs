using UnityEngine;

public class FloatMove : MonoBehaviour
{
    public float amplitude = 0.5f;   // 움직이는 높이
    public float speed = 1f;         // 움직이는 속도
    private Vector3 startPos;

    void Start()
    {
        // 시작 위치 저장
        startPos = transform.position;
    }

    void Update()
    {
        // Sin 함수를 사용해 위아래로 부드럽게 움직임
        float yOffset = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = startPos + new Vector3(0, yOffset, 0);
    }
}
