using UnityEngine;
using UnityEngine.InputSystem;

public class CursorLuzController : MonoBehaviour
{
    private RectTransform rectTransform;
    private Canvas canvas;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    void OnEnable()
    {
        OcultarCursorSistema();
    }

    void OnDisable()
    {
        // Al reanudar o salir, devuelve el cursor normal
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void Update()
    {
        // Fuerza a mantener el cursor del  sistema oculto de inmediato
        if (Cursor.visible)
        {
            Cursor.visible = false;
        }

        if (Mouse.current != null)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            if (canvas != null && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            {
                rectTransform.position = mousePos;
            }
            else if (canvas != null)
            {
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    canvas.transform as RectTransform,
                    mousePos,
                    canvas.worldCamera,
                    out Vector2 localPoint
                );
                rectTransform.localPosition = localPoint;
            }
        }
    }

    private void OcultarCursorSistema()
    {
        Cursor.visible = false;
        
        Cursor.lockState = CursorLockMode.Confined;
    }
}