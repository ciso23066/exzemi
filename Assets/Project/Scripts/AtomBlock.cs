using UnityEngine;

public enum AtomType { Mg, O }

public class AtomBlock : MonoBehaviour
{
    public AtomType type;
    public int x, y;

    private Vector3 originalScale;
    private Renderer ren;
    private Color originalColor;

    void Awake()
    {
        ren = GetComponent<Renderer>();
        originalScale = transform.localScale;
        originalColor = ren.material.color;
    }

    public void SetGridPosition(int x, int y)
    {
        this.x = x;
        this.y = y;
    }

    // ‘I‘ğ‚³‚ê‚½‚ÌŒ©‚½–Ú
    public void Select()
    {
        transform.localScale = originalScale * 1.2f;
        ren.material.color = Color.yellow;
    }

    // ‘I‘ğ‰ğœ‚³‚ê‚½‚ÌŒ©‚½–Ú
    public void Deselect()
    {
        transform.localScale = originalScale;
        ren.material.color = originalColor;
    }
}