using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class DamageEnemyState : State
{
    private Rigidbody2D body;
    private Ground ground;

    private float x;
    private float y;

    [SerializeField] private State nextState;

    [SerializeField] private float timeToChanceState;
    private float timeToChanceStateCounter;

    private void Awake()
    {
        timeToChanceStateCounter = timeToChanceState;

        ground = GetComponent<Ground>();
        body = GetComponent<Rigidbody2D>();
    }

    public override State RunCurrentStateFixedUpdate()
    {
        body.velocity = Vector3.zero;

        var _direction = transform.position.x > PlayerManager.instance.transform.position.x ? 1 : -1;

        if (timeToChanceStateCounter > 0)
        {
            body.MovePosition(new Vector2(body.position.x + x/5 * _direction, body.position.y + y/5));
        }

        return this;
    }

    public override State RunCurrentStateLateUpdate()
    {
        return this;
    }

    public override State RunCurrentStateUpdate()
    {
        if (ground.Grounded() && timeToChanceStateCounter <= 0)
        {
            timeToChanceStateCounter = timeToChanceState;

            return nextState;
        }
        timeToChanceStateCounter -= Time.deltaTime;
        return this;
    }

    public void SetBounce(float x, float y)
    {
        this.x = x;
        this.y = y;
    }
}
