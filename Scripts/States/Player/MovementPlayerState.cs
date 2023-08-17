using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class MovementPlayerState : State
{
    private Rigidbody2D body;
    private Ground ground;

    [SerializeField] private AttackState attackState;

    [Space(20)]
    [Header("MOVEMENT CONFIG")]
    [SerializeField] private float moveSpGround;
    [SerializeField] private float moveSpAir;

    private float x;

    [Space(20)]
    [Header("JUMP CONFIG")]
    [SerializeField] private float jumpHeight;
    [SerializeField] private float deafultGravityValue = 1f;
    [SerializeField] private float upwardGravityValue = 1.7f;
    [SerializeField] private float downwardGravityValue = 4.5f;

    private float gravityCoolDownTime;
    private bool jump;
    private Vector2 velo;

    [Header("features")]
    [SerializeField] private float coyoteTime = .3f;
    [SerializeField] private float bufferTime = .3f;
    [SerializeField] private int airJump;

    private float coyoteCounter;
    private float bufferCounter;
    private int airJumpCounter;
    private bool isJumping;

    [Space(20)]
    [Header("WALL CONFIG")]
    [SerializeField] private float wallStickTime;
    private float wallStickCounter;
    private float gravity;
    private CircleCollider2D circleCollider;
    private Wall wallCollider;

    private Vector2 velocity;

    [Header("Wall Slide")]
    [SerializeField] private float timeToSlideDown;
    private float timeToSlideDownCounter;

    [Header("Wall climp")]
    [SerializeField] private Vector2 wallClimb = new Vector2(4f, 12f);
    [SerializeField] private Vector2 wallBounce = new Vector2(10.7f, 10f);

    [Space(20)]
    [Header("DASH CONFIG")]
    [SerializeField] private DashState dashState;
    [SerializeField] private float cooldownDashTime;

    private float cooldownDashCounter;
    private bool dash;

    private void Awake()
    {
        ground = GetComponent<Ground>();
        body = GetComponent<Rigidbody2D>();

        wallCollider = GetComponentInChildren<Wall>();
        circleCollider = GetComponentInChildren<CircleCollider2D>();
    }

    public override State RunCurrentStateFixedUpdate()
    {
        var _ground = ground.Grounded();

        #region MOVEMENT
        var _move = _ground ? moveSpGround : moveSpAir;
        body.velocity = new Vector2(_move * x, body.velocity.y);
        #endregion

        #region WALL JUMP
        if (wallCollider.OnWall() && !_ground)
        {
            airJumpCounter = 0;

            gravity = 0f;
            velocity = Vector2.zero;

            timeToSlideDownCounter = timeToSlideDown;
            if (timeToSlideDownCounter <= 0 || InputManager.instance.RetriveYValue() < 0)
            {
                gravity = .85f;
                velocity = body.velocity;
            }
            timeToSlideDownCounter --;

            if (jump)
            {
                var _direction = transform.rotation.y == 1 ? 1 : -1;
                if (-_direction == InputManager.instance.RetriveXValue())
                {
                    velocity = new Vector2(wallClimb.x * _direction, wallClimb.y);
                    jump = false;
                }
                else if (InputManager.instance.RetriveXValue() == 0)
                {
                    velocity = new Vector2(wallBounce.x * _direction, wallBounce.y);
                    jump = false;
                }
            }
            if (wallStickCounter > 0)
            {
                if (InputManager.instance.RetriveXValue() != 0)
                {
                    wallStickCounter--;
                }
            }
            else wallStickCounter = wallStickTime;

            body.gravityScale = gravity;
            body.velocity = velocity;
        }
        #endregion

        #region JUMP
        velo = body.velocity;

        //coyote
        if (_ground && velo.y == 0f)
        {
            circleCollider.enabled = false;
            timeToSlideDownCounter = timeToSlideDown;
            wallStickCounter = wallStickTime;

            coyoteCounter = coyoteTime;
            airJumpCounter = 0;

            isJumping = false;
        }
        else coyoteCounter--;

        //buffer
        if (jump)
        {
            jump = false;
            bufferCounter = bufferTime;
        }
        else if (!jump && bufferCounter > 0) bufferCounter--;


        //jump
        if (bufferCounter > 0)
        {
            JumpAction();
        }

        if (InputManager.instance.RetriveHoldingJump() && velo.y > .1f) body.gravityScale = upwardGravityValue;
        else if ((!InputManager.instance.RetriveHoldingJump() || velo.y < .1f) && gravityCoolDownTime < 0)
        {
            circleCollider.enabled = true;
            body.gravityScale = downwardGravityValue;
        }
        else if (gravityCoolDownTime > 0)
        {
            circleCollider.enabled = true;
            body.gravityScale = upwardGravityValue;
        }
        else if (velo.y == 0) body.gravityScale = deafultGravityValue;

        gravityCoolDownTime--;

        body.velocity = velo;
        #endregion

        return this;
    }
    public override State RunCurrentStateLateUpdate()
    {
        return this;
    }
    public override State RunCurrentStateUpdate()
    {
        Move();
        Jump();
        Dash();

        if (dash)
        {
            dash = false;

            var _x = InputManager.instance.RetriveXValue();
            var _y = InputManager.instance.RetriveYValue();

            var _direction = _x == 0 && _y == 0 ? PlayerManager.instance.FacingDirection() : _x;

            dashState.SetDashData(_direction, _y);

            return dashState;
        }
        if (InputManager.instance.RetriveAttack())
        {
            return attackState;
        }

        return this;
    }

    #region INPUTS
    private void Dash()
    {
        if (InputManager.instance.RetriveDash() && cooldownDashCounter < 0)
        {
            cooldownDashCounter = cooldownDashTime;
            dash = true;

        }else if (InputManager.instance.RetriveDash() && cooldownDashCounter > 0) dash = false;

        cooldownDashCounter -= Time.deltaTime;
    }

    private void Jump()
    {
        jump |= InputManager.instance.RetriveJump();
    }

    private void Move()
    {
        x = InputManager.instance.RetriveXValue();
    }

    #endregion

    private void JumpAction()
    {
        if (coyoteCounter > 0 ||(airJumpCounter < airJump && isJumping))
        {
            if (isJumping)
            {
                airJumpCounter++;
            }

            isJumping = true;
            coyoteCounter = 0;

            velo.y += jumpHeight;
        }
    }

    public void SetGravityCooldownTime(float time)
    {
        gravityCoolDownTime = time;
    }
    
}