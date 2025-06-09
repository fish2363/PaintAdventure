using UnityEngine;
using UnityEngine.Playables;

public class Stage2Logic : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    public void PushFirstButton()
    {
        director.Play();
    }
}
