using UnityEngine;

public abstract class State : MonoBehaviour
{
    public abstract State RunCurrentStateUpdate();
    public abstract State RunCurrentStateFixedUpdate();
    public abstract State RunCurrentStateLateUpdate();
}
