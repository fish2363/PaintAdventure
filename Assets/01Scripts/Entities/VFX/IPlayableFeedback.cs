using UnityEngine;

public interface IPlayableFeedback
{
    public string FeedbackName { get; }
    public void PlayFeedback();
    public void StopFeedback();
}
