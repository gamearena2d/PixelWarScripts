using UnityEngine;

public class ClickToPaint : MonoBehaviour
{
    public BattlefieldPainter painter;
    public Color paintColor = default;  // lascialo vuoto per blu
    public int radiusCells = 0;         // 0 = 1 cella; 1 = ~3x3; 2 = ~5x5

    void Reset()
    {
        if (paintColor == default) paintColor = BattlefieldPainter.TeamBlue;
    }

    void Update()
    {
        // tasto sinistro: dipingi
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                if (hit.collider.TryGetComponent(out BattlefieldPainter p))
                {
                    if (radiusCells <= 0)
                        p.PaintCellAtUV(hit.textureCoord, paintColor);
                    else
                        p.PaintCircleAtUV(hit.textureCoord, radiusCells, paintColor);
                }
            }
        }

        // tasto destro: pulisci tutto (debug)
        if (Input.GetMouseButtonDown(1) && painter != null)
            painter.ClearAll();
    }
}
