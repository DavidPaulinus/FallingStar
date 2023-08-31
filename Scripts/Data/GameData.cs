using UnityEngine;

[System.Serializable]
public class GameData
{
    public GameData()
    {
        maxHP = 20;

        HP = maxHP;
        playerPosition= new Vector2(0,0);
    }

    public float HP;
    public float maxHP;

    public string sceneName;
    public Vector2 playerPosition;
}
