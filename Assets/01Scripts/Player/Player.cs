using UnityEngine;

public class Player : Entity
{
    [field: SerializeField] public InputReader InputReader { get; private set; }
    [SerializeField] private StateDataSO[] stateDataList;

    private EntityStateMachine _stateMachine;

    
    protected override void Awake()
    {
        base.Awake();
        _stateMachine = new EntityStateMachine(this, stateDataList);
    }

    private void Start()
    {
        Debug.Log(Application.persistentDataPath);
        _stateMachine.ChangeState("IDLE");
    }

    private void Update()
    {
        _stateMachine.UpdateState();
    }

    public void ChangeState(string newStateName)
        => _stateMachine.ChangeState(newStateName);
}
