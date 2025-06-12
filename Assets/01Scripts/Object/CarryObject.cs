using UnityEngine;

public class CarryObject : WeightObject
{

    public HandlePoint _handlePoint { get; private set; }
    private CanvasGroup canvasGroup;
    private Player _player;
    protected override void Awake()
    {
        base.Awake();
        _handlePoint = GetComponentInChildren<HandlePoint>();
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        _player = FindAnyObjectByType<Player>();
        _player.OnPlayerChange.AddListener(OnChanged);
    }

    private void OnChanged(bool isforced)
    {
        if (!isforced)
        {
            int change = FindAnyObjectByType<Player>().CurrentPlayer().playerName == "PaintMonster" ? 1 : 0;
            canvasGroup.alpha = change;
        }
        else
        {
            canvasGroup.alpha = 0;
        }
    }
    private void OnDisable()
    {
        _player.OnPlayerChange.RemoveListener(OnChanged);
    }

}
