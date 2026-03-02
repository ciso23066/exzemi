using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement; // シーン移動に必要

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public float timeLimit = 30f;
    public TMP_Text timerText;

    private float timeLeft;
    private int successCount = 0; // 反応させた数
    private int enemyMgOCount = 0; // 勝手に反応した数

    // リザルトシーンに渡すためのデータ（静的変数）
    public static int finalSuccess;
    public static int finalEnemy;
    public static int finalScore;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        timeLeft = timeLimit;
    }

    void Update()
    {
        if (timeLeft <= 0) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0)
        {
            timeLeft = 0;
            GameOver();
        }

        timerText.text = "Time: " + timeLeft.ToString("F1");
    }

    // プレイヤーが成功させた時
    public void AddSuccess()
    {
        successCount += 2; // 1回につき+2
    }

    // EnemyManagerで勝手に反応した時
    public void AddEnemyReaction()
    {
        enemyMgOCount++;
    }

    void GameOver()
    {
        // データを静的変数に保存してシーンを跨げるようにする
        finalSuccess = successCount;
        finalEnemy = enemyMgOCount;
        finalScore = successCount - enemyMgOCount;

        Debug.Log("Game Over! Score: " + finalScore);

        // "ResultScene" という名前のシーンへ移動
        SceneManager.LoadScene("ResultScene");
    }
}