using System;
using System.Linq;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public CinemachineCamera currentCamera;
    [SerializeField] private int activeCameraPriority = 15;
    [SerializeField] private int disableCameraPriority = 10;
    [SerializeField] private GameEventChannelSO cameraChannel;

    private void Awake()
    {
        cameraChannel.AddListener<SwapCameraEvent>(HandleSwapCamera);
        cameraChannel.AddListener<CameraShake>(HandleShakeCamera);
        currentCamera = FindObjectsByType<CinemachineCamera>(FindObjectsSortMode.None)
                        .FirstOrDefault(cam => cam.Priority == activeCameraPriority);

        ChangeCamera(currentCamera);
    }

    private void HandleShakeCamera(CameraShake obj)
    {

    }

    private void HandleSwapCamera(SwapCameraEvent swapEvt)
    {
        if (currentCamera == swapEvt.leftCamera && swapEvt.moveDirection.x > 0)
            ChangeCamera(swapEvt.rightCamera);
        else if (currentCamera == swapEvt.rightCamera && swapEvt.moveDirection.x < 0)
            ChangeCamera(swapEvt.leftCamera);
    }

    private void OnDestroy()
    {
        cameraChannel.RemoveListener<CameraShake>(HandleShakeCamera);
        cameraChannel.RemoveListener<SwapCameraEvent>(HandleSwapCamera);
    }

    public void ChangeCamera(CinemachineCamera newCamera)
    {
        currentCamera.Priority = disableCameraPriority;
        currentCamera = newCamera;
        currentCamera.Priority = activeCameraPriority;
    }
}
