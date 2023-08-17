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
        var _direction = transform.position.x > PlayerManager.instance.transform.position.x ? 1 : 1;

        body.AddForce(new Vector2(x * _direction, y), ForceMode2D.Impulse);
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
            if (transform.position.x > PlayerManager.instance.transform.position.x) transform.rotation = new Quaternion(0, 0, 0, 0);
            else transform.rotation = new Quaternion(0, 180, 0, 0);

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
