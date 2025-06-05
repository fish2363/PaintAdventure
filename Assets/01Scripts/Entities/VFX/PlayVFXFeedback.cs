using MoreMountains.Feedbacks;
using UnityEngine;

public class PlayVFXFeedback : MonoBehaviour,IPlayableFeedback
{
    [field:SerializeField]
    public string FeedbackName { get; private set; }

    [SerializeField]
    private MMF_Player mMF_Player;

    public void StopFeedback()
    {
    }

    public void PlayFeedback()
    {
        mMF_Player.PlayFeedbacks();
    }

    private void OnValidate()
    {
        if (string.IsNullOrEmpty(FeedbackName) == false)
            gameObject.name = FeedbackName;
    }
}
