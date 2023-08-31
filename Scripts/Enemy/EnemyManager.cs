using UnityEngine;

public class EnemyManager : Damageable
{
    [HideInInspector] public float hp;
    [SerializeField] public float MaxHp;
    [SerializeField] private DamageEnemyState damageState;
    [SerializeField] private float moveXDamage;
    [SerializeField] private float moveYDamage;

    private StateManager stateManager;

    private void Awake()
    {
        hp = MaxHp;

        stateManager = GetComponent<StateManager>();
    }

    private void Update()
    {
        if (hp <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    public override void DoDamage(int damage)
    {
        hp -= damage;

        damageState.SetBounce(moveXDamage,moveYDamage);
        stateManager.SwitchToNextState(damageState);
    }
}
