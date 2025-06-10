using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UI;

public static class UIEvents
{
    public static StartTipDialogueEvent StartTipDialogueEvent = new();
    public static StageNameEvent StageNameEvent = new();
    public static SkillUIEvent SkillUIEvent = new();
    public static TutorialEvent TutorialEvent = new();
}

public class StartTipDialogueEvent : GameEvent
{
    public string tipText;
    public string tipTrigger;
}
public class TutorialEvent : GameEvent
{
    public string tutorialText;
    public KeyCode skipKey;
}
public class StageNameEvent : GameEvent
{
    public string Text;
    public float duration;
}

public class SkillUIEvent : GameEvent
{
    public PlayerType type;
    public bool isHide;
}
