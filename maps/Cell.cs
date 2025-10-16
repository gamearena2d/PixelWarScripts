using UnityEngine;

public enum CellState { Neutral, Player, Enemy }

public class Cell : MonoBehaviour
{
    public new Renderer renderer; // "new" evita il warning su Component.renderer
    public CellState state = CellState.Neutral;

    private void Awake()
    {
        if (renderer == null)
            renderer = GetComponent<Renderer>();
        UpdateColor(1f);
    }

    public void SetState(CellState newState, float intensity = 1f)
    {
        state = newState;
        UpdateColor(intensity);
    }

    private void UpdateColor(float intensity)
    {
        if (renderer == null) return;

        switch (state)
        {
            case CellState.Player:
                renderer.material.color = Color.blue * intensity;
                break;
            case CellState.Enemy:
                renderer.material.color = Color.red * intensity;
                break;
            case CellState.Neutral:
                renderer.material.color = Color.gray;
                break;
        }
    }
}
