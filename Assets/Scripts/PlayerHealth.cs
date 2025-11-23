using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] float hitPoint = 100f;
    [SerializeField] int healthPack = 3;
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI healthPackText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    public Canvas gameOverCanvas;
    public FirstPersonController controller;
    [Header("Knockback")]
    [SerializeField] private FirstPersonController movementScript;
    [SerializeField] private float stunDuration = 1f;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float knockbackForce = 7f;
    [SerializeField] private CameraShake cameraShake;
    void Start()
    {
        if(gameOverCanvas != null)
        {
            gameOverCanvas.enabled = false;
        }
        healthPackText.text = healthPack.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H) && healthPack > 0)
        {
            hitPoint += 30f;
            if (hitPoint > 100) hitPoint = 100f;
            UpdateHealthBar(hitPoint);
            healthPack--;
        }
        if(healthPackText != null)
        {

            healthPackText.text = healthPack.ToString();
        }
    }
    public void TakeDamage(float damage, Vector3 hitSource)
    {
        hitPoint -= damage;
        Debug.Log(hitPoint);
        StartCoroutine(StunPlayer());
        ApplyKnockback(hitSource);
        UpdateHealthBar(hitPoint);
        if (cameraShake != null)
        {
            cameraShake.Shake();
        }
        if (hitPoint <= 0)
        {
            Debug.Log("Dead");
            Die();
        }
    }
    private void ApplyKnockback(Vector3 hitSource)
    {
        if (rb != null)
        {
            Vector3 knockDir = (transform.position - hitSource).normalized;
            rb.AddForce(knockDir * knockbackForce, ForceMode.Impulse);
        }
    }
    private IEnumerator StunPlayer()
    {
        if (movementScript != null)
        {
            movementScript.enabled = false;
        }

        yield return new WaitForSeconds(stunDuration);

        if (movementScript != null)
        {
            movementScript.enabled = true;
        }
    }

    void Die()
    {
        int score = 0;
        AddKillCount kill = FindAnyObjectByType<AddKillCount>();
        score = kill.killCount;
        if (PlayerPrefs.GetInt("NormalDifficulty") == 1)
        {
            if (PlayerPrefs.GetInt("NormalKillCount") < score)
            {
                PlayerPrefs.SetInt("NormalKillCount", score);
            }
        }
        else if (PlayerPrefs.GetInt("HardCoreDifficulty") == 1)
        {
            if (PlayerPrefs.GetInt("HardKillCount") < score)
            {
                PlayerPrefs.SetInt("HardKillCount", score);
            }
        }
        if (gameOverCanvas != null)
        {
            gameOverCanvas.enabled = true;
        }
        if (controller != null)
        {
            controller.enabled = false;
        }
        if(gameOverText != null)
        {
            gameOverText.text = "You are Dead";
        }
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    void UpdateHealthBar(float health)
    {
        health = Mathf.Clamp(health, 0, 100);
        healthBar.fillAmount = health / 100f;
    }

    public void AddHealthPack()
    {
        healthPack++;
    }
}
