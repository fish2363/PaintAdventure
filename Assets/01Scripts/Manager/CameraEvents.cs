using Unity.Cinemachine;
using UnityEngine;

public static class CameraEvents
{
    public static SwapCameraEvent SwapCameraEvent = new();
    public static SwapCameraEvent CameraShakeEvent = new();
}

public class SwapCameraEvent : GameEvent
{
    public CinemachineCamera leftCamera;
    public CinemachineCamera rightCamera;
}

public class CameraShake : GameEvent
{
    public float intensity;
    public float second;
}
