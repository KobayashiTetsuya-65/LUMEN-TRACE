using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public EnemyState CurrentState { get; private set; }
    
    public void ChangeState(EnemyState state)
    {
        if(CurrentState == state)return;

        ExitState();
        CurrentState = state;
        EnterState();
    }
    private void EnterState()
    {

    }
    private void ExitState()
    {

    }
}
