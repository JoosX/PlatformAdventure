using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine; // Si te marca error de compilación en esta línea, cámbiala por: using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 6f;
    public float jumpForce = 11f;

    [Header("Detección de Suelo")]
    public Transform groundCheck;
    public LayerMask groundLayer;

    // Componentes internos
    Rigidbody2D rb;
    Animator anim;
    CinemachineImpulseSource impulse;
    bool isGrounded;
    float x;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        impulse = GetComponent<CinemachineImpulseSource>();
    }

    void Update()
    {
        // 1. Lectura de teclas A/D y flechas
        x = 0f;
        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) x -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) x += 1f;

            // Giro del personaje según la dirección en que camina
            if (x > 0f)
            {
                transform.localScale = new Vector3(1f, 1f, 1f); // Mirar a la derecha
            }
            else if (x < 0f)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f); // Mirar a la izquierda
            }

            // 2. Comprobar si toca el suelo
            if (groundCheck != null)
            {
                isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.25f, groundLayer);
            }

            // 3. Salto + Disparo del Screen Shake
            if (Keyboard.current.spaceKey.wasPressedThisFrame && isGrounded)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

                // Activa la sacudida de pantalla si el componente existe
                if (impulse != null)
                {
                    impulse.GenerateImpulse(0.5f);
                }
            }
        }

        // 4. Parámetros para la animación
        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(x));
            anim.SetBool("IsGrounded", isGrounded);
            anim.SetFloat("yVelocity", rb.linearVelocity.y);
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(x * speed, rb.linearVelocity.y);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
        {
            PlayerHealth health = GetComponent<PlayerHealth>();
            if (health != null)
            {
                health.RecibirDanio(1); // Quita 1 corazón por impacto
            }
        }
    }
}