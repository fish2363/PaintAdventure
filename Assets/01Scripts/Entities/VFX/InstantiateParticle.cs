using UnityEngine;

public class InstantiateParticle : MonoBehaviour, IPlayableVFX
{
    [field: SerializeField]
    public string VfxName { get; private set; }

    [SerializeField] private ParticleSystem particle;

    [SerializeField] private bool isOnPosition;
    [SerializeField] private GameObject objParticle;
    public bool TrueIsObjParticle;

    private GameObject effect;

    public void PlayVfx(Vector3 position, Quaternion rotation)
    {
        if (isOnPosition == false)
            transform.SetPositionAndRotation(position, rotation);

        if (!TrueIsObjParticle)
        {
            effect = Instantiate(particle.gameObject);
            effect.GetComponent<ParticleSystem>().Play(true);
        }
        else
            effect = Instantiate(objParticle);
    }

    public void StopVfx()
    {
        Destroy(effect);
    }
}
