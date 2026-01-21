using UnityEngine;

public class GridSpawner : MonoBehaviour
{
    public int width = 10;
    public int height = 10;
    public float spacing = 1.1f;
    public Vector3 gridOffset;

    public GameObject mgPrefab;
    public GameObject oPrefab;

    // SelectionManagerからアクセスしやすいようにstaticにする
    public static GridSpawner Instance;
    public AtomBlock[,] grid;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        // 異常な数値を防ぐガード
        if (width <= 0) width = 10;
        if (height <= 0) height = 10;

        grid = new AtomBlock[width, height];
        GenerateGrid();
    }

    void GenerateGrid()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                SpawnAtom(x, y);
            }
        }
    }

    void SpawnAtom(int x, int y)
    {
        // 基本の計算に offset を足す
        Vector3 pos = new Vector3(x * spacing, y * spacing, 0) + gridOffset;

        GameObject prefab = Random.value > 0.5f ? mgPrefab : oPrefab;
        GameObject obj = Instantiate(prefab, pos, Quaternion.identity, transform);
        // ...以下略
    }
}