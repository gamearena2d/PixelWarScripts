using UnityEngine;
using UnityEngine.UI;

public class BattlefieldCell : MonoBehaviour
{
    public enum Owner { Neutral, Player1, Player2 }
    public Owner cellOwner;

    private Image cellImage;

    void Start()
    {
        cellImage = GetComponent<Image>();
        UpdateColor(); // Inizializza il colore in base al proprietario
    }

    public void SetOwner(Owner newOwner)
    {
        cellOwner = newOwner;
        UpdateColor();
    }

    private void UpdateColor()
    {
        switch (cellOwner)
        {
            case Owner.Player1:
                cellImage.color = Color.blue;
                break;
            case Owner.Player2:
                cellImage.color = Color.red;
                break;
            default:
                cellImage.color = Color.gray;
                break;
        }
    }
}
