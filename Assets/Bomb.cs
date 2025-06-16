using UnityEngine;

public class Bomb : ObjectUnit
{
    [SerializeField] private ParticleSystem explosion;
    [SerializeField] private LayerMask _whatIsDamageObj;
    private float minCheckTime;

    void Update()
    {
        minCheckTime += Time.deltaTime;
        if(Physics.CheckSphere(transform.position, 5f, _whatIsDamageObj) && minCheckTime> 1f)
        {
            explosion.Play();
            FindAnyObjectByType<BossAI>().Damage(gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 5f);
        Gizmos.color = Color.white;
    }
}
