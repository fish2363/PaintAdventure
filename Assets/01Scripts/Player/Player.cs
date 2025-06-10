using System;
using UnityEngine;

public class Player : Entity
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [field: SerializeField] public GameEventChannelSO UIChannel { get; private set; }

    [SerializeField] private StateDataSO[] stateDataList;

    private EntityStateMachine _stateMachine;
    public float detectionDistance;
    public LayerMask whatIsInteractableObj;
    public CarryObject catchObj;
    private PlayerChanger _playerChanger;
    public Transform startPoint;
    public LayerMask whatIsDeadPlace;

    

    protected override void Awake()
    {
        base.Awake();
        _stateMachine = new EntityStateMachine(this, stateDataList);
    }

    
    private void Start()
    {
        _playerChanger = GetComponentInChildren<PlayerChanger>();
        Debug.Log(Application.persistentDataPath);
        _stateMachine.ChangeState("IDLE");
    }

    private void Update()
    {
        _stateMachine.UpdateState();
    }
    public PlayerType CurrentPlayer()
    {
        return _playerChanger.currentPlayer;
    }
    public void ChangePlayer()
    {
        int num = _playerChanger.currentPlayer.playerName == "Bear" ? 1 : 0;
        _playerChanger.ChangePlayer(num);
        ChangeState("CHANGE");
    }
    public void ReStartSet(bool isDead,string[] canGesture = null)
    {
        transform.position = startPoint.position;
        if(!isDead)
            GetCompo<DrawManager>().currentCanGestures = canGesture;
    }
    public void ChangeState(string newStateName)
        => _stateMachine.ChangeState(newStateName);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,detectionDistance);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Dead")
        {
            StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
            startTipDialogueEvent.tipText = "아이들이 볼 수도\n있으니까 죽는 모습은\n그리면 안돼요 ㅡㅡ";
            UIChannel.RaiseEvent(startTipDialogueEvent);
            ReStartSet(true);
        }
    }
}
