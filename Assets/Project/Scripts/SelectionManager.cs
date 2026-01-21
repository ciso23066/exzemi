using System.Collections.Generic;
using UnityEngine;
// Debugの衝突を防ぐ
using Debug = UnityEngine.Debug;

public class SelectionManager : MonoBehaviour
{
    public Camera mainCamera;
    public LineRenderer lineRenderer; // インスペクターで割り当て

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

        if (Input.GetMouseButtonUp(0))
        {
            CheckReaction();
            ClearSelection();
        }

        DrawSelectionLine();
    }

    void TrySelect()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            AtomBlock block = hit.collider.GetComponent<AtomBlock>();
            if (block != null && !selected.Contains(block))
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

        if (selected.Count < 1)
        {
            lineRenderer.positionCount = 0;
            return;
        }

        lineRenderer.positionCount = selected.Count;
        for (int i = 0; i < selected.Count; i++)
        {
            // 玉の少し手前(Z-0.5)に線を引く
            Vector3 pos = selected[i].transform.position;
            pos.z -= 0.5f;
            lineRenderer.SetPosition(i, pos);
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

        // 2Mg + O2 (合計4つ) の判定
        if (mgCount == 2 && oCount == 2 && selected.Count == 4)
        {
            foreach (var b in selected)
            {
                // グリッド配列内の参照をクリア
                GridSpawner.Instance.grid[b.x, b.y] = null;
                Destroy(b.gameObject);
            }
            Debug.Log("化学反応成功: 2Mg + O2 -> 2MgO");
        }
    }
}