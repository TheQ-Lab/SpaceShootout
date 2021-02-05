using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance;

    private bool isFollowing;
    private Transform camTarget;
    //Transform camTrans;
    private Camera mainCamera;
    public Vector3 stdCameraPosition = new Vector3(0, 0, -10);
    private float smoothSpeed = 0.125f;
    private Animator cameraAnimator;

    private void Awake()
    {
        if (Instance != this)
            Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponent<Camera>();
        cameraAnimator = GetComponent<Animator>();
        stdCameraPosition = transform.position;
    }


    private void FixedUpdate()
    {
        if (isFollowing && camTarget != null)
            MoveCamera();
    }

    public void EnableTargetNZoom(Transform t)
    {
        camTarget = t;
        isFollowing = true;
        cameraAnimator.SetBool("Zoomed In", true);
    }

    public void EnableTarget(Transform t)
    {
        camTarget = t;
        isFollowing = true;
    }

    public void EnableZoom()
    {
        cameraAnimator.SetBool("Zoomed In", true);
    }

    public void DisableTarget()
    {
        camTarget = null;
        isFollowing = false;
    }

    public void DisableZoom()
    {
        cameraAnimator.SetBool("Zoomed In", false);
    }

    public void ResetPos()
    {
        transform.position = stdCameraPosition;
    }

    void MoveCamera()
    {
        Vector3 desiredPosition = camTarget.position;
        desiredPosition.z = stdCameraPosition.z;
        Vector3 smoothPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = smoothPosition;
        //cameraAnimator.SetBool("Zoomed In", true);
    }
}
