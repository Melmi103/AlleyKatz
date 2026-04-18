using UnityEngine;

public class Camerascript : MonoBehaviour
{
    [SerializeField] private Transform target;                      // assign in Inspector or auto-find by tag "Player"
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float smoothTime = 0.12f;              // lower = tighter follow
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = true;

    // Optional bounds (enable by setting useBounds = true)
    [SerializeField] private bool useBounds = false;
    [SerializeField] private float minX, maxX, minY, maxY;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        // If no target assigned, try to find a GameObject tagged "Player"
        if (target == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }

        // Ensure camera stays behind the scene on Z axis for 2D
        if (offset.z == 0f) offset.z = -10f;
    }

    // Use LateUpdate so camera follows after the player's movement for the frame
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desired = target.position + offset;

        // Respect follow toggles
        if (!followX) desired.x = transform.position.x;
        if (!followY) desired.y = transform.position.y;

        // Optional bounds
        if (useBounds)
        {
            desired.x = Mathf.Clamp(desired.x, minX, maxX);
            desired.y = Mathf.Clamp(desired.y, minY, maxY);
        }

        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }

    // Optional runtime setter
    public void SetTarget(Transform newTarget) => target = newTarget;
}
