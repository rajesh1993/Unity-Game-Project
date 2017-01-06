using UnityEngine;
using System.Collections;

public class ObjectFlight : MonoBehaviour {



    public float speed = 0;
    public float maxSpeed = 5.0f;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            RaycastHit hitPoint;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hitPoint, Mathf.Infinity))
            {
                StartCoroutine(MoveObject(hitPoint.transform.gameObject));
            }
        }
    }

    public IEnumerator MoveObject(GameObject gameObject)
    {
        Vector3 direction = new Vector3();

        while (true)
        {
            if (Input.anyKey)
            {
                if (Input.GetKey(KeyCode.UpArrow))
                    direction.z += maxSpeed;
                if (Input.GetKey(KeyCode.DownArrow))
                    direction.z -= maxSpeed;
                if (Input.GetKey(KeyCode.LeftArrow))
                    direction.x -= maxSpeed;
                if (Input.GetKey(KeyCode.RightArrow))
                    direction.x += maxSpeed;
                if (Input.GetKey(KeyCode.Comma))
                    direction.y -= maxSpeed;
                if (Input.GetKey(KeyCode.Question))
                    direction.y += maxSpeed;

                gameObject.transform.Translate(direction * speed * Time.deltaTime, Camera.main.transform);

                if (Input.GetKeyDown(KeyCode.Z))
                {
                    yield break;
                }
            }

            //Get rid of any momentum saved up
            direction.x = 0;
            direction.y = 0;
            direction.z = 0;
            yield return null;
        }
    }

}
