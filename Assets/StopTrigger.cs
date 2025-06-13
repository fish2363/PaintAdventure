using UnityEngine;

public class StopTrigger : MonoBehaviour
{
    public float pauseDuration = 2f;
    public bool isImmately;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out MovePlace platform))
        {
            if (platform != null)
            {
                platform.PauseMovement(isImmately, pauseDuration);
            }
        }
    }
}
