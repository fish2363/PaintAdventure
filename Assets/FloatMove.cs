using UnityEngine;

public class FloatMove : MonoBehaviour
{
    public float amplitude = 0.5f;   // �����̴� ����
    public float speed = 1f;         // �����̴� �ӵ�
    private Vector3 startPos;

    void Start()
    {
        // ���� ��ġ ����
        startPos = transform.position;
    }

    void Update()
    {
        // Sin �Լ��� ����� ���Ʒ��� �ε巴�� ������
        float yOffset = Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = startPos + new Vector3(0, yOffset, 0);
    }
}
