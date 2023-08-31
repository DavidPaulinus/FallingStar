using UnityEngine;

public class AnimationPlayerManager : MonoBehaviour
{
    private Wall wall;
    private Animator anima;
    private Rigidbody2D body;
    private InputManager input;
    private StateManager stateManager;

    private string stateAnima;

    private void Awake()
    {
        anima = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        input= GetComponent<InputManager>();
        stateManager = GetComponent<StateManager>();

        wall = GetComponentInChildren<Wall>();
    }

    void Update()
    {
        stateAnima = StateToString.ConvertState(stateManager.GetCurrentState().ToString());

        if (stateAnima.Equals("MovementPlayer"))
        {
            if (input.RetriveXValue() != 0)
            {
                stateAnima = "Run";
            } else stateAnima = "Idle";

            if (body.velocity.y < -.1f)
            {
                stateAnima = "Fall";
            } else if (body.velocity.y > .1f) stateAnima = "Jump";

            if (wall.OnWall())
            {
                stateAnima = "WallSlide";
            }
        }

        anima.CrossFade(stateAnima, 0);
    }
}
