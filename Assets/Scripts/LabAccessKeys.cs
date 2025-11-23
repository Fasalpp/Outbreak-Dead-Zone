using UnityEngine;

public class LabAccessKeys : MonoBehaviour
{
    [HideInInspector] public bool accessKey1 = false;
    [HideInInspector] public bool accessKey2 = false;
    [HideInInspector] public bool accessKey3 = false;
    void Start()
    {

    }
    void Update()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.root.CompareTag("KeyL1"))
        {
            accessKey1 = true;
            Destroy(other.gameObject.transform.root.gameObject);
        }
        else if (other.gameObject.transform.root.CompareTag("KeyL2"))
        {
            accessKey2 = true;
            Destroy(other.gameObject.transform.root.gameObject);
        }
        else if (other.gameObject.transform.root.CompareTag("KeyL3"))
        {
            accessKey3 = true;
            Destroy(other.gameObject.transform.root.gameObject);
        }
    }
}
