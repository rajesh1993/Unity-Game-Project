using UnityEngine;
using System.Collections;

public class Eyedrop : MonoBehaviour
{
    public Camera mainCamera;
    private Material[] newMaterials;
    private Material[] oldMaterials;
    private Renderer focusedRenderer;

    void Start()
    {
    }

    bool HasMaterial()
    {
        return (newMaterials != null);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (newMaterials == null)
            {
                RaycastHit hit;
                Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out hit))
                {
                    newMaterials = hit.collider.GetComponent<Renderer>().materials;
                }
                else
                    newMaterials = null;
            }
            else
                newMaterials = null;
        }

        if (newMaterials != null)
        {
            // Are we previewing a material?
            if (focusedRenderer != null)
                focusedRenderer.materials = oldMaterials;

            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                focusedRenderer = hit.collider.GetComponent<Renderer>();
                oldMaterials = focusedRenderer.materials;
                focusedRenderer.materials = newMaterials;

                // Apply the material
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    focusedRenderer = null;
                }
            }
        }
    }
}
