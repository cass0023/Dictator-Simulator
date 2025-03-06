using UnityEngine;
using Cinemachine;

public class ParallaxMenu : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public float offsetMultiplier = 5f;  // Increase for more effect
    public float smoothTime = 0.3f;
    public Vector3 velocity = Vector3.zero;

    private Vector3 startPosition;
    private CinemachineTransposer transposer;

    void Start()
    {
        if (virtualCamera == null)
        {
            Debug.LogError("CinemachineVirtualCamera is not assigned!", this);
            return;
        }

        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();

        if (transposer == null)
        {
            Debug.LogError("CinemachineTransposer not found! Set 'Body' to 'Transposer' in the Inspector.", this);
            return;
        }

        startPosition = transposer.m_FollowOffset;
    }

    void Update()
    {
        if (transposer == null) return;

        Vector2 offset = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        Vector3 targetOffset = startPosition + new Vector3(offset.x, offset.y, 0) * offsetMultiplier;

        // Use Lerp instead of SmoothDamp for real-time parallax
        transposer.m_FollowOffset = Vector3.SmoothDamp(transposer.m_FollowOffset, targetOffset, ref velocity, smoothTime);

        Debug.Log("Offset: " + transposer.m_FollowOffset); // Debug to see if it's changing
    }
}