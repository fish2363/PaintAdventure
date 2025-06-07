using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public static class UIEvents
{
    public static StartTipDialogueEvent StartTipDialogueEvent = new();
    public static StageNameEvent StageNameEvent = new();
}

public class StartTipDialogueEvent : GameEvent
{
    public string[] tipText;
    public MMF_Player feedback;
    public Sprite characterIllustration;
}

public class StageNameEvent : GameEvent
{
    public string Text;
    public float duration;
}
