//---------------------------------------------------------
// Script que gestiona los ataques del jefe en la fase 1.
//
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Clase que maneja los ataques del jefe en la fase 1.
/// - Persigue al jugador girando hacia él.
/// - Dispara proyectiles si no es vulnerable.
/// </summary>
public class Enemy3 : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] Transform target; // Referencia al jugador.
    [SerializeField] float spinSpeed = 0.002f; // Velocidad de rotación del jefe.
    [SerializeField] float distanciaMinima = 5f; // Distancia entre el jugador y el enemigo para que el enemigo huya.
    [SerializeField] float distanciaMaxima = 10f; // Distancia entre el jugador y el enemigo para que el enemigo tenga que perseguir al jugador.
    [SerializeField] float movementSpeed = 2f; //Velocidad de movimiento del enemigo
    [SerializeField] GameObject proyectile; // Prefab del proyectil del jefe.
    [SerializeField] Transform firePosition; // Posición desde la que se disparan los proyectiles.

    [SerializeField] float fireRate; // Tiempo entre disparos.
    [SerializeField] GameObject Enemy; // Referencia al jefe en la escena.

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private float timeToFire = 0f; // Controla el tiempo entre disparos.
    private Rigidbody2D rb; // Referencia al Rigidbody2D del jefe.
    private GameObject _player; // Referencia al jugador.
    private int BoosLife; // Vida del jefe. 
    private bool _isVulnerable; // Indica si el jefe es vulnerable.
    private bool TripleShot; // Indica si el jefe lanza 3 proyectiles.



    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se ejecuta al inicio del objeto.
    /// - Obtiene el Rigidbody2D del jefe.
    /// </summary>
    void Start()
    {
        timeToFire = fireRate; // Inicializa el tiempo de disparo.
        rb = GetComponent<Rigidbody2D>(); // Busca el Rigidbody2D adjunto al jefe.
        _player = FindObjectOfType<Movement>().gameObject;
    }

    /// <summary>
    /// Se ejecuta en cada frame.
    /// - Si no tiene un objetivo, lo busca.
    /// - Si tiene un objetivo, rota hacia él.
    /// - Si el jefe no es vulnerable, dispara proyectiles.
    /// </summary>
    void Update()
    {
        float distanciaAlJugador = Vector2.Distance(transform.position, target.position);

        // Si el jugador está muy cerca, alejarse
        if (distanciaAlJugador < distanciaMinima)
        {
            AlejarseDelJugador();
        }
        // Si el jugador está muy lejos, acercarse
        else if (distanciaAlJugador > distanciaMaxima)
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

        if (timeToFire <= 0f && distanciaAlJugador < distanciaMaxima) // Si el tiempo ha llegado a 0, dispara.
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
    /// - Dispara si ha pasado suficiente tiempo desde el último disparo.
    /// </summary>
    private void Shoot()
    {
        Instantiate(proyectile, firePosition.position, firePosition.rotation); // Crea un proyectil.
    }

    /// <summary>
    /// Rota el jefe en dirección al jugador.
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
    void AlejarseDelJugador()
    {
        // Calcular dirección opuesta al jugador
        Vector2 direccionHuida = (transform.position - target.position).normalized;

        // Mover en esa dirección
        Enemy.transform.position += (Vector3)direccionHuida * movementSpeed * Time.deltaTime;
    }
    void AcercarseAlJugador()
    {
        // Calcular dirección hacia el jugador
        Vector2 direccionAcercamiento = (target.position - transform.position).normalized;

        // Mover hacia el jugador
        Enemy.transform.position += (Vector3)direccionAcercamiento * movementSpeed * Time.deltaTime;
    }
    void MirarHaciaJugador()
    {
        // Calcular dirección hacia el jugador
        Vector2 direccion = (target.position - transform.position).normalized;

        // Calcular ángulo y rotar
        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angulo, Vector3.forward);
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
} // Fin de la clase Boss_Attacks_Phase1

