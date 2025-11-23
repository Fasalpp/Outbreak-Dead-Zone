using UnityEngine;

public class DoorOpenClose : MonoBehaviour
{
    [SerializeField] GameObject door;
    [SerializeField] Vector3 openRotation; 
    [SerializeField] float rotationSpeed = 2f;

    private bool ready = false;
    private bool isOpen = false;
    private Quaternion initialRotation;
    private Quaternion targetRotation;

    void Start()
    {
        if (door == null) door = this.gameObject;

        initialRotation = door.transform.rotation;
        targetRotation = Quaternion.Euler(openRotation) * initialRotation;
    }

    // Update is called once per frame
    void Update()
    {
        if (ready)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                isOpen = !isOpen;
                Debug.Log("IsOpen : " + isOpen);
            }

            if (isOpen)
            {
                door.transform.rotation = Quaternion.Slerp(door.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
            else
            {
                door.transform.rotation = Quaternion.Slerp(door.transform.rotation, initialRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ready = true;
            Debug.Log("Ready");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ready = false;
        }
    }

}
