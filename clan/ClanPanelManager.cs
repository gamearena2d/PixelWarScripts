using UnityEngine;

public class ClanPanelManager : MonoBehaviour
{
    [Header("Pannelli principali")]
    public GameObject profiloPanel;       // 1️⃣ Profilo
    public GameObject ilMioClanPanel;     // 2️⃣ Il mio clan
    public GameObject clanPanelContainer; // 3️⃣ Clan (contenitore con subtabs)

    private void Start()
    {
        // Mostra di default il pannello profilo
        ShowProfilo();
    }

    public void ShowProfilo()
    {
        profiloPanel.SetActive(true);
        ilMioClanPanel.SetActive(false);
        clanPanelContainer.SetActive(false);
    }

    public void ShowIlMioClan()
    {
        profiloPanel.SetActive(false);
        ilMioClanPanel.SetActive(true);
        clanPanelContainer.SetActive(false);
    }

    public void ShowClanContainer()
    {
        profiloPanel.SetActive(false);
        ilMioClanPanel.SetActive(false);
        clanPanelContainer.SetActive(true);
    }
}
