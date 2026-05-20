using UnityEngine;

public class GroundDetection : MonoBehaviour
{
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float checkRadius = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    public delegate void OnGroundChangedDelegate(bool isOnGround);
    public event OnGroundChangedDelegate OnGroundChanged;

    private bool isOnGround;

    void Update()
    {
        CheckGround();
    }

    private void CheckGround()
    {
        bool wasOnGround = isOnGround;
        isOnGround = Physics2D.OverlapCircle(groundCheckPoint.position, checkRadius, groundLayer);

        if (wasOnGround != isOnGround)
        {
            OnGroundChanged?.Invoke(isOnGround);
        }
    }

    // Для визуализации радиуса проверки в редакторе
    private void OnDrawGizmosSelected()
    {
        if (groundCheckPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheckPoint.position, checkRadius);
        }
    }
}