using UnityEngine;

// 1. MgOタイプを追加
public enum AtomType { Mg, O, MgO }

public class AtomBlock : MonoBehaviour
{
    public AtomType type;
    public int x, y;

    private Vector3 originalScale;
    private Renderer[] childRenderers; // 子オブジェクトのRendererを保持
    private Color originalColor;

    // 点滅用
    private static readonly int EmissionColorProperty = Shader.PropertyToID("_EmissionColor");
    private Color alertColor = Color.red * 4.0f; // HDR発光強度を4に設定

    void Awake()
    {
        // 2. 自分と子オブジェクトすべてのRendererを取得
        childRenderers = GetComponentsInChildren<Renderer>();
        originalScale = transform.localScale;

        // 代表して最初のRendererの色を保持（単体Mg/O用）
        if (childRenderers.Length > 0)
        {
            originalColor = childRenderers[0].material.color;
        }

        // MgOの場合は発光（Emission）を有効化
        if (type == AtomType.MgO)
        {
            foreach (var ren in childRenderers)
            {
                ren.material.EnableKeyword("_EMISSION");
            }
        }
    }

    void Update()
    {
        // 3. MgOタイプなら時間経過で赤く点滅させる
        if (type == AtomType.MgO)
        {
            float lerp = (Mathf.Sin(Time.time * 8.0f) + 1.0f) / 2.0f; // 0〜1を往復
            Color finalColor = alertColor * lerp;

            foreach (var ren in childRenderers)
            {
                ren.material.SetColor(EmissionColorProperty, finalColor);
            }
        }
    }

    public void SetGridPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // 選択された時の見た目
    public void Select()
    {
        // MgOなら反応させない（選択不可）
        if (type == AtomType.MgO) return;

        transform.localScale = originalScale * 1.2f;
        foreach (var ren in childRenderers)
        {
            ren.material.color = Color.yellow;
        }
    }

    // 選択解除された時の見た目
    public void Deselect()
    {
        if (type == AtomType.MgO) return;

        transform.localScale = originalScale;
        foreach (var ren in childRenderers)
        {
            ren.material.color = originalColor;
        }
    }
}