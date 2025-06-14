using UnityEngine;
using UnityEngine.Events;

public class ElectricManager : MonoBehaviour
{
    private int currentLightCnt;
    public int maxLightcnt;
    public UnityEvent OnClear;

    private bool isEnd;
    
    public void LightOn()
    {
        currentLightCnt++;
        if (currentLightCnt >= maxLightcnt&&isEnd)
        {
            OnClear?.Invoke();
            isEnd = true;
        }
    }
}
