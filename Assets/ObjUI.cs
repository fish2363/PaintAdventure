using UnityEngine;

public class ObjUI : MonoBehaviour
{
    void Update()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0; // Y�� ����
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Y������ 180�� ȸ�� �߰�
        targetRotation *= Quaternion.Euler(0, 180, 0);

        transform.rotation = targetRotation;
    }
}
