using UnityEngine;

public class AttackManager : MonoBehaviour
{
    private StateManager stateManager;

    [SerializeField] private int damage;
    [SerializeField] private float increaseDashTimeWhenHitEnemy;
    [SerializeField] private float stopTimeTime;

    private float stopTimeTimeCounter;
    private bool hit = false;

    private void Awake()
    {
        stateManager = GetComponentInParent<StateManager>();
    }

    private void Update()
    {
        if (stopTimeTimeCounter <= 0)
        {
            Time.timeScale = 1f;
        }
        stopTimeTimeCounter -= Time.unscaledDeltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (!hit)
            {
                if (StateToString.ConvertState(stateManager.GetCurrentState().ToString()).Equals("Dash"))
                {
                    DashState dash = (DashState)stateManager.GetCurrentState();
                    dash.IncreaseDashTime(increaseDashTimeWhenHitEnemy);
                }

                stopTimeTimeCounter = stopTimeTime;

                CameraManager.Instance.Shake();
                Time.timeScale = 0;

                hit = true;
                collision.GetComponent<EnemyManager>().DoDamage(2);
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy")) hit = false;
    }
}
