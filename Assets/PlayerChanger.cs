using System;
using UnityEngine;

public class PlayerChanger : MonoBehaviour,IEntityComponent
{
    private EntityVFX _entityVFX;
    public PlayerType[] PlayerList;
    public PlayerType currentPlayer { get; private set; }
    private EntityAnimator _entityAnimator;
    private bool isStart;
    public void ChangePlayer(int count)
    {
        foreach(PlayerType p in PlayerList)
            p.visual.SetActive(false);

        currentPlayer = PlayerList[count];
        currentPlayer.visual.SetActive(true);
        _entityAnimator.animator = currentPlayer.visual.GetComponent<Animator>();
        if(isStart)
        _entityVFX.PlayVfx("PaintMonsterChange",Vector3.zero,Quaternion.identity);
        isStart = true;
    }

    public void Initialize(Entity entity)
    {
        _entityAnimator = entity.GetCompo<EntityAnimator>();
        _entityVFX = entity.GetCompo<EntityVFX>();
    }

    private void Start()
    {
        ChangePlayer(0);
    }
}

[Serializable]
public struct PlayerType
{
    public float GroundCheckDistance;
    public float WalkSpeed;
    public GameObject visual;
    public string playerName;
}
