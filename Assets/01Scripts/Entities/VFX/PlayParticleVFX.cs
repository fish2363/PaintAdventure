using UnityEngine;

public class PlayParticleVFX : MonoBehaviour, IPlayableVFX
{
    [field: SerializeField]
    public string VfxName { get; private set; }
    [SerializeField] private bool isOnPosition;
    [SerializeField] private ParticleSystem particle;
    [SerializeField] private GameObject objParticle;
    public bool TrueIsObjParticle;

    private GameObject effect;

    public void PlayVfx(Vector3 position, Quaternion rotation)
    {
        if (TrueIsObjParticle)
        {
            if (isOnPosition == false)
                transform.SetPositionAndRotation(position, rotation);
            effect = Instantiate(objParticle, transform);
        }
        else
        {
            if (isOnPosition == false)
                transform.SetPositionAndRotation(position, rotation);

            particle.Play(true); //자식들 재생중에 갑자기 멈춰버리고
                                 //다시 재생하면 문제생김 그래서 true 넣어줌 뭐지 ?
        }
    }

    public void StopVfx()
    {
        if (TrueIsObjParticle)
        {
            Destroy(effect);
        }
        else
            particle.Stop(true);
    }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(VfxName) == false)
            gameObject.name = VfxName;
    }
}
