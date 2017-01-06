using UnityEngine;
using System.Collections;

///[RequireComponent(typeof(CharacterMotor))]
public class ScaleController : MonoBehaviour {

    public KeyCode m_ScaleModeToggle = KeyCode.B;

    public KeyCode m_ScaleUp = KeyCode.J;
    public KeyCode m_ScaleDown = KeyCode.K;

    public float m_ScaleSpeed = 1f;

    public float m_MaxRaycastDistance = Mathf.Infinity;
    public LayerMask m_RaycastLayerMask = int.MaxValue;

    private readonly Vector3 p_ViewportMiddle = new Vector3(0.5f, 0.5f, 0f);

    private bool m_InScaleMode;
    private Transform m_ScalingObject;
    private Camera p_Camera;

    void Start () {
        var cameras = GetComponentsInChildren<Camera>();
        if (cameras.Length == 0) {
            Debug.LogWarning("No cameras found.");
            p_Camera = Camera.main;
        } else if (cameras.Length > 1) {
            Debug.LogWarning("More than one camera found.");
            p_Camera = Camera.main;
        } else {
            p_Camera = cameras[0];
        }
    }

    void Update () {
        var scaleModeToggleKey = Input.GetKeyDown(m_ScaleModeToggle);

        if (scaleModeToggleKey) {
            m_InScaleMode ^= true;
        }

        if (m_InScaleMode) {
            var fwd = Input.GetKey(m_ScaleUp);
            var bwd = Input.GetKey(m_ScaleDown);
            if (fwd ^ bwd) {
                var scaleSpeed = Vector3.one * m_ScaleSpeed * Time.deltaTime;

                var fwdDown = Input.GetKeyDown(m_ScaleUp);
                var bwdDown = Input.GetKeyDown(m_ScaleDown);

                if (!fwdDown && !bwdDown && m_ScalingObject != null) {
                    m_ScalingObject.localScale += fwd ? scaleSpeed : -scaleSpeed;
                    return;
                }

                var ray = p_Camera.ViewportPointToRay(p_ViewportMiddle);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, m_MaxRaycastDistance, m_RaycastLayerMask)) {
                    m_ScalingObject = hit.transform;
                    m_ScalingObject.localScale += fwd ? scaleSpeed : -scaleSpeed;
                } else {
                    m_ScalingObject = null;
                }
            }
        }
    }
}
