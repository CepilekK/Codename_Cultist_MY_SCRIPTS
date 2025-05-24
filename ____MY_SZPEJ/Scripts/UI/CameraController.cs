using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private GameObject player; // Obiekt gracza, do którego pod¹¿a kamera
    [SerializeField] private float rotationSpeed = 100f; // Prêdkoœæ obrotu kamery wokó³ gracza
    [SerializeField] private float zoomSpeed = 5f; // Prêdkoœæ przybli¿ania/oddalania kamery
    [SerializeField] private int minZoom = 5; // Minimalna odleg³oœæ kamery od gracza
    [SerializeField] private int maxZoom = 20; // Maksymalna odleg³oœæ kamery od gracza
    [SerializeField] private float cameraHeight = 10f; // Pocz¹tkowa wysokoœæ kamery nad graczem
    [SerializeField] private float cameraAngle = 65f; // K¹t nachylenia kamery
    [SerializeField] private float smoothTime = 0.2f; // Czas wyg³adzania ruchu

    private Transform camTransform;
    private float currentZoom;
    private float targetRotation = 0f;
    private float rotationVelocity;
    private Vector3 velocity = Vector3.zero;

    private void Start()
    {
        camTransform = Camera.main.transform;
        currentZoom = (minZoom + maxZoom) / 2f;


        SetInitialCameraPosition();
    }

    private void Update()
    {
        if (player == null) return;

        HandleRotation();
        HandleZoom();
    }

    private void LateUpdate()
    {
        FollowPlayerSmooth();
    }

    private void HandleRotation()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        if (horizontalInput != 0)
        {
            targetRotation += horizontalInput * rotationSpeed * Time.deltaTime;
        }

        float smoothRotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref rotationVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(cameraAngle, smoothRotation, 0);
    }

    private void HandleZoom()
    {
        float scrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (scrollInput != 0)
        {
            currentZoom -= scrollInput * zoomSpeed;
            currentZoom = Mathf.Clamp(currentZoom, minZoom, maxZoom);
        }

        UpdateCameraPosition();
    }

    private void FollowPlayerSmooth()
    {
        Vector3 targetPosition = player.transform.position;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }

    private void SetInitialCameraPosition()
    {
        transform.rotation = Quaternion.Euler(cameraAngle, 0, 0);
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        Vector3 desiredPosition = new Vector3(0, cameraHeight, -currentZoom);
        camTransform.localPosition = Vector3.Lerp(camTransform.localPosition, desiredPosition, smoothTime);
        camTransform.LookAt(player.transform.position);
    }
}
