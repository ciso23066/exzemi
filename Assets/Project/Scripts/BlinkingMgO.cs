using UnityEngine;

public class BlinkingMgO : MonoBehaviour
{
    private Renderer ren;
    private Color emissionColor = Color.red * 3.0f; // 発光色（強度3）
    private static readonly int EmissionColorProperty = Shader.PropertyToID("_EmissionColor");

    void Start()
    {
        ren = GetComponent<Renderer>();
        // マテリアルのEmissionを有効にする
        ren.material.EnableKeyword("_EMISSION");
    }

    void Update()
    {
        // サイン波を使って、時間経過で 0.0 ～ 1.0 の間をふわふわさせる
        float lerp = Mathf.PingPong(Time.time * 2.0f, 1.0f);

        // 色を黒(0)から赤(lerp)の間で変化させる
        ren.material.SetColor(EmissionColorProperty, emissionColor * lerp);
    }
}