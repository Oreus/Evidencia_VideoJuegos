using UnityEngine;
using UnityEngine.SceneManagement;

public class MovimientoJugador : MonoBehaviour
{
    public float velocidad = 8f;
    public float fuerzaSalto = 12f;
    public int saltosMaximos = 2;
    private int saltosRestantes;
    public float cooldownAtaque = 0.5f;
    public Transform puntoAtaque;
    public float radioAtaque = 0.6f;
    public LayerMask capaEnemigos;
    private float tiempoSiguienteAtaque = 0f;
    public float delayImpacto = 0.2f;
    public Transform comprobadorSuelo;
    public float radioSuelo = 0.2f;
    public LayerMask capaSuelo;
    private Rigidbody2D rb;
    private Animator animator; 
    private float horizontal;
    private bool EnSuelo;
    private bool MirandoDerecha = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (horizontal > 0 && !MirandoDerecha)
        {
            Voltear();
        }
        else if (horizontal < 0 && MirandoDerecha)
        {
            Voltear();
        }

        EnSuelo = Physics2D.OverlapCircle(comprobadorSuelo.position, radioSuelo, capaSuelo);

        if (animator != null)
        {
            animator.SetBool("EnSuelo", EnSuelo);
            animator.SetFloat("Velocidad", Mathf.Abs(horizontal));
        }

        if (EnSuelo)
        {
            saltosRestantes = saltosMaximos;
        }

        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, fuerzaSalto);
            saltosRestantes--;
        }

        if (Time.time >= tiempoSiguienteAtaque)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.J))
            {
                Atacar();
                tiempoSiguienteAtaque = Time.time + cooldownAtaque;
            }
        }
    }

    void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(horizontal * velocidad, rb.linearVelocity.y);
    }

    void Atacar()
    {
        if (animator != null)
        {
            animator.SetTrigger("Atacar");
        }

        StartCoroutine(EjecutarGolpeConDelay());
    }

    System.Collections.IEnumerator EjecutarGolpeConDelay()
    {
        yield return new WaitForSeconds(delayImpacto);

        if (puntoAtaque == null) yield break;

        Collider2D[] enemigosGolpeados = Physics2D.OverlapCircleAll(puntoAtaque.position, radioAtaque, capaEnemigos);

        foreach (Collider2D enemigo in enemigosGolpeados)
        {
            Enemigo scriptEnemigo = enemigo.GetComponent<Enemigo>();
            if (scriptEnemigo != null)
            {
                scriptEnemigo.Morir();
            }   
        }
    }

    public void Morir()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void OnDrawGizmosSelected()
    {
        if (comprobadorSuelo != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(comprobadorSuelo.position, radioSuelo);
        }

        if (puntoAtaque != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(puntoAtaque.position, radioAtaque);
        }
    }

    void Voltear()
    {
        MirandoDerecha = !MirandoDerecha;

        Vector3 escala = transform.localScale;
        escala.x *= -1;
        transform.localScale = escala;
    }
}