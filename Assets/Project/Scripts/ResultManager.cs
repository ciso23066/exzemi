using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class ResultManager : MonoBehaviour
{
    [Header("UI Text References")]
    [SerializeField] private TMP_Text successText;
    [SerializeField] private TMP_Text enemyText;
    [SerializeField] private TMP_Text scoreText;

    [Header("Audio Settings")]
    public AudioSource audioSource; // リザルト用のAudioSource
    public AudioClip resultSound;   // シーン開始時に鳴らす音

    void Start()
    {
        // 前回のスコア表示処理
        if (successText != null) successText.text = "MgO: " + GameManager.finalSuccess;
        if (enemyText != null) enemyText.text = "Enemy: " + GameManager.finalEnemy;
        if (scoreText != null) scoreText.text = "Score: " + GameManager.finalScore;

        // --- リザルト効果音を鳴らす ---
        if (audioSource != null && resultSound != null)
        {
            audioSource.PlayOneShot(resultSound);
        }
    }

    // （OnRetryButton などの既存メソッドはそのまま）
    public void OnRetryButton()
    {
        SceneManager.LoadScene("StartScene");
    }

    public void OnQuitButton()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}