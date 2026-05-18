using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Configuración de Movimiento")]
    public float velocidad = 5f;

    public float limiteMinX = -5f;
    public float limiteMaxX = 5f;
    public float limiteMinY = -4f;
    public float limiteMaxY = 4f;

    [Header("Configuración de Disparo")]
    public GameObject prefabMisil;
    public Transform puntoDeDisparo;
    public float cadenciaDeDisparo = 0.2f;
    private float proximoDisparo = 0f;

    [Header("Audio")]
    public AudioClip sonidoDisparo;
    private AudioSource audioSource;
    [Range(0f, 1f)] public float volumenDisparo = 0.5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        MoverJugador();
        Disparar();
    }

    void MoverJugador()
    {
        float movimientoX = Input.GetAxis("Horizontal");
        float movimientoY = Input.GetAxis("Vertical");

        Vector3 movimiento = new Vector3(movimientoX, movimientoY, 0f);
        transform.Translate(movimiento * velocidad * Time.deltaTime);

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