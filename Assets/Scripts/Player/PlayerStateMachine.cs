using System.Collections;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }
    private PlayerController _controller;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
    }

    /// <summary>
    /// ƒvƒŒƒCƒ„[‚Ìó‘Ô•Ï‰»
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(PlayerState state)
    {
        if(CurrentState == state) return;

        ExitState(CurrentState);
        CurrentState = state;
        EnterState(CurrentState);
    }

    private void EnterState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Attack:
                //UŒ‚”»’èì‚é

                break;
            case PlayerState.Dodge:
                //–³“G”»’è‚Â‚­‚é

                break;
            case PlayerState.Hide:
                //–¾‚é‚³’²®ˆ—‚Â‚­‚é

                break;
        }
        _controller.ChangeColliderHeight(state);
    }
    private void ExitState(PlayerState state)
    {
        switch (state)
        {
            case PlayerState.Hide:
                
                break;
        }
    }
}
