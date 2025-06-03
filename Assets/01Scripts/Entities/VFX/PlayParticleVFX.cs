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

            particle.Play(true); //�ڽĵ� ����߿� ���ڱ� ���������
                                 //�ٽ� ����ϸ� �������� �׷��� true �־��� ���� ?
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
