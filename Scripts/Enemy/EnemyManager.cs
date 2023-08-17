using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private int hp;
    [SerializeField] private DamageEnemyState damageState;
    [SerializeField] private float moveXDamage;
    [SerializeField] private float moveYDamage;

    private StateManager stateManager;

    private void Awake()
    {
        stateManager = GetComponent<StateManager>();
    }

    private void Update()
    {
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public void DoDamage(int damage)
    {
        hp -= damage;

        damageState.SetBounce(moveXDamage,moveYDamage);
        stateManager.SwitchToNextState(damageState);
    }
}
