using UnityEngine;

public class LabDoor : MonoBehaviour
{
    private bool canOpen = false;
    private bool doorOpening = false;

    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject glassDoorLeft;
    public GameObject glassDoorRight;

    public LabAccessKeys keys;
    public bool Level1 = false;
    public bool Level2 = false;
    public bool Level3 = false;

    public float doorSpeed = 2f;
    public Vector3 leftDoorPos;
    public Vector3 rightDoorPos;

    private Vector3 leftDoorClosedPos, rightDoorClosedPos;
    private Vector3 leftDoorOpenPos, rightDoorOpenPos;

    private Vector3 glassLeftClosedPos, glassRightClosedPos;
    private Vector3 glassLeftOpenPos, glassRightOpenPos;

    void Start()
    {
        if (leftDoor && rightDoor)
        {
            leftDoorClosedPos = leftDoor.transform.position;
            rightDoorClosedPos = rightDoor.transform.position;
            leftDoorOpenPos = leftDoorClosedPos + leftDoorPos;
            rightDoorOpenPos = rightDoorClosedPos + rightDoorPos;
        }

        if (glassDoorLeft && glassDoorRight)
        {
            glassLeftClosedPos = glassDoorLeft.transform.position;
            glassRightClosedPos = glassDoorRight.transform.position;
            glassLeftOpenPos = glassLeftClosedPos + leftDoorPos;
            glassRightOpenPos = glassRightClosedPos + rightDoorPos;
        }
    }

    void Update()
    {
        if (canOpen && Input.GetKeyDown(KeyCode.E))
        {
            if (HasAccess())
                doorOpening = !doorOpening;
            else
                Debug.Log("You need the correct access key to open this door.");
        }

        if (doorOpening)
        {
            if (leftDoor && rightDoor)
            {
                leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftDoorOpenPos, Time.deltaTime * doorSpeed);
                rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightDoorOpenPos, Time.deltaTime * doorSpeed);
            }

            if (glassDoorLeft && glassDoorRight)
            {
                glassDoorLeft.transform.position = Vector3.Lerp(glassDoorLeft.transform.position, glassLeftOpenPos, Time.deltaTime * doorSpeed);
                glassDoorRight.transform.position = Vector3.Lerp(glassDoorRight.transform.position, glassRightOpenPos, Time.deltaTime * doorSpeed);
            }
        }
        else
        {
            if (leftDoor && rightDoor)
            {
                leftDoor.transform.position = Vector3.Lerp(leftDoor.transform.position, leftDoorClosedPos, Time.deltaTime * doorSpeed);
                rightDoor.transform.position = Vector3.Lerp(rightDoor.transform.position, rightDoorClosedPos, Time.deltaTime * doorSpeed);
            }

            if (glassDoorLeft && glassDoorRight)
            {
                glassDoorLeft.transform.position = Vector3.Lerp(glassDoorLeft.transform.position, glassLeftClosedPos, Time.deltaTime * doorSpeed);
                glassDoorRight.transform.position = Vector3.Lerp(glassDoorRight.transform.position, glassRightClosedPos, Time.deltaTime * doorSpeed);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            canOpen = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            canOpen = false;
    }

    private bool HasAccess()
    {
        if (keys != null)
        {
            if (Level1) return keys.accessKey1;
            if (Level2) return keys.accessKey1 && keys.accessKey2;
            if (Level3) return keys.accessKey1 && keys.accessKey2 && keys.accessKey3;
        }
        return true;
    }
}
