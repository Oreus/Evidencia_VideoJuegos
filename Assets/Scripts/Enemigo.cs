using UnityEngine;

public class Enemigo : MonoBehaviour
{
    public float velocidad = 3f;
    private Transform jugador;
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        MovimientoJugador scriptJugador = FindFirstObjectByType<MovimientoJugador>();
        if (scriptJugador != null)
        {
            jugador = scriptJugador.transform;
        }
    }

    void FixedUpdate()
    {
        if (jugador == null) return;

        float direccionX = Mathf.Sign(jugador.position.x - transform.position.x);

        if (Mathf.Abs(jugador.position.x - transform.position.x) > 0.3f)
        {
            rb.linearVelocity = new Vector2(direccionX * velocidad, rb.linearVelocity.y);

            transform.localScale = new Vector3(direccionX > 0 ? 1 : -1, 1, 1);
        }
        else
        {
            rb.linearVelocity = new Vector2(0, rb.linearVelocity.y);
        }
    }

    public void Morir()
    {
        Rondas gestor = FindFirstObjectByType<Rondas>();
        if (gestor != null)
        {
            gestor.EnemigoEliminado();
        }

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            MovimientoJugador playerScript = collision.gameObject.GetComponent<MovimientoJugador>();
            if (playerScript != null)
            {
                playerScript.Morir();
            }
        }
    }
}