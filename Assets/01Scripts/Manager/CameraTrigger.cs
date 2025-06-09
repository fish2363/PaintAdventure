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
            SwapCameraEvent swapEvt = CameraEvents.SwapCameraEvent;
            swapEvt.leftCamera = leftCamera;
            swapEvt.rightCamera = rightCamera;
            OnCameraTriggerEvent?.Invoke();

            cameraChannel.RaiseEvent(swapEvt);
        }
    }
}
