using UnityEngine;

public class WallColliderLine : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private LayerMask layer;

    public bool OnWall(int facingDirection)
    {
        return Physics2D.Linecast(transform.position, new Vector2(transform.position.x + distance * facingDirection, transform.position.y), layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + distance, transform.position.y));
    }
}
