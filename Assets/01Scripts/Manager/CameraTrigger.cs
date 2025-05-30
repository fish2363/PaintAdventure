using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CameraTrigger : MonoBehaviour
{
    public CinemachineCamera leftCamera;
    public CinemachineCamera rightCamera;

    [SerializeField] private GameEventChannelSO cameraChannel;

    public UnityEvent OnCameraTriggerEvent;

    private void OnTriggerEnter(Collider other)
    {
        if (leftCamera is null || rightCamera is null) return;

        if (other.CompareTag("Player"))
        {
            Vector3 exitDirection = (other.transform.position - transform.position).normalized;
            SwapCameraEvent swapEvt = CameraEvents.SwapCameraEvent;
            swapEvt.leftCamera = leftCamera;
            swapEvt.rightCamera = rightCamera;
            swapEvt.moveDirection = exitDirection;
            OnCameraTriggerEvent?.Invoke();

            cameraChannel.RaiseEvent(swapEvt);
        }
    }
}
