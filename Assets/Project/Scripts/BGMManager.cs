using UnityEngine;

public class BGMManager : MonoBehaviour
{
    public static BGMManager Instance;

    void Awake()
    {
        // シーンを移動してもこのオブジェクトを破壊しない設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // すでに存在する場合は、重複しないように自分を消去
            Destroy(gameObject);
        }
    }
}