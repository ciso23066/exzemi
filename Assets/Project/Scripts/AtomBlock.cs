using UnityEngine;

public enum AtomType { Mg, O, MgO }

public class AtomBlock : MonoBehaviour
{
    public AtomType type;
    public bool selected;

    Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();
        UpdateColor();
    }

    public void Select()
    {
        selected = true;
        rend.material.color = Color.yellow;
    }

    public void Deselect()
    {
        selected = false;
        UpdateColor();
    }

    void UpdateColor()
    {
        if (type == AtomType.Mg)
            rend.material.color = Color.gray;

        if (type == AtomType.O)
            rend.material.color = Color.red;

        if (type == AtomType.MgO)
        {
            // êeÇ»ÇÃÇ≈âΩÇ‡ÇµÇ»Ç¢
        }
    }

}
