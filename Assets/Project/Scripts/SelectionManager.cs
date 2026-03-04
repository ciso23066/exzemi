using UnityEngine;
using System.Collections.Generic;

public class SelectionManager : MonoBehaviour
{
    public Camera mainCamera;
    public List<AtomBlock> selected = new List<AtomBlock>();
    public LineRenderer lineRenderer;
    public GameObject reactionEffectPrefab;

    [Header("Audio Settings")]
    public AudioSource audioSource; // スピーカー（自分自身のAudioSourceをドラッグ）
    public AudioClip selectSound;   // 選択した時の音（SEファイルをドラッグ）
    public AudioClip successSound;  // 反応成功時の派手な音（もしあれば）

    void Update()
    {
        // マウスを離した時に反応チェック
        if (Input.GetMouseButtonUp(0))
        {
            CheckReaction();
            ClearSelection(); // 消去後にリストを空にしてエラーを防ぐ
        }

        if (Input.GetMouseButtonDown(0))
        {
            ClearSelection();
        }

        if (Input.GetMouseButton(0))
        {
            TrySelect();
        }

        DrawSelectionLine();
    }

    void TrySelect()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            AtomBlock block = hit.collider.GetComponent<AtomBlock>();

            // まだ選択されておらず、かつMgOではないブロックを新しく選択した時
            if (block != null && !selected.Contains(block) && block.type != AtomType.MgO)
            {
                selected.Add(block);
                block.Select();

                // --- 選択音を鳴らす ---
                if (audioSource != null && selectSound != null)
                {
                    audioSource.PlayOneShot(selectSound);
                }
            }
        }
    }

    void CheckReaction()
    {
        int mgCount = 0;
        int oCount = 0;

        foreach (var b in selected)
        {
            if (b != null) // 生きているかチェック
            {
                if (b.type == AtomType.Mg) mgCount++;
                if (b.type == AtomType.O) oCount++;
            }
        }

        // 2Mg + O2 = 2MgO (合計4つ)
        if (mgCount == 2 && oCount == 2 && selected.Count == 4)
        {
            // スコア加算
            if (GameManager.Instance != null)
            {
                GameManager.Instance.AddSuccess();
            }

            // 反応成功音
            if (audioSource != null && successSound != null)
            {
                audioSource.PlayOneShot(successSound);
            }

            // エフェクト生成（中心点）
            Vector3 center = Vector3.zero;
            foreach (var b in selected) center += b.transform.position;
            center /= 4f;

            if (reactionEffectPrefab != null)
            {
                Instantiate(reactionEffectPrefab, center, Quaternion.identity);
            }

            // 原子を消去
            foreach (var b in selected)
            {
                if (b != null)
                {
                    // グリッド管理データがあれば消去
                    // GridSpawner.Instance.grid[b.x, b.y] = null;
                    Destroy(b.gameObject);
                }
            }
        }
    }

    void ClearSelection()
    {
        foreach (var b in selected)
        {
            if (b != null) b.Deselect();
        }
        selected.Clear();
    }

    void DrawSelectionLine()
    {
        if (lineRenderer == null) return;

        // 無効（Destroy済み）な要素を除いた有効なカウントを計算
        List<Vector3> validPositions = new List<Vector3>();
        foreach (var b in selected)
        {
            if (b != null)
            {
                Vector3 pos = b.transform.position;
                pos.z -= 0.5f; // 線が見えるように少し手前に
                validPositions.Add(pos);
            }
        }

        lineRenderer.positionCount = validPositions.Count;
        lineRenderer.SetPositions(validPositions.ToArray());
    }
}