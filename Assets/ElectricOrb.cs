using UnityEngine;

public class ElectricOrb : MonoBehaviour
{
    public Material[] material;
    private MeshRenderer renderer;
    public bool IsElectoronic;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    public void OnElectric()
    {
        IsElectoronic = true;
        renderer.materials[2] = material[1];
    }
    public void OutElectric()
    {
        IsElectoronic =false;
        renderer.materials[2] = material[0];
    }
}
