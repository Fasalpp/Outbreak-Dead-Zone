using TMPro;
using UnityEngine;

public class AddKillCount : MonoBehaviour
{
    public TextMeshProUGUI killCountText;
    [HideInInspector]public int killCount = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddKill()
    {
        killCount++;
        if (killCountText != null) killCountText.text = "KILL : " + killCount;
    }
}
