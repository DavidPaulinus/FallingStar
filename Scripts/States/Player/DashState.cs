using UnityEngine;

public class DashState : State
{
    private Rigidbody2D body;
    [SerializeField] private BoxCollider2D attackBox;

    [SerializeField] private MovementPlayerState movementState;
    [SerializeField] private float gravityCoolDownTime;

    [Space(10)]
    [SerializeField] private float dashStayingTime;
    [SerializeField] private float dashSpeed;

    private float x;
    private float y;

    private float dashStayingCounter;

    private void Awake()
    {
        dashStayingCounter = dashStayingTime;

        body = GetComponent<Rigidbody2D>();
    }

    public override State RunCurrentStateFixedUpdate()
    {
        Physics2D.IgnoreLayerCollision(9, 8, true);

        body.velocity = new Vector2(dashSpeed * x, dashSpeed * y);

        return this;
    }

    public override State RunCurrentStateLateUpdate()
    {
        return this;
    }

    public override State RunCurrentStateUpdate()
    {
        attackBox.enabled = true;
        if (dashStayingCounter <= 0)
        {
            attackBox.enabled = false;
            body.velocity = new Vector2(.5f,.5f);

            Physics2D.IgnoreLayerCollision(9, 8, false);
            dashStayingCounter = dashStayingTime;

            movementState.SetGravityCooldownTime(gravityCoolDownTime);
            return movementState;
        }
        dashStayingCounter-= Time.deltaTime;

        return this;
    }

    public void SetDashData(float x, float y)
    {
        this.x = x; 
        this.y = y;
    }
    public void IncreaseDashTime(float time)
    {
        dashStayingCounter += time;
    }
}
