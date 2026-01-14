using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    private List<AtomBlock> selected = new List<AtomBlock>();

    public GameObject mgOPrefab; // MgOƒvƒŒƒnƒu

    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                var atom = hit.collider.GetComponent<AtomBlock>();
                if (atom != null && !selected.Contains(atom) && atom.type != AtomType.MgO)
                {
                    atom.Select();
                    selected.Add(atom);
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            CheckReaction();
        }
    }

    void CheckReaction()
    {
        int mg = 0, o = 0;

        foreach (var a in selected)
        {
            if (a.type == AtomType.Mg) mg++;
            if (a.type == AtomType.O) o++;
        }

        if (mg == 2 && o == 2)
        {
            DoReaction();
        }
        else
        {
            ClearSelection();
        }
    }

    void ClearSelection()
    {
        foreach (var a in selected)
        {
            if (a != null) a.Deselect();
        }
        selected.Clear();
    }

    void DoReaction()
    {
        Vector3 center = Vector3.zero;

        foreach (var a in selected)
        {
            center += a.transform.position;
            Destroy(a.gameObject);
        }

        center /= selected.Count;

        for (int i = 0; i < 2; i++)
        {
            Instantiate(mgOPrefab, center + Random.insideUnitSphere * 0.3f, Quaternion.identity);
        }

        selected.Clear();
    }
}
