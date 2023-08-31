using UnityEngine;

public class GroundColliderLine : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private LayerMask layer;

    public bool OnGround()
    {
        return Physics2D.Linecast(transform.position, new Vector2(transform.position.x, transform.position.y - distance), layer);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - distance));
    }
}
