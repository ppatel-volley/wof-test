using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Transform target;

    [SerializeField]
    private Vector3 offset = new Vector3(0f, 5f, -10f);

    void Awake()
    {
        if (target != null)
        {
            if (offset == Vector3.zero)
            {
                offset = transform.position - target.position;
            }
            return;
        }

        GameObject found = GameObject.Find("Plane.011");
        if (found != null)
        {
            target = found.transform;
            if (offset == Vector3.zero)
            {
                offset = transform.position - target.position;
            }
        }
        else
        {
            Debug.LogWarning("Camera could not find a GameObject named 'Plane.002'.");
        }

        Debug.Log("Awake: Camera target: " + target.name);
    }

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        transform.position = target.position + offset;
        transform.LookAt(target);
        Debug.Log("Late Update Camera position: " + transform.position);
    }
}
