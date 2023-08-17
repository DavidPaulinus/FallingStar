using UnityEngine;

public class AttackState : State
{
    private Rigidbody2D body;
    private Ground ground;

    [SerializeField] private MovementPlayerState moveState;
    [SerializeField] private float timeToMoveAgain;

    [SerializeField] private float moveX;

    private float timeToMoveAgainCounter;

    private void Awake()
    {
        timeToMoveAgainCounter = timeToMoveAgain;

        body = GetComponentInParent<Rigidbody2D>();
        ground = GetComponentInParent<Ground>();
    }

    public override State RunCurrentStateFixedUpdate()
    {
        body.velocity = Vector3.zero;

        var _move = ground.Grounded() ? moveX : 0;

        body.MovePosition(new Vector2(body.position.x + _move * PlayerManager.instance.FacingDirection()/10, body.position.y));

        return this;
    }

    public override State RunCurrentStateLateUpdate()
    {
        return this;
    }

    public override State RunCurrentStateUpdate()
    {
        if (timeToMoveAgainCounter <= 0)
        {
            timeToMoveAgainCounter = timeToMoveAgain;

            return moveState;
        }
        timeToMoveAgainCounter -= Time.deltaTime;

        return this;
    }
}
