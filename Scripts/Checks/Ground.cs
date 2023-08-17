using UnityEngine;

public class Ground : MonoBehaviour
{
    [SerializeField] private Collider2D collision;
    [SerializeField] private LayerMask layer;

    public bool Grounded()
    {
        return Physics2D.BoxCast(collision.bounds.center, collision.bounds.size, 0, Vector2.down, .1f,layer);
    }
}
