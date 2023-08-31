using UnityEngine;

public class PatrolState : State
{
    [Header("NEXT STATE")]
    [SerializeField] private State nextState;
    [SerializeField] private Metodo metodo;

    [Space(10)]
    [Header("MOVEMENT")]
    [SerializeField] private float moveSp;
    [SerializeField, Range(-1,1)] private int facingDirection;

    [Header("checks")]
    [SerializeField] private WallColliderLine wall;
    [SerializeField] private GroundColliderLine ground;


    private Rigidbody2D body;

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    public override State RunCurrentStateFixedUpdate()
    {
        body.velocity = new Vector2(moveSp * facingDirection, body.velocity.y);

        return this;
    }

    public override State RunCurrentStateLateUpdate()
    {
        return this;
    }

    public override State RunCurrentStateUpdate()
    {
        if (wall.OnWall(facingDirection) || !ground.OnGround())
        {
            facingDirection = -facingDirection;
            if (facingDirection < 0)
            {
                transform.rotation = new Quaternion(0,180,0,0);
            }else transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        if (metodo != null && metodo.RunMethod())
        {
            return nextState;
        }

        return this;
    }
}
