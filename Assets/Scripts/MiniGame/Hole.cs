using UnityEngine;

public class Hole : MonoBehaviour
{
    public bool IsFilled { get; private set; } = false;

    public void FillHole()
    {
        IsFilled = true;
        // Optional: Loch optisch schließen, z.B. Collider deaktivieren
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;

        // Optional: Loch-Sprite ändern oder Effekte abspielen
    }
}