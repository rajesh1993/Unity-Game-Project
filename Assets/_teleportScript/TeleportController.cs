using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class TeleportController : MonoBehaviour
{

    [SerializeField]
    private string audioFileName = "soundTeleport.mp3";    

    public KeyCode m_TeleportKey = KeyCode.T;

    public float m_TeleportDistance = 1f;

    public float m_MaxRaycastDistance = Mathf.Infinity;
    public LayerMask m_RaycastLayerMask = int.MaxValue;

    private readonly Vector3 p_ViewportMiddle = new Vector3(0.5f, 0.5f, 0f);

    private Camera p_Camera;

    void Start()
    {
        var cameras = GetComponentsInChildren<Camera>();
        if (cameras.Length == 0)
        {
            Debug.LogWarning("No cameras found.");
            p_Camera = Camera.main;
        }
        else if (cameras.Length > 1)
        {
            Debug.LogWarning("More than one camera found.");
            p_Camera = Camera.main;
        }
        else
        {
            p_Camera = cameras[0];
        }
    }

    void Update()
    {
        var teleportKey = Input.GetKeyDown(m_TeleportKey);

        if (teleportKey)
        {
            var ray = p_Camera.ViewportPointToRay(p_ViewportMiddle);
            RaycastHit hit;
            
            //Audio
            if (gameObject.GetComponent<AudioSource>() == null)
            {
                AudioSource audio = gameObject.AddComponent<AudioSource>();
            }
            AudioClip clip = (AudioClip)Resources.Load(audioFileName);

            if (clip != null)
            {
                GetComponent<AudioSource>().PlayOneShot(clip, 1.0F);
            }
            else
            {
                Debug.Log("The teleportation audio file is either missing or not set as the serialized field");
            }

            if (Physics.Raycast(ray, out hit, m_MaxRaycastDistance, m_RaycastLayerMask))
            {
                var objCollider = hit.collider;
                var selfCollider = GetComponent<Collider>();
                var selfToObj = objCollider.ClosestPointOnBounds(selfCollider.bounds.center);
                var objToSelf = selfCollider.ClosestPointOnBounds(objCollider.bounds.center);
                var delta = selfToObj - objToSelf;
                var mag = delta.magnitude;
                var newMag = mag - m_TeleportDistance;
                delta *= (1F / mag) * newMag;
                transform.position += delta;
                // p_Camera.gameObject.transform.LookAt(objCollider.bounds.center);
            }
        }
    }
}

