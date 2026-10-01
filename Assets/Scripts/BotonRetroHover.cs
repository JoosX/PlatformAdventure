using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class BotonRetroHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
{
    private GameObject flechaIzqObj;
    private GameObject flechaDerObj;

    [Header("Separación de las flechas hacia afuera del botón")]
    [SerializeField] private float distanciaExterior = 25f;

    void Awake()
    {
        RectTransform rtBoton = GetComponent<RectTransform>();
        TextMeshProUGUI textoHijo = GetComponentInChildren<TextMeshProUGUI>();

        float mitadAncho = rtBoton != null ? (rtBoton.rect.width / 2f) : 110f;
        float posicionX = mitadAncho + distanciaExterior;

        // Crea la flecha izquierda
        flechaIzqObj = CrearObjetoFlecha("FlechaIzquierda", "◄", -posicionX, textoHijo);

        // Crea la flecha derecha
        flechaDerObj = CrearObjetoFlecha("FlechaDerecha", "►", posicionX, textoHijo);

        // Inician apagadas
        SetFlechas(false);
    }

    private GameObject CrearObjetoFlecha(string nombre, string simbolo, float posX, TextMeshProUGUI textoBase)
    {
        GameObject obj = new GameObject(nombre);
        obj.transform.SetParent(this.transform, false);

        RectTransform rt = obj.AddComponent<RectTransform>();
        rt.sizeDelta = new Vector2(30, 30);
        rt.anchoredPosition = new Vector2(posX, 0);

        TextMeshProUGUI tmp = obj.AddComponent<TextMeshProUGUI>();
        tmp.text = simbolo;
        tmp.alignment = TextAlignmentOptions.Center;
        tmp.raycastTarget = false; // Para no interferir con los clics

        if (textoBase != null)
        {
            tmp.font = textoBase.font;
            tmp.color = textoBase.color;
            tmp.fontSize = textoBase.fontSize;
        }
        else
        {
            tmp.color = Color.white;
            tmp.fontSize = 24;
        }

        return obj;
    }

    public void OnPointerEnter(PointerEventData eventData) => SetFlechas(true);
    public void OnPointerExit(PointerEventData eventData) => SetFlechas(false);
    public void OnSelect(BaseEventData eventData) => SetFlechas(true);
    public void OnDeselect(BaseEventData eventData) => SetFlechas(false);

    private void SetFlechas(bool activa)
    {
        if (flechaIzqObj != null) flechaIzqObj.SetActive(activa);
        if (flechaDerObj != null) flechaDerObj.SetActive(activa);
    }

    void OnDisable()
    {
        SetFlechas(false);
    }
}