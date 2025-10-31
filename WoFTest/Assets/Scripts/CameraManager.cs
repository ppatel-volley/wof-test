using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private bool captureSceneOffset = true;

    [SerializeField]
    private Vector3 localOffset = new Vector3(0f, 0f, 3f);

    [SerializeField]
    private Vector3 upDirection = Vector3.up;

    private bool offsetCaptured;

    void Awake()
    {
        if (target == null)
        {
            GameObject found = GameObject.Find("Plane.011");
            if (found != null)
            {
                target = found.transform;

                //here we are going to calculate the desired position of the camera.
                //we want to get the target object, and offset right down it's z axis. by a given distance to get the 
                //desired position.
                Vector3 desiredPosition = target.position + target.forward * localOffset.z;
                transform.position = desiredPosition;
                transform.LookAt(target);
            }
            else
            {
                Debug.LogWarning("Camera could not find a GameObject named 'Plane.011'.");
                return;
            }
        }

        Debug.Log("Awake: Camera target: " + target.name);
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        
        Debug.Log("Late Update Camera position: " + transform.position);
    }

}
