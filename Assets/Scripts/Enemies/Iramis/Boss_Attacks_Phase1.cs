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
public class Boss_Attacks_Phase1 : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] Transform target; // Referencia al jugador.
    [SerializeField] float spinSpeed = 0.002f; // Velocidad de rotación del jefe.

    [SerializeField] GameObject bossProyectile; // Prefab del proyectil del jefe.
    [SerializeField] Transform firePosition; // Posición desde la que se disparan los proyectiles.

    [SerializeField] float fireRate; // Tiempo entre disparos.
    [SerializeField] float RateSpawn; // Tiempo entre apariciones de enemigos.
    [SerializeField] GameObject Boss; // Referencia al jefe en la escena.

    [SerializeField] GameObject Spawner; // Referencia al spawner de enemigos.

    [SerializeField] GameObject Wall; // Referencia a las paredes que se invocan al comenzar la fase.

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private float timeToFire = 0f; // Controla el tiempo entre disparos.
    private float timeToSpawn; // Controla el tiempo entre la aparición de enemigos.
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
        rb = GetComponent<Rigidbody2D>(); // Busca el Rigidbody2D adjunto al jefe.
        _player = FindObjectOfType<Movement>().gameObject;

        for (int i = 0; i < 12; i++)
        {
            Instantiate(Wall, new Vector3(-5.5f + i, 107f, 0f), Quaternion.identity);
        }

        for (int i = 0; i < 36; i++)
        {
            Instantiate(Wall, new Vector3(-17.5f + i, 164f, 0f), Quaternion.identity);
        }
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

        if (timeToFire <= 0f) // Si el tiempo ha llegado a 0, dispara.
        {
            Shoot(); // Llama al método de disparo.
            timeToFire = fireRate; // Reinicia el contador de tiempo para el siguiente disparo.
        }
        else
        {
            timeToFire -= Time.deltaTime; // Reduce el tiempo hasta el siguiente disparo.
        }

        if (timeToSpawn <= 0f) // Si el tiempo ha llegado a 0, spawnea.
        {
            Instantiate(Spawner); // Spawnea el prefab.
            timeToSpawn = RateSpawn; // Reinicia el contador de tiempo para el siguiente disparo.
        }
        else
        {
            timeToSpawn -= Time.deltaTime; // Reduce el tiempo hasta el siguiente disparo.
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
        _isVulnerable = Boss.GetComponent<Boss_Life_Phase1>().getIsVulnerable();
        if (_isVulnerable) // Si el jefe es vulnerable, no dispara.
        {
            return; // Sale del método sin hacer nada.
        }

        Instantiate(bossProyectile, firePosition.position, firePosition.rotation); // Crea un proyectil.

        TripleShot = Boss.GetComponent<Boss_Life_Phase1>().SetTripleShotOn();

        if (TripleShot == true)
        {
            // Crea el segundo proyectil, alejado y girado 15º
            Vector3 offset = new Vector3(0.5f, 0, 0); // Ajusta el valor de offset según sea necesario
            Quaternion rotation = Quaternion.Euler(firePosition.rotation.eulerAngles + new Vector3(0, 0, 15));
            Instantiate(bossProyectile, firePosition.position + offset, rotation);

            // Crea el tercer proyectil, alejado en el otro sentido y con una inclinación de -15º
            rotation = Quaternion.Euler(firePosition.rotation.eulerAngles + new Vector3(0, 0, -15));
            Instantiate(bossProyectile, firePosition.position - offset, rotation);
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
        if (_player != null)
        {
            target = _player.transform;
        }
    }


    #endregion
} // Fin de la clase Boss_Attacks_Phase1

