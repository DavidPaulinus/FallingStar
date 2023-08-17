using UnityEngine;

public class Wall : MonoBehaviour
{
    private bool wall;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            wall = true;

        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 6)
        {
            wall = false;

        }
    }

    public bool OnWall()
    {
        return wall;
    }
}
