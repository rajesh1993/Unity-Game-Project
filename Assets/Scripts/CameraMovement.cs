using UnityEngine;
using System.Collections;


public class CameraMovement : MonoBehaviour
{

    public float speed;
    public bool enable;
    public bool characterLock;
    //private bool previousEnable;
    //private bool previousCharacterMotor;


    void Start()
    {
        enable = false;
        characterLock = false;
        //this.GetComponent<CharacterController>().enabled = false;
    }

    void Update()
    {
         if(Input.GetKeyDown(KeyCode.F))
        {
            enable = !enable;
            if (!enable && !characterLock)
            {
                this.GetComponent<CharacterController>().enabled = true;
            } 
            
        }
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            characterLock = !characterLock;
            if (characterLock)
            {
                this.GetComponent<CharacterController>().enabled = false;
            }
            else
            {
                this.GetComponent<CharacterController>().enabled = true;
            }
        }
		//if (this.GetComponent<CharacterController>())
  //      {

  //          if (Input.GetKeyDown(KeyCode.LeftControl))
  //          {
		//		if (!this.GetComponent<CharacterController>().enabled && !enable)
  //              {   
		//			this.GetComponent<CharacterController>().enabled = previousCharacterMotor;
  //                  enable = previousEnable;
  //                  previousCharacterMotor = false;
  //                  previousEnable = false;
  //              }
  //              else
  //              {
  //                  previousEnable = enable;
		//			previousCharacterMotor = this.GetComponent<CharacterController>().enabled;
		//			this.GetComponent<CharacterController>().enabled = false;
  //                  enable = false;
  //              }
  //          }

  //          if (Input.GetKeyDown(KeyCode.F) && !previousCharacterMotor && !previousEnable)
  //          {
  //              enable = !enable;
  //              if (enable)
  //              {
		//			this.GetComponent<CharacterController>().enabled = false;
  //              }
  //              else
  //              {
		//			this.GetComponent<CharacterController>().enabled = true;
  //              }
  //          }

  //      }

        if (enable && !characterLock)
        {
            this.GetComponent<CharacterController>().enabled = false;
            Vector3 direction = new Vector3();

            if (Input.GetKey(KeyCode.W)) direction.z += 1.0f;
            if (Input.GetKey(KeyCode.S)) direction.z -= 1.0f;
            if (Input.GetKey(KeyCode.A)) direction.x -= 1.0f;
            if (Input.GetKey(KeyCode.D)) direction.x += 1.0f;
            if (Input.GetKey(KeyCode.LeftShift)) direction.y -= 1.0f;
            if (Input.GetKey(KeyCode.Space)) direction.y += 1.0f;
            direction.Normalize();

            transform.Translate(direction * speed * Time.deltaTime);
        }
    }
}