using UnityEngine;
using UnityEngine.Playables;

public class Stage2Logic : MonoBehaviour
{
    [Header("¹ÌÄ¡°Ô ±ÍÂú´Ù")]
    [SerializeField] private PlayableDirector firstDirector;
    [Header("¹ÌÄ¡°Ô ±ÍÂú2´Ù")]
    [SerializeField] private PlayableDirector secondDirector;
    [SerializeField] private PlayableDirector secondExitDirector;
    [Header("Á¹¸®´Ù")]
    [SerializeField] private PlayableDirector thirdDirector;
    [SerializeField] private PlayableDirector thirdExitDirector;
    public void PushFirstButton(bool isbuttonOn)
    {
        if (isbuttonOn)
            firstDirector.Play();
    }
    public void PushSecondButton(bool isbuttonOn)
    {
        if (isbuttonOn)
        {
            secondExitDirector.Stop();
            secondDirector.Play();
        }
        else
        {
            secondDirector.Stop();
            secondExitDirector.Play();
        }
    }
    public void PushThirdButton(bool isbuttonOn)
    {
        if (isbuttonOn)
        {
            thirdExitDirector.Stop();
            thirdDirector.Play();
        }
        else
        {
            thirdDirector.Stop();
            thirdExitDirector.Play();
        }
    }
}
