using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timeLimit = 90f;
    public TMP_Text timerText;

    private float timeLeft;

    void Start()
    {
        timeLeft = timeLimit;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0) timeLeft = 0;

        timerText.text = "Time: " + timeLeft.ToString("F1");

        if (timeLeft <= 0)
        {
            Debug.Log("Game Over");
            enabled = false;
        }
    }
}
