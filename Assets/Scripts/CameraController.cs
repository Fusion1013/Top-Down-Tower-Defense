using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float moveSpeed;
    public float posSmoothing;
    public float zoomSpeedFactor;
    public float scrollSpeed;
    public float zoomSmoothing;
    public float minZoom;
    public float maxZoom;
    
    Camera viewCamera;
    Vector3 velocity;

    private Vector3 targetPos;
    private Vector3 actualPos;

    private float targetZoom;
    private float actualZoom;

    void Start()
    {
        targetPos = transform.position;
        actualPos = targetPos;

        viewCamera = Camera.main;
        targetZoom = viewCamera.orthographicSize;
        actualZoom = targetZoom;
    }
    
    void Update()
    {
        // Camera Movement
        velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * moveSpeed * (Input.GetAxis("SpeedUp") + 1);
        velocity *= actualZoom * zoomSpeedFactor;

        targetPos += velocity * Time.deltaTime;
        actualPos = Vector3.Lerp(actualPos, targetPos, posSmoothing);

        transform.position = actualPos;

        // Camera Zoom
        targetZoom = targetZoom - Input.mouseScrollDelta.y * scrollSpeed;
        targetZoom = Mathf.Min(Mathf.Max(minZoom, targetZoom), maxZoom);

        actualZoom = Mathf.Lerp(actualZoom, targetZoom, zoomSmoothing);
        viewCamera.orthographicSize = actualZoom;
    }
}
