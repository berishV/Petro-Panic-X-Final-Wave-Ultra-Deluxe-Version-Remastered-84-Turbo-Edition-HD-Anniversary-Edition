using UnityEngine;
public class GestorOleadas : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject enemigoGrandePrefab;

    [Header("Formación")]
    [SerializeField] private int columnas = 4;
    [SerializeField] private float separacionX = 2.5f;
    [SerializeField] private float alturaFormacion = 3f;
    [SerializeField] private float alturaSalida = 9f;

    [Header("Oleadas")]
    [SerializeField] private float tiempoEntreOleadas = 8f;
    private float cronometroOleada;

    void Start()
    {
        SpawnearOleada();
        cronometroOleada = tiempoEntreOleadas;
    }

    void Update()
    {
        cronometroOleada -= Time.deltaTime;
        if (cronometroOleada <= 0f)
        {
            SpawnearOleada();
            cronometroOleada = tiempoEntreOleadas;
        }
    }

    void SpawnearOleada()
    {
        float anchoTotal = (columnas - 1) * separacionX;
        float startX = -anchoTotal / 2f;

        for (int i = 0; i < columnas; i++)
        {
            float x = startX + i * separacionX;

            Vector3 spawnPos = new Vector3(x, alturaSalida, 0f);
            GameObject enemigo = Instantiate(enemigoGrandePrefab, spawnPos, Quaternion.identity);

            Vector3 posFormacion = new Vector3(x, alturaFormacion, 0f);
            ControladorEnemigoIA ia = enemigo.GetComponent<ControladorEnemigoIA>();
            if (ia != null)
            {
                ia.AsignarPosicionFormacion(posFormacion);
            }
        }
    }
}