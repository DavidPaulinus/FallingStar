using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private State currentState;

    void Update()
    {
        RunStateMachineUpdate();
    }

    void FixedUpdate()
    {
        RunStateMachineFixedUpdate();
    }
    private void LateUpdate()
    {
        RunStateMachineLateUpdate();
    }

    private void RunStateMachineUpdate()
    {
        State nextState = currentState?.RunCurrentStateUpdate();

        if (nextState != null)
        {
            SwitchToNextState(nextState);
        }
    }
    private void RunStateMachineFixedUpdate()
    {
        State nextState = currentState?.RunCurrentStateFixedUpdate();

        if (nextState != null)
        {
            SwitchToNextState(nextState);
        }
    }
    private void RunStateMachineLateUpdate()
    {
        State nextState = currentState?.RunCurrentStateLateUpdate();

        if (nextState != null)
        {
            SwitchToNextState(nextState);
        }
    }

    public void SwitchToNextState(State next)
    {
        currentState = next;
    }

    public State GetCurrentState()
    {
        return currentState;
    }
}
