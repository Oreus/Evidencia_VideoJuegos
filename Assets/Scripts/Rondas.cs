using UnityEngine;
using System.Collections;
using TMPro;

public class Rondas : MonoBehaviour
{
    public TMP_Text textoRonda;
    public TMP_Text textoEnemigos;
    public GameObject prefabEnemigo;
    public Transform[] puntosSpawn;
    public float tiempoEntreSpawns = 0.5f;
    private int rondaActual = 1;
    private int enemigosVivos = 0;
    private bool generandoRonda = false;

    void Start()
    {
        IniciarSiguienteRonda();
    }

    void IniciarSiguienteRonda()
    {
        int totalEnemigos = 5 + (rondaActual - 1) * 2;
        enemigosVivos = totalEnemigos;
                
        ActualizarUI();
        StartCoroutine(GenerarEnemigos(totalEnemigos));
    }

    IEnumerator GenerarEnemigos(int cantidad)
    {
        generandoRonda = true;

        for (int i = 0; i < cantidad; i++)
        {
            if (puntosSpawn.Length == 0) yield break;

            Transform spawnElegido = puntosSpawn[Random.Range(0, puntosSpawn.Length)];
            Instantiate(prefabEnemigo, spawnElegido.position, Quaternion.identity);

            yield return new WaitForSeconds(tiempoEntreSpawns);
        }

        generandoRonda = false;
    }

    public void EnemigoEliminado()
    {
        enemigosVivos--;
        ActualizarUI();

        if (enemigosVivos <= 0 && !generandoRonda)
        {
            rondaActual++;
            Invoke(nameof(IniciarSiguienteRonda), 2f);
        }
    }

    void ActualizarUI()
    {
        if (textoRonda != null)
        {
            textoRonda.text = "Ronda: " + rondaActual;
        }

        if (textoEnemigos != null)
        {
            textoEnemigos.text = "Enemigos: " + Mathf.Max(0, enemigosVivos);
        }
    }
}