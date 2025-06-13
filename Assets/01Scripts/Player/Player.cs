using UnityEngine;
using UnityEngine.Events;

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
    public UnityEvent<bool> OnPlayerChange;

    public static bool IsCanDraw;
    public static bool IsCanChange;

    public Transform secondPos;

    protected override void Awake()
    {
        base.Awake();
        _stateMachine = new EntityStateMachine(this, stateDataList);
    }

    public void ChangeCanDraw(bool s)
    {
        IsCanDraw = s;
    }

    public void SaveToSecondPos(Transform trans)
    {
        if (trans == null) secondPos = null;
        secondPos = trans;
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
        OnPlayerChange?.Invoke(false);
        ChangeState("CHANGE");
    }
    public void ReStartSet(bool isDead,string[] canGesture = null)
    {
        if (secondPos != null) transform.position = secondPos.position;
        else transform.position = startPoint.position;
        if(!isDead)
            GetCompo<DrawManager>().currentCanGestures = canGesture;
    }
    public void ChangeState(string newStateName)
        => _stateMachine.ChangeState(newStateName);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,detectionDistance);
        Gizmos.color = Color.white;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Dead")
        {
            string[] dialogue = { "아니 떨어지는 걸\n그리시면 어떡해요..", "아이들이 볼 수도\n있으니까 죽는 모습은\n그리면 안돼요 ㅡㅡ" };
            StartTipDialogueEvent startTipDialogueEvent = UIEvents.StartTipDialogueEvent;
            startTipDialogueEvent.tipText = dialogue;
            startTipDialogueEvent.tipTrigger = "";
            UIChannel.RaiseEvent(startTipDialogueEvent);
            ReStartSet(true);
            OnDeadEvent?.Invoke();
        }
    }
}
