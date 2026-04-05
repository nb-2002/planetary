using UnityEngine;
using UnityEngine.AI;

public class CameraMovement1 : MonoBehaviour
{
    private Camera cam;
    private Quaternion rotation;
    private float zoomOut = 200;
    private float zoomIn = 10;

    void Start()
    {
        cam = transform.GetComponentInChildren<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R)) transform.rotation = Quaternion.Euler(-45, 0, 0);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider != null)
                {
                    transform.SetParent(hit.collider.gameObject.transform);
                    transform.localPosition = Vector3.zero;
                }
            }

        }
        float verticalInput = Input.GetAxis("Vertical");
        float horizontalInput = Input.GetAxis("Horizontal");
        float thirdAxis = 0;

        if (Input.GetKey(KeyCode.Q)) thirdAxis = -1;
        else if(Input.GetKey(KeyCode.E)) thirdAxis = 1;


        float zoom = Input.GetAxis("Mouse ScrollWheel");
        Vector3 newPos = cam.transform.localPosition;
        newPos.y += zoom;
        if (zoom > 0) newPos.y = zoomIn;
        else if (zoom < 0) { newPos.y = zoomOut; zoom = -zoom; }
        newPos = Vector3.Lerp(cam.transform.localPosition, newPos, zoom);
        cam.transform.localPosition = newPos;

        transform.rotation *= Quaternion.AngleAxis(verticalInput, Vector3.right);
        transform.rotation *= Quaternion.AngleAxis(horizontalInput, Vector3.back);
        transform.rotation *= Quaternion.AngleAxis(thirdAxis, Vector3.up);

    }
}
