using Unity.Mathematics;
using UnityEngine;

public class CamManager : MonoBehaviour
{
    public static Camera mainCam;
    public static float camHeight;
    public static float camWidth ;

    public Transform targetToFollow;
    public float smoothSpeed = 0.125f;

    void Awake()
    {
        mainCam = Camera.main;
        camHeight = mainCam.orthographicSize * 2;
        camWidth = camHeight * mainCam.aspect;
    }

    private void FixedUpdate()
    {
        var newPos = Vector2.Lerp(transform.position, targetToFollow.position, smoothSpeed);

        transform.position = new Vector3(
            x: math.max(0f, newPos.x),
            y: 0,
            z: -10
        );
    }
}