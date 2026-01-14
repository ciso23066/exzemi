using UnityEngine;
using TMPro; // TextMesh Pro 用

public class GameManager : MonoBehaviour
{
    [Header("制限時間（秒）")]
    public float timeLimit = 120f; // 2分
    private float timer;

    [Header("UI")]
    public TMP_Text timerText; // Inspector で TimerText をアタッチ

    [Header("ゲーム終了")]
    public bool isGameOver = false;

    void Start()
    {
        timer = timeLimit;
        UpdateTimerUI();
    }

    void Update()
    {
        if (isGameOver) return;

        // 時間経過
        timer -= Time.deltaTime;
        if (timer < 0f)
        {
            timer = 0f;
            GameOver();
        }

        UpdateTimerUI();
    }

    // タイマーUI更新
    void UpdateTimerUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer % 60f);
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    void GameOver()
    {
        isGameOver = true;
        UnityEngine.Debug.Log("時間切れ！ゲーム終了");

        // TODO: ゲーム終了画面を表示したい場合はここに処理追加
        // 例：CanvasのPanelを有効化してスコア表示
    }

    // 外部からゲーム終了を呼ぶ場合
    public void EndGame()
    {
        if (!isGameOver)
            GameOver();
    }
}
