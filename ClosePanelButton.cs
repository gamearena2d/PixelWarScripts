using UnityEngine;

public class ClosePanelButton : MonoBehaviour
{
    [Tooltip("Reference to the panel that will be closed.")]
    [SerializeField] private GameObject panelToClose;

    public void Close()
    {
        if (panelToClose != null)
        {
            panelToClose.SetActive(false);
        }
        else
        {
            Debug.LogWarning("ClosePanelButton: panelToClose is not assigned.");
        }
    }
}
