using System.Collections;
using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    public PlayerState CurrentState { get; private set; }

    private PlayerController _controller;
    private PlayerSpriteAnimator _animator;

    private Coroutine _coroutine;

    private void Awake()
    {
        _controller = GetComponent<PlayerController>();
        _animator = GetComponent<PlayerSpriteAnimator>();
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
                _coroutine = StartCoroutine(CheckActionFinish());
                break;
            case PlayerState.Dodge:
                //–³“G”»’è‚Â‚­‚é
                _coroutine = StartCoroutine(CheckActionFinish());
                break;
            case PlayerState.Hide:
                //–¾‚é‚³’²®ˆ—‚Â‚­‚é
                _coroutine = StartCoroutine(CheckActionFinish());
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
    private IEnumerator CheckActionFinish()
    {
        yield return new WaitUntil(() => _animator.IsFinishAction);
        _controller.Attack(false);
        _controller.ChangeInvincibleState(false);
        _controller.Hide(false);
        ChangeState(PlayerState.Idle);
        _animator.FinishAction(false);
    }
}
