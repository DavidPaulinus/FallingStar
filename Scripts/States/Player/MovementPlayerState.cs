using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class MovementPlayerState : State
{
    private InputManager input;
    private Rigidbody2D body;
    private DirtyEffect dirt;
    private Ground ground;

    [Space(20)]
    [Header("MOVEMENT CONFIG")]
    [SerializeField] private float moveSpGround;
    [SerializeField] private float moveSpAir;

    private float x;
    private float bonusSp;

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
    [SerializeField] private float cooldownDashTimeNoHit;
    [SerializeField] private float cooldownDashTimeHit;

    private float cooldownDashTime;
    private bool dash;

    private void Awake()
    {
        ground = GetComponent<Ground>();
        body = GetComponent<Rigidbody2D>();
        input = GetComponent<InputManager>();

        dirt = GetComponentInChildren<DirtyEffect>();
        wallCollider = GetComponentInChildren<Wall>();
        circleCollider = GetComponentInChildren<CircleCollider2D>();

        cooldownDashTime = cooldownDashTimeNoHit;
    }

    public override State RunCurrentStateFixedUpdate()
    {
        var _ground = ground.Grounded();

        //jump dirt effect
        dirt.StopParticles();

        #region MOVEMENT
        var _move = _ground ? moveSpGround : moveSpAir;
        body.velocity = new Vector2((_move + bonusSp) * x, body.velocity.y);
        #endregion

        #region WALL JUMP
        if (wallCollider.OnWall() && !_ground)
        {
            airJumpCounter = 0;

            gravity = 0f;
            velocity = Vector2.zero;

            timeToSlideDownCounter = timeToSlideDown;
            if (timeToSlideDownCounter <= 0 || input.RetriveYValue() < 0)
            {
                gravity = .85f;
                velocity = body.velocity;
            }
            timeToSlideDownCounter --;

            if (jump)
            {
                var _direction = transform.rotation.y == 1 ? 1 : -1;
                if (-_direction == input.RetriveXValue())
                {
                    velocity = new Vector2(wallClimb.x * _direction, wallClimb.y);
                    jump = false;
                }
                else if (input.RetriveXValue() == 0)
                {
                    velocity = new Vector2(wallBounce.x * _direction, wallBounce.y);
                    jump = false;
                }
            }
            if (wallStickCounter > 0)
            {
                if (input.RetriveXValue() != 0)
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
            //jump dirt effect
            if (_ground) dirt.PlayParticles(transform, PlayerManager.instance.FacingDirection());

            jump = false;
            bufferCounter = bufferTime;
        }
        else if (!jump && bufferCounter > 0) bufferCounter--;


        //jump
        if (bufferCounter > 0)
        {
            JumpAction();
        }

        if (input.RetriveHoldingJump() && velo.y > .1f) body.gravityScale = upwardGravityValue;
        else if ((!input.RetriveHoldingJump() || velo.y < .1f) && gravityCoolDownTime < 0)
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

            var _x = input.RetriveXValue();
            var _y = input.RetriveYValue();

            var _direction = _x == 0 && _y == 0 ? PlayerManager.instance.FacingDirection() : _x;

            dashState.SetDashData(_direction, _y);

            return dashState;
        }

        return this;
    }

    #region INPUTS
    private void Dash()
    {
        if (input.RetriveDash() && PlayerManager.instance.cooldownDashCounter < 0)
        {
            PlayerManager.instance.cooldownDashCounter = cooldownDashTime;
            dash = true;

        }else if (input.RetriveDash() && PlayerManager.instance.cooldownDashCounter > 0) dash = false;
    }

    private void Jump()
    {
        jump |= input.RetriveJump();
    }

    private void Move()
    {
        x = input.RetriveXValue();
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

    public void IncreaseBonusSp(float amount)
    {
        bonusSp += amount;
    }
    public void ResetBonusSp()
    {
        bonusSp = 0;
    }
    public void SetQuickDashCooldown()
    {
        cooldownDashTime = cooldownDashTimeHit;
    }
    public void SetDeafultDashCooldown()
    {
        cooldownDashTime = cooldownDashTimeNoHit;
    }
    public void SetGravityCooldownTime(float time)
    {
        gravityCoolDownTime = time;
    }
}