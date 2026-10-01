using System.Collections;
using UnityEngine;

public class PlayerFlash : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Color colorOriginal;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            colorOriginal = spriteRenderer.color;
        }
    }

    public void EjecutarFlash(float duracion)
    {
        if (spriteRenderer != null)
        {
            StopAllCoroutines();
            StartCoroutine(RutinaFlash(duracion));
        }
    }

    private IEnumerator RutinaFlash(float duracion)
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(duracion);
        spriteRenderer.color = colorOriginal;
    }
}