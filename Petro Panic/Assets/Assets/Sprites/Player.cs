using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 12f;

    public float limiteMinX = -5f;
    public float limiteMaxX = 5f;
    public float limiteMinY = -7f;
    public float limiteMaxY = 4f;

    [Header("Configuración de Dash")]
    public float velocidadDash = 30f;
    public float duracionDash = 0.15f;
    public float cooldownDash = 1f;

    public Color colorDash = Color.red;
    private Color colorOriginal = Color.white;

    private bool estaEnDash = false;
    private float tiempoDestinadoDash;
    private float tiempoSiguienteDash;
    private Vector3 direccionDash;

    [Header("Configuración de Disparo")]
    public GameObject prefabMisil;
    public Transform puntoDeDisparo;
    public float cadenciaDeDisparo = 0.2f;
    private float proximoDisparo = 0f;

    [Header("Audio")]
    public AudioClip sonidoDisparo;
    private AudioSource audioSource;
    [Range(0f, 1f)] public float volumenDisparo = 0.3f;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        if (estaEnDash)
        {
            ContinuarDash();
            return;
        }

        MoverJugador();
        ProcesarEntradaDash();
        Disparar();
    }

    void MoverJugador()
    {
        float movimientoX = Input.GetAxisRaw("Horizontal");
        float movimientoY = Input.GetAxisRaw("Vertical");

        Vector3 movimiento = new Vector3(movimientoX, movimientoY, 0f).normalized;
        transform.Translate(movimiento * velocidad * Time.deltaTime);

        RestringirPosicion();
    }

    void ProcesarEntradaDash()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && Time.time >= tiempoSiguienteDash)
        {
            float movimientoX = Input.GetAxisRaw("Horizontal");
            float movimientoY = Input.GetAxisRaw("Vertical");

            if (movimientoX != 0 || movimientoY != 0)
            {
                estaEnDash = true;
                tiempoDestinadoDash = Time.time + duracionDash;
                tiempoSiguienteDash = Time.time + cooldownDash;
                direccionDash = new Vector3(movimientoX, movimientoY, 0f).normalized;

                if (spriteRenderer != null)
                {
                    spriteRenderer.color = colorDash;
                }
            }
        }
    }

    void ContinuarDash()
    {
        transform.Translate(direccionDash * velocidadDash * Time.deltaTime);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, colorOriginal, (Time.deltaTime / duracionDash));
        }

        RestringirPosicion();

        if (Time.time >= tiempoDestinadoDash)
        {
            estaEnDash = false;

            if (spriteRenderer != null)
            {
                spriteRenderer.color = colorOriginal;
            }
        }
    }

    void RestringirPosicion()
    {
        float posicionXRestringida = Mathf.Clamp(transform.position.x, limiteMinX, limiteMaxX);
        float posicionYRestringida = Mathf.Clamp(transform.position.y, limiteMinY, limiteMaxY);

        transform.position = new Vector3(posicionXRestringida, posicionYRestringida, transform.position.z);
    }

    void Disparar()
    {
        if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Fire1"))
        {
            if (Time.time > proximoDisparo)
            {
                proximoDisparo = Time.time + cadenciaDeDisparo;
                Instantiate(prefabMisil, puntoDeDisparo.position, Quaternion.identity);

                if (sonidoDisparo != null)
                {
                    audioSource.PlayOneShot(sonidoDisparo, volumenDisparo);
                }
            }
        }
    }
}