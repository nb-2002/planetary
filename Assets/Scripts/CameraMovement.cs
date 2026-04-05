using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject chaseObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = (chaseObject.transform.position - transform.position);
        pos -= new Vector3 (0, pos.y, 0);
        transform.position += pos / 2;
    }
}
