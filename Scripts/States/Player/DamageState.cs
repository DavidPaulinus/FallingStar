using UnityEngine;

public class DamageState : State
{
    private Rigidbody2D body;

    [SerializeField] private MovementPlayerState movementState;
    [SerializeField] private float timeToMoveAgain;
    [SerializeField] private float moveX;
    [SerializeField] private float moveY;


    private float timeToMoveAgainCounter;

    private void Awake()
    {
        timeToMoveAgainCounter = timeToMoveAgain;

        body = GetComponent<Rigidbody2D>();
    }

    public override State RunCurrentStateFixedUpdate()
    {
        body.MovePosition(new Vector2(body.position.x + moveX * -PlayerManager.instance.FacingDirection(), body.position.y + moveY));

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

            return movementState;
        }
        timeToMoveAgainCounter -= Time.deltaTime;

        return this;
    }
}
