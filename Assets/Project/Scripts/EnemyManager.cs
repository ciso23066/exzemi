using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    public GameObject mgOPrefab; // Inspector Ç≈ê›íË
    public float interval = 5f;   // 5ïbÇ≤Ç∆Ç…èüéËÇ…îΩâû

    void Start()
    {
        StartCoroutine(EnemyRoutine());
    }

    IEnumerator EnemyRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(interval);

            AtomBlock[] atoms = Object.FindObjectsByType<AtomBlock>(FindObjectsSortMode.None);

            // Mg + O ÇÉâÉìÉ_ÉÄÇ…1ëgëIÇ‘
            AtomBlock mg1 = null, mg2 = null, o1 = null, o2 = null;
            int mgCount = 0, oCount = 0;

            foreach (var a in atoms)
            {
                if (a.type == AtomType.Mg)
                {
                    if (mgCount == 0) mg1 = a;
                    else if (mgCount == 1) mg2 = a;
                    mgCount++;
                }
                if (a.type == AtomType.O)
                {
                    if (oCount == 0) o1 = a;
                    else if (oCount == 1) o2 = a;
                    oCount++;
                }
                if (mgCount >= 2 && oCount >= 2) break;
            }

            if (mg1 != null && mg2 != null && o1 != null && o2 != null)
            {
                Vector3 center = (mg1.transform.position + mg2.transform.position + o1.transform.position + o2.transform.position) / 4f;

                Destroy(mg1.gameObject);
                Destroy(mg2.gameObject);
                Destroy(o1.gameObject);
                Destroy(o2.gameObject);

                for (int i = 0; i < 2; i++)
                {
                    Instantiate(mgOPrefab, center + Random.insideUnitSphere * 0.3f, Quaternion.identity);
                    if (GameManager.Instance != null)
                    {
                        GameManager.Instance.AddEnemyReaction();
                    }
                }
            }
        }
    }
}
