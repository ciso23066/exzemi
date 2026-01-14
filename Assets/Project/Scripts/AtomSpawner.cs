using System.Collections.Generic;
using UnityEngine;

public class AtomSpawner : MonoBehaviour
{
    public GameObject mgPrefab;
    public GameObject oPrefab;
    public int maxAtoms = 100;
    public float spawnHeight = 12f;

    private List<GameObject> atoms = new List<GameObject>();

    void Update()
    {
        // リスト内の削除されたオブジェクトをクリア
        atoms.RemoveAll(a => a == null);

        // 常に maxAtoms にする
        while (atoms.Count < maxAtoms)
        {
            SpawnAtom();
        }
    }

    void SpawnAtom()
    {
        // 箱の範囲内に生成
        Vector3 pos = new Vector3(
            Random.Range(-3.8f, 3.8f),  // X
            spawnHeight,                // Y
            Random.Range(-3.8f, 3.8f)  // Z
        );

        // ランダムで Mg または O
        GameObject prefab = Random.value > 0.5f ? mgPrefab : oPrefab;
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity);

        atoms.Add(obj);
    } // ← SpawnAtom 閉じカッコ

} // ← クラス AtomSpawner 閉じカッコ
