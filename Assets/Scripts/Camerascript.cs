using UnityEngine;
using UnityEngine.InputSystem;

public class Camerascript : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);
    [SerializeField] private float smoothTime = 0.12f;
    [SerializeField] private bool followX = true;
    [SerializeField] private bool followY = true;

    [SerializeField] private float minX, maxX, minY, maxY;

    [SerializeField, Range(0f, 1f)] private float mouseFollowStrength = 0f;

    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        if (target == null)
        {
            var player = GameObject.FindGameObjectWithTag("Player");
            Debug.Log("Player found!");
            if (player != null) target = player.transform;
        }

        if (Mathf.Approximately(offset.z, 0f))
            offset.z = -10f;
    }

    void LateUpdate()
    {
        if (target == null) return;

       
        Vector2 mouseScreen;
        if (Mouse.current != null)
        {
            mouseScreen = Mouse.current.position.ReadValue();
        }
        else
        {
            mouseScreen = Input.mousePosition;
        }

       
        Vector3 desired;
        Camera cam = Camera.main;
        if (cam != null)
        {
           
            float screenZ = cam.WorldToScreenPoint(target.position).z;
            Vector3 mouseWorld = cam.ScreenToWorldPoint(new Vector3(mouseScreen.x, mouseScreen.y, screenZ));

           
            Vector3 mouseInfluence = (mouseWorld - target.position) * mouseFollowStrength;
            desired = target.position + mouseInfluence;
        }
        else
        {
         
            desired = target.position + offset;
        }

        
        desired.z = target.position.z + offset.z;

        if (!followX) desired.x = transform.position.x;
        if (!followY) desired.y = transform.position.y;

        transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocity, smoothTime);
    }

    public void SetTarget(Transform newTarget) => target = newTarget;
}
