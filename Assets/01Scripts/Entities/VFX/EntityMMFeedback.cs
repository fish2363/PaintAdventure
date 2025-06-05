using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EntityMMFeedback : MonoBehaviour, IEntityComponent
{
    private Dictionary<string, IPlayableFeedback> _playableDictionary;
    private Entity _entity;

    public void Initialize(Entity entity)
    {
        _entity = entity;
        _playableDictionary = new Dictionary<string, IPlayableFeedback>();
        GetComponentsInChildren<IPlayableFeedback>().ToList()
            .ForEach(playable => _playableDictionary.Add(playable.FeedbackName, playable));
    }

    public void PlayFeedback(string vfxName)
    {
        IPlayableFeedback vfx = _playableDictionary.GetValueOrDefault(vfxName);
        Debug.Assert(vfx != default(IPlayableFeedback), $"{vfxName} is not exist in dictionary");

        vfx.PlayFeedback();
    }

    public void StopFeedback(string vfxName)
    {
        IPlayableFeedback vfx = _playableDictionary.GetValueOrDefault(vfxName);
        Debug.Assert(vfx != default(IPlayableFeedback), $"{vfxName} is not exist in dictionary");
        vfx.StopFeedback();
    }
}

