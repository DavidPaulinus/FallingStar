using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance;

    private StateManager stateManager;
    private SpriteRenderer spr;

    [SerializeField] private DamageState damageState;
    [SerializeField] private float IframesDuration;
    private float IframesDureationCounter;
    private Color alpha;
    private bool hit;

    private void Awake()
    {
        instance = this;

        spr = GetComponent<SpriteRenderer>();
        stateManager = GetComponent<StateManager>();

        IframesDureationCounter = IframesDuration;
    }

    private void Update()
    {
        if (InputManager.instance.RetriveXValue() != 0)
        {
            var _direction = InputManager.instance.RetriveXValue() < 0 ? 180 : 0;
            transform.rotation = new Quaternion(0, _direction, 0, 0);
        }
        if (hit)
        {
            alpha = spr.color;
            alpha.a = .5f;
            IFrames();
        }
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (!hit)
            {
                IframesDureationCounter = IframesDuration;
                hit = true;
                stateManager.SwitchToNextState(damageState);
            }
        }
    }

}
