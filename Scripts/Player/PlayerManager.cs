using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private MovementPlayerState moveState;
    private StateManager stateManager;
    private PlayerStatus status;
    private InputManager input;
    private SpriteRenderer spr;
    private GhostTrail ghost;
    private Color alpha;

    [SerializeField] private DamageState damageState;
    [SerializeField] private float IframesDuration;
    private float IframesDureationCounter;
    private bool hit;

    [HideInInspector] public float cooldownDashCounter;
    private float MpMultiplier = 1;

    private void Awake()
    {
        instance = this;

        ghost = GetComponent<GhostTrail>();
        spr = GetComponent<SpriteRenderer>();
        input = GetComponent<InputManager>();
        status = GetComponent<PlayerStatus>();
        stateManager = GetComponent<StateManager>();
        moveState = GetComponent<MovementPlayerState>();

        IframesDureationCounter = IframesDuration;
    }

    private void Update()
    {
        if (input.RetriveXValue() != 0)
        {
            var _direction = input.RetriveXValue() < 0 ? 180 : 0;
            transform.rotation = new Quaternion(0, _direction, 0, 0);
        }
        if (hit)
        {
            alpha = spr.color;
            alpha.a = .5f;
            IFrames();
        }
        if (status.HP <= 0)
        {
            //TO DO
        }

        cooldownDashCounter -= Time.deltaTime;
    }

    public float FacingDirection()
    {
        return transform.rotation.y == 1 ? -1 : 1;
    }
    private void IFrames()
    {
        Physics2D.IgnoreLayerCollision(9, 8, true);
        if (IframesDureationCounter <= 0)
        {
            alpha.a = 1f;
            Physics2D.IgnoreLayerCollision(9, 8, false);
            hit = false;
        }
        spr.color = alpha;
        IframesDureationCounter -= Time.deltaTime;
    }

    public void RestoreMP(float amount)
    {
        MpMultiplier += amount;
        status.RestoreMP(2 * MpMultiplier);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 8)
        {
            if (!hit)
            {
                ghost.MakeGhost(false);
                ghost.ResetColors();

                moveState.ResetBonusSp();
                moveState.SetDeafultDashCooldown();

                ComboPointsImages.instance.RemoveImage();

                IframesDureationCounter = IframesDuration;
                hit = true;

                status.Damage(2);

                stateManager.SwitchToNextState(damageState);
            }
        }
    }
}
