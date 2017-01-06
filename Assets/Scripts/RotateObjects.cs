using UnityEngine;
using System.Collections;

public class RotateObjects : MonoBehaviour
{
    public Camera mainCamera;
    private Transform target = null;
    private Quaternion targetRotation;
    private float rotationAmount = 90.0f;

    void Start()
    {
    }

    bool HasTarget()
    {
        return (target != null);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (target == null)
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    target = hit.transform;
                    targetRotation = target.rotation;
                }
                else
                    target = null;
            }
            else
                target = null;
        }

        if (target != null)
        {
            if (Input.GetKeyDown(KeyCode.W))
                 targetRotation *= Quaternion.Euler(mainCamera.transform.right * rotationAmount);
            if (Input.GetKeyDown(KeyCode.S))
                targetRotation *= Quaternion.Euler(mainCamera.transform.right * -rotationAmount);
            if (Input.GetKeyDown(KeyCode.A))
                targetRotation *= Quaternion.Euler(mainCamera.transform.forward * rotationAmount);
            if (Input.GetKeyDown(KeyCode.D))
                targetRotation *= Quaternion.Euler(mainCamera.transform.forward * -rotationAmount);
            if (Input.GetKeyDown(KeyCode.Q))
                targetRotation *= Quaternion.Euler(mainCamera.transform.up * rotationAmount);
            if (Input.GetKeyDown(KeyCode.E))
                targetRotation *= Quaternion.Euler(mainCamera.transform.up * -rotationAmount);
            Snap();
            target.rotation = Quaternion.RotateTowards(target.rotation, targetRotation, 10.0f);
        }
    }

    private void Snap()
    {
        Vector3 vec = targetRotation.eulerAngles;
        vec.x = Mathf.Round(vec.x / rotationAmount) * rotationAmount;
        vec.y = Mathf.Round(vec.y / rotationAmount) * rotationAmount;
        vec.z = Mathf.Round(vec.z / rotationAmount) * rotationAmount;
        targetRotation.eulerAngles = vec;
    }
}
