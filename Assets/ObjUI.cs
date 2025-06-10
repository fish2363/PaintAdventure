using UnityEngine;

public class ObjUI : MonoBehaviour
{
    void Update()
    {
        Vector3 direction = Camera.main.transform.position - transform.position;
        direction.y = 0; // Y축 고정
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Y축으로 180도 회전 추가
        targetRotation *= Quaternion.Euler(0, 180, 0);

        transform.rotation = targetRotation;
    }
}
