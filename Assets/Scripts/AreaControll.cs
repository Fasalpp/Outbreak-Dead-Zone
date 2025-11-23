using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AreaControll : MonoBehaviour
{
    public TextMeshProUGUI countdownText;
    public GameObject outofmapCanvas;
    private bool isCountingDown = false;
    private FirstPersonController controller;
    private Coroutine countdownCoroutine;

    void Start()
    {
        outofmapCanvas.SetActive(false);
        countdownText.text = "";
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && !isCountingDown)
        {
            controller = other.GetComponent<FirstPersonController>();
            isCountingDown = true;
            countdownCoroutine = StartCoroutine(StartCountdown());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isCountingDown)
        {
            StopCoroutine(countdownCoroutine);
            countdownText.text = "";
            isCountingDown = false;
        }
    }

    private IEnumerator StartCountdown()
    {
        int timeLeft = 10;
        while (timeLeft > 0)
        {
            countdownText.text = "Returning in: " + timeLeft + "s";
            yield return new WaitForSeconds(1f);
            timeLeft--;
        }

        countdownText.text = "";
        if (controller != null) controller.enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (!outofmapCanvas.activeSelf)
            outofmapCanvas.SetActive(true);
        Time.timeScale = 0;
    }

    
}
