//---------------------------------------------------------
// Script que gestiona los ataques del jefe en la fase 1.
//
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Clase que maneja los ataques del jefe en la fase 1.
/// - Persigue al jugador girando hacia él.
/// - Dispara proyectiles si no es vulnerable.
/// </summary>
public class Boss_Attacks_Phase1 : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] Transform target; // Referencia al jugador.
    [SerializeField] float spinSpeed = 0.002f; // Velocidad de rotación del jefe.

    [SerializeField] GameObject bossProyectile; // Prefab del proyectil del jefe.
    [SerializeField] Transform firePosition; // Posición desde la que se disparan los proyectiles.

    [SerializeField] float fireRate; // Tiempo entre disparos.
    [SerializeField] GameObject Boss; // Referencia al jefe en la escena.

    [SerializeField] GameObject Spawner; // Referencia al spawner de enemigos.

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private float timeToFire; // Controla el tiempo entre disparos.
    private Rigidbody2D rb; // Referencia al Rigidbody2D del jefe.

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se ejecuta al inicio del objeto.
    /// - Obtiene el Rigidbody2D del jefe.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Busca el Rigidbody2D adjunto al jefe.
    }

    /// <summary>
    /// Se ejecuta en cada frame.
    /// - Si no tiene un objetivo, lo busca.
    /// - Si tiene un objetivo, rota hacia él.
    /// - Si el jefe no es vulnerable, dispara proyectiles.
    /// </summary>
    void Update()
    {
        if (!target) // Si no hay objetivo asignado, lo busca.
        {
            GetTarget();
        }
        else
        {
            RotateTowardsTarget(); // Rota hacia el jugador.
        }

        // Verifica si el jefe es vulnerable.
        bool _isVulnerable = Boss.GetComponent<Boss_Life_Phase1>().getIsVulnerable();

        if (!_isVulnerable) // Si el jefe no es vulnerable, dispara.
        {
            Shoot();
        }

        int BoosLife = Boss.GetComponent<Boss_Life_Phase1>().getBossLife();

        if (BoosLife <= 0)
        {
            Spawner.SetActive(false);
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
        if (timeToFire <= 0f) // Si el tiempo ha llegado a 0, dispara.
        {
            Debug.Log("Disparo"); // Mensaje en consola para depuración.
            Instantiate(bossProyectile, firePosition.position, firePosition.rotation); // Crea un proyectil.
            timeToFire = fireRate; // Reinicia el contador de tiempo para el siguiente disparo.
        }
        else
        {
            timeToFire -= Time.deltaTime; // Reduce el tiempo hasta el siguiente disparo.
        }
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

    /// <summary>
    /// Busca y asigna al jugador como objetivo.
    /// - Usa la etiqueta "Player" para encontrar al jugador en la escena.
    /// </summary>
    private void GetTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            target = player.transform;
        }
    }

    #endregion
} // Fin de la clase Boss_Attacks_Phase1

