using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ClanSubTabColorController : MonoBehaviour
{
    [System.Serializable]
    public class Tab
    {
        public Button button;          // Il GameObject del bottone (con Image)
        public Image background;       // Se lasci vuoto, usa l'Image del bottone
        public TMP_Text label;         // (opzionale) il testo del bottone
    }

    [Header("Sottotab")]
    public Tab bacheca;
    public Tab cerca;
    public Tab preferiti;

    [Header("Colori")]
    public Color bgNormal = new Color(0.70f, 0.70f, 0.70f, 1f); // grigio
    public Color bgSelected = Color.white;                      // bianco
    public Color textNormal = new Color(0.15f, 0.15f, 0.15f, 1f);
    public Color textSelected = Color.black;

    private Tab current;

    void Awake()
    {
        InitTab(bacheca);
        InitTab(cerca);
        InitTab(preferiti);

        // Se vuoi, puoi lasciare questi listener: funzionano anche insieme ai tuoi OnClick dell'Inspector
        bacheca.button.onClick.AddListener(SelectBacheca);
        cerca.button.onClick.AddListener(SelectCerca);
        preferiti.button.onClick.AddListener(SelectPreferiti);
    }

    void OnEnable()
    {
        if (current == null) SelectBacheca();
        else ApplyColors();
    }

    void InitTab(Tab t)
    {
        if (t != null && t.background == null && t.button != null)
            t.background = t.button.targetGraphic as Image;
    }

    public void SelectBacheca()  { current = bacheca;  ApplyColors(); }
    public void SelectCerca()    { current = cerca;    ApplyColors(); }
    public void SelectPreferiti(){ current = preferiti;ApplyColors(); }

    public void SelectTabByButton(Button btn)
    {
        if (btn == bacheca.button) SelectBacheca();
        else if (btn == cerca.button) SelectCerca();
        else if (btn == preferiti.button) SelectPreferiti();
    }

    void ApplyColors()
    {
        SetTabColors(bacheca,   current == bacheca);
        SetTabColors(cerca,     current == cerca);
        SetTabColors(preferiti, current == preferiti);
    }

    void SetTabColors(Tab t, bool selected)
    {
        if (t == null) return;

        if (t.background) t.background.color = selected ? bgSelected : bgNormal;
        if (t.label)      t.label.color      = selected ? textSelected : textNormal;

        // Evita che l’hover sovrascriva il colore
        var cb = t.button.colors;
        cb.normalColor      = selected ? bgSelected : bgNormal;
        cb.highlightedColor = selected ? bgSelected : bgNormal;
        cb.pressedColor     = selected ? bgSelected : (bgNormal * 0.95f);
        cb.selectedColor    = selected ? bgSelected : bgNormal;
        t.button.colors = cb;
    }
}
