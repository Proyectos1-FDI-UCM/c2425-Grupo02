//---------------------------------------------------------
// Script que gestiona el comportamiento del enemigo 3.
//
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Clase que gestiona todo el comportamiento.
/// - Persigue al jugador si está muy lejos.
/// - Se aleja de él si está muy cerca.
/// - Dispara si está dentro del rango para poder disparar.
/// </summary>
public class Enemy3 : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] Transform target; // Referencia al jugador.
    [SerializeField] float spinSpeed = 1f; // Velocidad de rotación del enemigo.
    [SerializeField] float distanciaMinima = 5f; // Distancia entre el jugador y el enemigo para que el enemigo huya.
    [SerializeField] float distanciaMaxima = 10f; // Distancia entre el jugador y el enemigo para que el enemigo tenga que perseguir al jugador.
    [SerializeField] float movementSpeed = 2f; //Velocidad de movimiento del enemigo.
    [SerializeField] GameObject proyectile; // Prefab del proyectil.
    [SerializeField] Transform firePosition; // Posición desde la que se disparan los proyectiles.
    [SerializeField] float fireRate; // Tiempo entre disparos.
    [SerializeField] GameObject Enemy; // Referencia al enemigo en la escena.

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private float timeToFire = 0f; // Controla el tiempo entre disparos.
    private Rigidbody2D rb; // Referencia al Rigidbody2D del enemigo.
    private GameObject _player; // Referencia al jugador.

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se ejecuta al inicio del objeto.
    /// - Obtiene el Rigidbody2D del enemigo.
    /// </summary>
    void Start()
    {
        timeToFire = fireRate; // Inicializa el tiempo de disparo.
        rb = GetComponent<Rigidbody2D>(); // Busca el Rigidbody2D adjunto al enemigo.
        _player = FindObjectOfType<Movement>().gameObject; // Identifica al jugador.
    }

    /// <summary>
    /// Se ejecuta en cada frame.
    /// - Si está muy cerca del jugador, se aleja.
    /// - Si está muy lejos, se acerca.
    /// - Si no tiene un objetivo, lo busca.
    /// - Si tiene un objetivo, rota hacia él.
    /// - Si está lo suficientemente cerca, disparará proyectiles.
    /// </summary>
    void Update()
    {
        float distanciaAlJugador = Vector2.Distance(transform.position, target.position);
        if (distanciaAlJugador < distanciaMinima) // Si el jugador está muy cerca, se aleja de él.
        {
            AlejarseDelJugador();
        }
        else if (distanciaAlJugador > distanciaMaxima) // Si el jugador está muy lejos, se acerca a él.
        {
            AcercarseAlJugador();
        }
        if (!target) // Si no hay objetivo asignado, lo busca.
        {
            GetTarget();
        }
        else
        {
            RotateTowardsTarget(); // Rota hacia el jugador.
        }

        if (timeToFire <= 0f && distanciaAlJugador < distanciaMaxima) // Si el tiempo ha llegado a 0 y está lo suficientemente cerca, dispara.
        {
            Shoot(); // Llama al método de disparo.
            timeToFire = fireRate; // Reinicia el contador de tiempo para el siguiente disparo.
        }
        else
        {
            timeToFire -= Time.deltaTime; // Reduce el tiempo hasta el siguiente disparo.
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Controla el disparo de los proyectiles.
    /// - Dispara al ser llamado.
    /// </summary>
    private void Shoot()
    {
        Instantiate(proyectile, firePosition.position, firePosition.rotation); // Crea un proyectil.
    }

    /// <summary>
    /// Rota al enemigo en dirección al jugador.
    /// - Calcula la dirección hacia el jugador.
    /// - Usa interpolación (Slerp) para girar suavemente.
    /// </summary>
    private void RotateTowardsTarget()
    {
        Vector2 targetDirection = target.position - transform.position; // Calcula dirección al jugador.
        float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg - 90f; // Convierte a ángulo.
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle)); // Crea la rotación.
        transform.localRotation = Quaternion.Slerp(transform.rotation, q, spinSpeed); // Rota suavemente.
    }
    /// <summary>
    /// Aleja al enemigo del jugador.
    /// - Calcula la dirección opuesta al jugador.
    /// - Mueve al enemigo en esa dirección (para huir).
    /// </summary>
    void AlejarseDelJugador()
    {
        Vector2 direccionHuida = (transform.position - target.position).normalized; // Calcula la dirección opuesta al jugador.
        Enemy.transform.position += (Vector3)direccionHuida * movementSpeed * Time.deltaTime; // Mueve al enemigo en esa dirección.
    }
    /// <summary>
    /// Acerca al enemigo hacia al jugador.
    /// - Calcula la dirección al jugador.
    /// - Mueve al enemigo en esa dirección (para perseguirle).
    /// </summary>
    void AcercarseAlJugador()
    {
        Vector2 direccionAcercamiento = (target.position - transform.position).normalized; // Calcula la dirección hacia el jugador.
        Enemy.transform.position += (Vector3)direccionAcercamiento * movementSpeed * Time.deltaTime; // Mueve al enemigo en esa dirección.
    }
    /// <summary>
    /// Busca y asigna al jugador como objetivo.
    /// - Usa la etiqueta "Player" para encontrar al jugador en la escena.
    /// </summary>
    private void GetTarget()
    {
        if (_player != null)
        {
            target = _player.transform;
        }
    }


    #endregion
}// class Enemy3
// namespace

