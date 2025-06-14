using UnityEngine;
using UnityEngine.Events;

public enum Line
{
    Horizontal,
    Vertical
}
public class ElectricArea : MonoBehaviour
{
    public ElectricOrb left;
    public ElectricOrb right;
    private LineRenderer renderer;
    public UnityEvent OnLightEvent;
    public Line line;

    private void Awake()
    {
        renderer = GetComponent<LineRenderer>();
    }
    public bool CanLightOn(Line drawLine)
    {
        if ((left.IsElectoronic || right.IsElectoronic) && drawLine == line)
        {
            DrawElect();
            return true;
        }
        else return false;
    }
    public void DrawElect()
    {
        renderer.positionCount = 2;
        OnLightEvent?.Invoke();
        renderer.SetPosition(0, left.transform.position);
        renderer.SetPosition(1, right.transform.position);
    }
}
