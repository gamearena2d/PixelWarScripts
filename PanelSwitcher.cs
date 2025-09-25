using UnityEngine;

public class PanelSwitcher : MonoBehaviour
{
    // Riferimenti ai pannelli (array per gestione dinamica)
    public GameObject[] panels;

    // Funzione per mostrare solo un pannello alla volta
    public void ShowPanel(int panelIndex)
    {
        // Nasconde tutti i pannelli
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }

        // Mostra solo il pannello selezionato
        panels[panelIndex].SetActive(true);
    }

    // Funzione per impostare il pannello di default (Home)
    void Start()
    {
        ShowPanel(0); // Mostra il primo pannello all'accesso (Home)
    }
}