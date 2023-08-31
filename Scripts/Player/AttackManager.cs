using System;
using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private MovementPlayerState movePlayer;
    private StateManager stateManager;
    private GhostTrail ghost;

    [Header("ATTACK CONFIG")]
    [SerializeField] private int damage;
    [SerializeField] private float stopTimeTime;
    [SerializeField] private float increaseDashTimeWhenHitEnemy;

    private float stopTimeTimeCounter;
    private bool hit = false;

    [Space(20)]
    [Header("GHOST CONFIG")]
    [SerializeField] private float timeToStopGhost;
    [SerializeField] private float decreaseGhostDelay;
    [SerializeField] private int comboNumberToChangeColor;

    [Space(20)]
    [Header("BONUS CONFIG")]
    [SerializeField] private float bonusSpWhenHit;
    [SerializeField] private float bonusMpHitMultiplier;
    [SerializeField] private float bonusGhostTimeHitMultiplier;

    private int hitCounter;
    private int hitNumberHolder;
    private float timeToStopGhostCounter;
    private float bonusGhostTimeHitMultiplierHolder = 0;

    private void Awake()
    {
        ghost = GetComponentInParent<GhostTrail>();
        stateManager = GetComponentInParent<StateManager>();
        movePlayer = GetComponentInParent<MovementPlayerState>();
    }

    private void Update()
    {
        //reset time after hit enemy
        if (stopTimeTimeCounter <= 0)
        {
            Time.timeScale = 1f;
        }
        stopTimeTimeCounter -= Time.unscaledDeltaTime;
        //change ghost and movement bonus every certain number
        if (hitCounter != 0 && hitCounter % comboNumberToChangeColor == 0 && hitCounter != hitNumberHolder)
        {
            hitNumberHolder = hitCounter;

            bonusGhostTimeHitMultiplierHolder += bonusGhostTimeHitMultiplier;

            movePlayer.IncreaseBonusSp(bonusSpWhenHit);
            PlayerManager.instance.RestoreMP(bonusMpHitMultiplier);

            ghost.DecreaseGhostDelay(decreaseGhostDelay);
            ghost.NextColor();
        }
        //stop ghost and reset everything
        if (timeToStopGhostCounter <= 0)
        {
            movePlayer.ResetBonusSp();
            movePlayer.SetDeafultDashCooldown();

            ComboPointsImages.instance.RemoveImage();

            GhostConfig(false, 0);
            ghost.ResetColors();

            hitCounter = 0;
            bonusGhostTimeHitMultiplierHolder = 0;
        }
        timeToStopGhostCounter -= Time.deltaTime;
    }

    private void GhostConfig(bool make, float time)
    {
        ghost.MakeGhost(make);
        timeToStopGhostCounter = time;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Damageable"))
        {
            if (!hit)
            {
                DashState dash = (DashState)stateManager.GetCurrentState();
                dash.IncreaseDashTime(increaseDashTimeWhenHitEnemy);

                movePlayer.SetQuickDashCooldown();

                GhostConfig(true, timeToStopGhost + bonusGhostTimeHitMultiplierHolder);
                hitCounter++;

                if (hitCounter >= comboNumberToChangeColor) ComboPointsImages.instance.SetImage(hitCounter);

                stopTimeTimeCounter = stopTimeTime;

                CameraManager.Instance.Shake();
                Time.timeScale = 0;

                hit = true;
                collision.GetComponent<Damageable>().DoDamage(2);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Damageable")) hit = false;
    }
}
