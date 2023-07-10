using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private PlayerInputController inputInfo;
    private Transform playerCamera;

    //Camera will look at this position instead of center of the character
    [SerializeField] private Transform LookAtPosition;

    //Mouse Sensitivity
    [Range(0.1f, 1.0f), SerializeField, Header("Mouse Sensitivity")] public float mouseSensitivity;

    //Rotate Smooth
    private Vector3 rotationSmoothVelocity = Vector3.zero;
    [Range(0.1f, 0.5f), SerializeField, Header("Smooth Time")] public float rotationSmoothTime = 0.12f;
    //Transform Smooth Time
    [SerializeField] private float TransformLerpFactor;

    //Camera Distance
    [SerializeField, Header("Distance")] private float cameraDistance;
    //Camera Pitch Range
    [SerializeField, Header("PitchRange")] private Vector2 PitchRange = new Vector2(-85f, 70f);
    

    //Lock On
    [SerializeField, Header("Lock On")] private bool isLockOn;
    [SerializeField] private Transform LockTarget;

    //collision
    [SerializeField, Header("Collision")] private Vector2 collisionDistanceRange = new Vector2(0.01f, 3f);
    [SerializeField] private float colliderMotionLerpTime;
    [SerializeField] private float occlusionScale;
    private float _cameraDistance;
    private Vector3 _camDirection;
    

    //��ǰ��תŷ����
    private Vector3 currentRotationValue;
    //ƫ����
    private float yaw;
    //������
    private float pitch;

    //�ü���Mask
    public LayerMask collisionLayer;

    #region Unity�¼�����
    private void Awake()
    {
        playerCamera = Camera.main.transform;
        inputInfo = LookAtPosition.transform.root.GetComponent<PlayerInputController>();
    }

    void Start()
    {
        _camDirection = transform.localPosition.normalized;
        _cameraDistance = collisionDistanceRange.y;
    }

    void Update()
    {
        cursorController();
        getCameraInput();
    }

    private void LateUpdate()
    {
        cameraMovement();
        cameraOcclusion(playerCamera);
    }

    private void OnDrawGizmos()
    {
        
    }

    #endregion


    
    #region �ڲ�����
    /// <summary>
    /// �������
    /// </summary>
    private void cursorController()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    /// <summary>
    /// ��ȡ������� ����ƫ������
    /// </summary>
    private void getCameraInput()
    {
        if(isLockOn)
        {
            return;
        }

        yaw += inputInfo.CameraLook.x * mouseSensitivity;
        pitch = Mathf.Clamp(pitch - inputInfo.CameraLook.y * mouseSensitivity, PitchRange.x, PitchRange.y);

    }

    
    /// <summary>
    /// �ƶ����
    /// </summary>
    private void cameraMovement()
    {
        if(isLockOn == false)
        {
            //��ת
            currentRotationValue = Vector3.SmoothDamp(currentRotationValue, new Vector3(pitch, yaw, 0f), ref rotationSmoothVelocity, rotationSmoothTime);
            transform.eulerAngles = currentRotationValue;
        }

        //λ��
        Vector3 TargetPos = LookAtPosition.position - transform.forward * cameraDistance;
        transform.position = Vector3.Lerp(transform.position, TargetPos, TransformLerpFactor * Time.deltaTime);
    }

    private void cameraOcclusion(Transform camera)
    {
        Vector3 expectedCameraPosition = transform.TransformPoint(_camDirection * 3f);
        if (Physics.Linecast(transform.position, expectedCameraPosition, out var hit, collisionLayer))
        {
            _cameraDistance = Mathf.Clamp(hit.distance * .9f, collisionDistanceRange.x, collisionDistanceRange.y);
        }
        else
        {
            _cameraDistance = collisionDistanceRange.y;
        }
        camera.transform.localPosition = Vector3.Lerp(camera.transform.localPosition, _camDirection * (_cameraDistance - 0.1f), colliderMotionLerpTime * Time.deltaTime);
    }
    


    #endregion

    #region �ⲿ����

    public void setLookAtPosition(Transform position)
    {
        LookAtPosition = position;
    }




    #endregion
}
