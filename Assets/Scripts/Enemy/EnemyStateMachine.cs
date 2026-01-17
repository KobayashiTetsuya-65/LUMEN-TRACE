using UnityEngine;
/// <summary>
/// “G‚Ìó‘ÔŠÇ—
/// </summary>
public class EnemyStateMachine : MonoBehaviour
{
    public EnemyState CurrentState { get; private set; }

    private NormalEnemyController _controller;
    private void Awake()
    {
        _controller = GetComponentInChildren<NormalEnemyController>();
    }
    /// <summary>
    /// ó‘Ô‚ğØ‚è‘Ö‚¦‚ÄAØ‚è‘Ö‚¦‚Ìˆ—‚ğŒÄ‚Ô
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(EnemyState state)
    {
        if(CurrentState == state)return;

        ExitState(state);
        CurrentState = state;
        EnterState(state);
    }
    private void EnterState(EnemyState state)
    {
        switch (state)
        {
            
        }
    }
    private void ExitState(EnemyState state)
    {
        switch (state)
        {
            case EnemyState.Attack:
                _controller.Attack(false); 
                break;
        }
    }
}
