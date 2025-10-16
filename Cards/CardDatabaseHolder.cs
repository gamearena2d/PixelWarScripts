using UnityEngine;

public class CardDatabaseHolder : MonoBehaviour
{
    public CardDatabase cardDatabase;

    void Awake()
    {
        if (cardDatabase == null)
            cardDatabase = Resources.Load<CardDatabase>("CardDatabase");

        if (cardDatabase != null)
            cardDatabase.LoadAllCards();
    }
}
