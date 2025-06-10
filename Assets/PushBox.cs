using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PushBox : CarryObject
{
    [SerializeField] private Transform spanwPoint;
    private CanvasGroup canvasGroup;
    private Player _player;

    protected override void Awake()
    {
        base.Awake();
        canvasGroup = GetComponentInChildren<CanvasGroup>();
        _player = FindAnyObjectByType<Player>();
        _player.OnPlayerChange.AddListener(OnChanged);
    }
    private void OnChanged(bool isforced)
    {
        if(!isforced)
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
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Dead")
        {
            transform.position = spanwPoint.position;
        }
    }
}
