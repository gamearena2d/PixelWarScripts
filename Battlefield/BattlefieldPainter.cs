using UnityEngine;

/// Gestisce la PaintTex (1 px = 1 cella) e fornisce funzioni per colorare celle/aree.
/// Pensato per URP Unlit + ShaderGraph con property _PaintTex.
[RequireComponent(typeof(Renderer))]
public class BattlefieldPainter : MonoBehaviour
{
    [Header("Grid (cells)")]
    public int cellsX = 28;
    public int cellsY = 56;

    [Header("Material & Property")]
    public string paintTexProperty = "_PaintTex";
    public FilterMode filterMode = FilterMode.Point;
    public TextureWrapMode wrapMode = TextureWrapMode.Clamp;

    // colori comodi
    public static readonly Color TeamBlue = new Color(0.2f, 0.6f, 1f, 1f);
    public static readonly Color TeamRed  = new Color(1f, 0.25f, 0.3f, 1f);
    public static readonly Color Neutral  = new Color(0f, 0f, 0f, 0f); // trasparente

    // runtime
    private Renderer rend;
    private Texture2D paintTex;
    private Color32[] pixels;
    private bool dirty;

    private Collider col; // per raycast/uv

    void Awake()
    {
        rend = GetComponent<Renderer>();
        col  = GetComponent<Collider>();
        if (!col) col = gameObject.AddComponent<MeshCollider>();

        // 1 px per cella
        paintTex = new Texture2D(cellsX, cellsY, TextureFormat.RGBA32, false, true);
        paintTex.filterMode = filterMode;
        paintTex.wrapMode   = wrapMode;

        pixels = new Color32[cellsX * cellsY];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = new Color32(0, 0, 0, 0);

        paintTex.SetPixels32(pixels);
        paintTex.Apply(false, false);

        rend.material.SetTexture(paintTexProperty, paintTex);
    }

    void LateUpdate()
    {
        if (!dirty) return;
        paintTex.SetPixels32(pixels);
        paintTex.Apply(false, false);
        dirty = false;
    }

    // ---------- Helpers ----------

    public bool WorldToUV(Vector3 world, out Vector2 uv)
    {
        uv = default;
        if (!col) return false;

        // raggio dall'alto verso il basso (top-down)
        var ray = new Ray(world + Vector3.up * 10f, Vector3.down);
        if (col.Raycast(ray, out var hit, 50f))
        {
            uv = hit.textureCoord; // 0..1
            return true;
        }
        return false;
    }

    public Vector2Int UVToCell(Vector2 uv)
    {
        int cx = Mathf.Clamp(Mathf.FloorToInt(uv.x * cellsX), 0, cellsX - 1);
        int cy = Mathf.Clamp(Mathf.FloorToInt(uv.y * cellsY), 0, cellsY - 1);
        return new Vector2Int(cx, cy);
    }

    private int Index(int x, int y) => y * cellsX + x;

    // ---------- API di pittura ----------

    /// Colora una singola cella.
    public void PaintCell(int cx, int cy, Color colr)
    {
        if ((uint)cx >= cellsX || (uint)cy >= cellsY) return;
        pixels[Index(cx, cy)] = (Color32)colr;
        dirty = true;
    }

    /// Colora una singola cella da UV.
    public void PaintCellAtUV(Vector2 uv, Color colr)
    {
        var c = UVToCell(uv);
        PaintCell(c.x, c.y, colr);
    }

    /// Cerchio su griglia in raggio celle (inclusivo: 0 = solo cella centrale).
    public void PaintCircleCells(int centerX, int centerY, int radiusCells, Color colr)
    {
        int r2 = radiusCells * radiusCells;
        for (int y = centerY - radiusCells; y <= centerY + radiusCells; y++)
        {
            if ((uint)y >= cellsY) continue;
            for (int x = centerX - radiusCells; x <= centerX + radiusCells; x++)
            {
                if ((uint)x >= cellsX) continue;
                int dx = x - centerX, dy = y - centerY;
                if (dx*dx + dy*dy <= r2) pixels[Index(x, y)] = (Color32)colr;
            }
        }
        dirty = true;
    }

    public void PaintCircleAtUV(Vector2 uv, int radiusCells, Color colr)
    {
        var c = UVToCell(uv);
        PaintCircleCells(c.x, c.y, radiusCells, colr);
    }

    /// Rettangolo allineato alla griglia.
    public void PaintRectCells(int xMin, int yMin, int xMax, int yMax, Color colr)
    {
        xMin = Mathf.Clamp(xMin, 0, cellsX - 1);
        yMin = Mathf.Clamp(yMin, 0, cellsY - 1);
        xMax = Mathf.Clamp(xMax, 0, cellsX - 1);
        yMax = Mathf.Clamp(yMax, 0, cellsY - 1);

        for (int y = yMin; y <= yMax; y++)
            for (int x = xMin; x <= xMax; x++)
                pixels[Index(x, y)] = (Color32)colr;

        dirty = true;
    }

    /// Cancella tutto (trasparente).
    public void ClearAll()
    {
        for (int i = 0; i < pixels.Length; i++) pixels[i] = new Color32(0, 0, 0, 0);
        dirty = true;
    }
}
