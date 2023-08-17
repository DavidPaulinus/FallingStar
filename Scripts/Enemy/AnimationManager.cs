using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator anima;
    private StateManager stateManager;

    private void Awake()
    {
        anima = GetComponent<Animator>();
        stateManager = GetComponent<StateManager>();
    }

    void Update()
    {
        var _state = StateToString.ConvertState(stateManager.GetCurrentState().ToString());
        anima.CrossFade(_state, 0);
    }
}
