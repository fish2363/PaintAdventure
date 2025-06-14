using DG.Tweening;
using Unity.Cinemachine;
using UnityEngine;

public class BossSceneLogic : MonoBehaviour
{
    [SerializeField] private CinemachineCamera cinemachine;

    public void ChangeFOV(float value)
    {
        DOTween.To(() => cinemachine.Lens.FieldOfView, x => cinemachine.Lens.FieldOfView = x, value, 0.2f);
    }
}
