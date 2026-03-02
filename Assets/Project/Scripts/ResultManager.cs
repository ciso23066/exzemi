using TMPro; // TextMeshProを使うために必須
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultManager : MonoBehaviour
{
    // [SerializeField] をつけることで、private変数でも確実に枠を表示させます
    [Header("UI Text References")]
    [SerializeField] private TMP_Text successText;
    [SerializeField] private TMP_Text enemyText;
    [SerializeField] private TMP_Text scoreText;

    void Start()
    {
        // GameManagerに保存されている静的変数を参照
        // ※GameManager側で public static int finalSuccess などが定義されている必要があります
        if (successText != null) successText.text = "MgO: " + GameManager.finalSuccess;
        if (enemyText != null) enemyText.text = "Enemy: " + GameManager.finalEnemy;
        if (scoreText != null) scoreText.text = "score: " + GameManager.finalScore;
    }

    public void OnRetryButton()
    {
        SceneManager.LoadScene("title");
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