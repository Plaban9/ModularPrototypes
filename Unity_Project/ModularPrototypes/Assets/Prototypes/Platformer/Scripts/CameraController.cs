using UnityEngine;
using UnityEngine.InputSystem;

namespace ModularPrototypes.Platformer.CameraUnit
{
    [RequireComponent(typeof(Camera))]
    public class CameraController : MonoBehaviour
    {
        [Header("Input")]
        public Key toggleProjectionKey = Key.P;
        public Key orbitModifierKey = Key.None;
        public Key resetCameraKey = Key.R;

        [Header("Sensitivity")]
        public float scrollSensitivity = 40f;
        public float orbitSensitivity = 600f;
        public float sensitivityMultiplier = 1f;

        [Header("Zoom Limits")]
        public float minOrthoSize = 1f;
        public float maxOrthoSize = 50f;
        public float minDistance = 1f;
        public float maxDistance = 100f;

        [Header("Orbit")]
        public Vector3 orbitPivot = Vector3.zero;
        public float minPitch = -80f;
        public float maxPitch = 80f;

        [Header("Smoothing")]
        public float smoothTime = 0.06f;

        Camera cam;
        Vector3 velocityPos;
        float velocityOrtho;
        float yaw;
        float pitch;
        float currentDistance;
        bool isOrbiting;

        // Stored defaults
        Vector3 defaultPos;
        Quaternion defaultRot;
        float defaultOrthoSize;
        float defaultDistance;
        float defaultScrollSens;
        float defaultOrbitSens;
        float defaultMultiplier;

        void Awake()
        {
            cam = GetComponent<Camera>();

            // Save defaults
            defaultPos = transform.position;
            defaultRot = transform.rotation;
            defaultOrthoSize = cam.orthographicSize;
            defaultDistance = Vector3.Distance(transform.position, orbitPivot);
            defaultScrollSens = scrollSensitivity;
            defaultOrbitSens = orbitSensitivity;
            defaultMultiplier = sensitivityMultiplier;

            // Initialize orbit state
            currentDistance = defaultDistance;
            Vector3 e = transform.eulerAngles;
            yaw = e.y;
            pitch = e.x;
        }

        public void ChangeCameraType(bool isPerspective)
        {
            //if (Keyboard.current[toggleProjectionKey].wasPressedThisFrame)
            cam.orthographic = !isPerspective;
        }

        void Update()
        {
            if (Keyboard.current[resetCameraKey].wasPressedThisFrame)
                ResetCamera();

            if (Mouse.current == null)
                return;

            // Zoom
            float scrollY = Mouse.current.scroll.ReadValue().y;
            if (Mathf.Abs(scrollY) > 0.001f)
                HandleZoom(scrollY, Mouse.current.position.ReadValue());

            // Orbit
            bool left = Mouse.current.leftButton.isPressed;
            bool mod = orbitModifierKey == Key.None || Keyboard.current[orbitModifierKey].isPressed;
            bool shouldOrbit = left && mod;

            if (shouldOrbit && !isOrbiting)
            {
                isOrbiting = true;
                currentDistance = Vector3.Distance(transform.position, orbitPivot);
                Vector3 e = transform.eulerAngles;
                yaw = e.y;
                pitch = e.x;
            }
            else if (!shouldOrbit)
            {
                isOrbiting = false;
            }

            if (isOrbiting)
                HandleOrbit(Mouse.current.delta.ReadValue());
        }

        // ---------------- RESET CAMERA ----------------
        public void ResetCamera()
        {
            transform.position = defaultPos;
            transform.rotation = defaultRot;

            cam.orthographicSize = defaultOrthoSize;
            currentDistance = defaultDistance;

            scrollSensitivity = defaultScrollSens;
            orbitSensitivity = defaultOrbitSens;
            sensitivityMultiplier = defaultMultiplier;

            Vector3 e = transform.eulerAngles;
            yaw = e.y;
            pitch = e.x;
        }

        // ---------------- ZOOM ----------------
        void HandleZoom(float scrollDelta, Vector2 mousePos)
        {
            float delta = scrollDelta * scrollSensitivity * sensitivityMultiplier * Time.deltaTime;

            if (cam.orthographic)
                ZoomOrtho(delta, mousePos);
            else
                ZoomPerspective(delta, mousePos);
        }

        void ZoomOrtho(float delta, Vector2 mousePos)
        {
            float focalDist = Vector3.Distance(transform.position, orbitPivot);
            Vector3 before = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, focalDist));

            float target = Mathf.Clamp(cam.orthographicSize - delta, minOrthoSize, maxOrthoSize);
            cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, target, ref velocityOrtho, smoothTime);

            Vector3 after = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, focalDist));
            transform.position += before - after;

            currentDistance = Vector3.Distance(transform.position, orbitPivot);
        }

        void ZoomPerspective(float delta, Vector2 mousePos)
        {
            Ray ray = cam.ScreenPointToRay(mousePos);
            Vector3 focal = ray.GetPoint(currentDistance);

            Vector3 dir = (focal - transform.position).normalized;
            Vector3 desired = transform.position + dir * delta;

            float dist = Vector3.Distance(desired, orbitPivot);
            dist = Mathf.Clamp(dist, minDistance, maxDistance);

            desired = orbitPivot - (transform.forward * dist);

            transform.position = Vector3.SmoothDamp(transform.position, desired, ref velocityPos, smoothTime);
            currentDistance = dist;
        }

        // ---------------- ORBIT ----------------
        void HandleOrbit(Vector2 delta)
        {
            float sens = orbitSensitivity * sensitivityMultiplier;

            yaw += (delta.x / Screen.width) * sens * Time.deltaTime;
            pitch -= (delta.y / Screen.height) * sens * Time.deltaTime;
            pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

            Quaternion rot = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 offset = rot * Vector3.back * currentDistance;

            transform.rotation = rot;
            transform.position = Vector3.SmoothDamp(transform.position, orbitPivot + offset, ref velocityPos, smoothTime);
        }
    }
}