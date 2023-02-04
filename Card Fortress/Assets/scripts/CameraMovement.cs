using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]  public float cameraSpeed;
    public int border;


    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        if (Input.mousePosition.x > Screen.width - 5)
        {
            horizontal = 1;
        }
        else if (Input.mousePosition.x < 5)
        {
            horizontal = -1;
        }

        if (horizontal != 0)
        {
            float x = transform.position.x + Time.deltaTime * horizontal * cameraSpeed;
            x = Mathf.Clamp(x, -border, border);
            transform.position = new Vector3(x, transform.position.y, -10);
        }

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            

            if(Physics.Raycast(ray,out RaycastHit hitInfo))
            {
                Debug.Log(hitInfo.collider.gameObject.name);
            }
        }

        

    }
}
