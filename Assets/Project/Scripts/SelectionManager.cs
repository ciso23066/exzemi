using System.Collections.Generic;
using UnityEngine;
// Debugの衝突を防ぐ
using Debug = UnityEngine.Debug;

public class SelectionManager : MonoBehaviour
{
    public Camera mainCamera;
    public LineRenderer lineRenderer; // インスペクターで割り当て
    public GameObject reactionEffectPrefab; // 2で作ったPrefabをアタッチ

    private List<AtomBlock> selected = new List<AtomBlock>();

    void Start()
    {
        if (mainCamera == null) mainCamera = Camera.main;

        // LineRendererの初期設定
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 0;
            lineRenderer.startWidth = 0.2f;
            lineRenderer.endWidth = 0.2f;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ClearSelection();
        }

        if (Input.GetMouseButton(0))
        {
            TrySelect();
        }

        // 修正ポイント1：マウスを離した時の処理順序
        if (Input.GetMouseButtonUp(0))
        {
            CheckReaction();
            // CheckReactionでDestroyされた場合、selectedの中身が壊れるので
            // 線を引く前に必ずクリアする、もしくはリストを整理する
            ClearSelection();
        }

        DrawSelectionLine();
    }

    void TrySelect()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        // Raycastを1回にまとめ、hit情報を取得
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // 1. 生成されたMgO（BlinkingMgO）なら無視
            if (hit.collider.GetComponent<BlinkingMgO>() != null) return;

            // 2. AtomBlockコンポーネントを取得
            AtomBlock block = hit.collider.GetComponent<AtomBlock>();

            // 3. 選択可能な条件をチェック
            if (block != null && !selected.Contains(block) && block.gameObject != null)
            {
                // 最初の一つ目、もしくは直前のものと隣接しているかチェック
                if (selected.Count == 0 || IsAdjacent(block, selected[selected.Count - 1]))
                {
                    selected.Add(block);
                    block.Select();
                }
            }
        }
    }

    bool IsAdjacent(AtomBlock a, AtomBlock b)
    {
        // Mathf.Abs は絶対値（距離）を計算します
        int dx = Mathf.Abs(a.x - b.x);
        int dy = Mathf.Abs(a.y - b.y);

        // 上下左右のみ許可する場合 (今の設定)
        // return (dx + dy) == 1; 

        // 斜めも許可する場合（こちらの方が操作感がスムーズになります）
        return dx <= 1 && dy <= 1;
    }

    void DrawSelectionLine()
    {
        if (lineRenderer == null) return;

        // 修正ポイント2：リスト内の要素が生きているかチェックする
        // selectedの中にDestroyされたオブジェクトが混ざっているとエラーになるため
        // nullチェック(b != null)を追加
        int validCount = 0;
        foreach (var b in selected)
        {
            if (b != null) validCount++;
        }

        if (validCount < 1)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = validCount;
        int index = 0;
        for (int i = 0; i < selected.Count; i++)
        {
            if (selected[i] != null) // ここでnullチェック
            {
                Vector3 pos = selected[i].transform.position;
                pos.z -= 0.5f;
                lineRenderer.SetPosition(index, pos);
                index++;
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

    
    

    void CheckReaction()
    {
        int mgCount = 0;
        int oCount = 0;

        foreach (var b in selected)
        {
            if (b.type == AtomType.Mg) mgCount++;
            if (b.type == AtomType.O) oCount++;
        }

        // 2Mg + O2 = 2MgO の判定
        if (mgCount == 2 && oCount == 2 && selected.Count == 4)
        {
            // 4つの中心点を計算
            Vector3 center = Vector3.zero;
            foreach (var b in selected) center += b.transform.position;
            center /= 4f;

            // エフェクト生成
            if (reactionEffectPrefab != null)
            {
                Instantiate(reactionEffectPrefab, center + Vector3.back * 0.5f, Quaternion.identity);
            }

            foreach (var b in selected)
            {
                // GridSpawnerの静的インスタンス経由でグリッドを空にする
                GridSpawner.Instance.grid[b.x, b.y] = null;
                Destroy(b.gameObject);
            }
            Debug.Log("化学反応成功: 2MgO生成");
        }
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddSuccess();
        }
    }
}