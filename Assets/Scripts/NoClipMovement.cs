using UnityEngine;

public class NoClipMovement : MonoBehaviour
{
    public float speed;
    public float rotationSpeed;
    
    void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(Vector3.back * speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
        }

        transform.eulerAngles += new Vector3(-Input.mousePositionDelta.y, Input.mousePositionDelta.x, 0) * rotationSpeed;



    }
}
