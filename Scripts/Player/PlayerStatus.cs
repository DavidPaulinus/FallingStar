using UnityEngine;

public class PlayerStatus : MonoBehaviour
{
    [SerializeField] public int HP;
    [SerializeField] public int MaxHP;

    [SerializeField] public float MP;
    [SerializeField] public int MaxMP;

    public void Heal(int heal)
    {
        HP += heal;
        if (HP > MaxHP)
        {
            HP = MaxHP;
        }
    }
    public void RestoreMP(float amount)
    {
        MP += amount;
        if(MP > MaxMP) MP = MaxMP;
    }

    public void Damage(int amount)
    {
        HP -= amount;
    }

    public void IncreaseMaxHP(int amount)
    {
        MaxHP += amount;
    }
}
