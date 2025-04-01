//---------------------------------------------------------
// Script que gestiona la vida y vulnerabilidad del jefe en la fase 1 del combate.
//
// Adrián Arbas Perdiguero
// ASTRA DAMNATORUM
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine; // Necesario para usar las clases y funciones de Unity.

/// <summary>
/// Controla la vida y la vulnerabilidad del jefe en la fase 1 del combate.
/// Se vuelve vulnerable al activar ciertos pilares y puede recibir daño.
/// </summary>
public class Boss_Life_Phase1 : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] int BossHealth; // Vida total del jefe.

    // Variables booleanas que indican si los pilares están activados.
    [SerializeField] bool Pillar1;
    [SerializeField] bool Pillar2;
    [SerializeField] bool Pillar3;
    [SerializeField] bool Pillar4;

    [SerializeField] Vector2 CenterPosition; // Posición central a la que se teletransportará el jefe.

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private bool _isVulnerable = false; // Indica si el jefe puede recibir daño.
    private int HitCount = 0; // Cuenta la cantidad de golpes que ha recibido.

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se ejecuta al inicio del juego o cuando el objeto se activa.
    /// Inicializa los valores de los pilares y el contador de golpes.
    /// </summary>
    void Start()
    {
        // Inicializamos los pilares como desactivados.
        Pillar1 = false;
        Pillar2 = false;
        Pillar3 = false;
        Pillar4 = false;

        // Reiniciamos la cantidad de golpes recibidos.
        HitCount = 0;
    }

    /// <summary>
    /// Se ejecuta en cada frame del juego.
    /// (Actualmente no hace nada, pero podría usarse para futuras mecánicas del jefe).
    /// </summary>
    void Update()
    {

    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Activa el pilar correspondiente y hace que el jefe se vuelva vulnerable.
    /// </summary>
    /// <param name="pillar">Número del pilar a activar (1-4).</param>
    public void SetPillarBool(int pillar)
    {
        Debug.Log($"Se ha llamado a SetPillarBool con pilar {pillar}");

        // Activamos el pilar correspondiente según el número recibido.
        if (pillar == 1) Pillar1 = true;
        else if (pillar == 2) Pillar2 = true;
        else if (pillar == 3) Pillar3 = true;
        else if (pillar == 4) Pillar4 = true;

        // Cuando se activa un pilar, el jefe se vuelve vulnerable.
        _isVulnerable = true;

        Debug.Log($"Después de ejecutar SetPillarBool, isVulnerable = {_isVulnerable}");
    }

    /// <summary>
    /// Devuelve si el jefe es vulnerable o no.
    /// </summary>
    /// <returns>True si el jefe es vulnerable, False si no lo es.</returns>
    public bool getIsVulnerable()
    {
        return _isVulnerable;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Reduce la vida del jefe si es vulnerable.
    /// Si la vida llega a 0, destruye al jefe.
    /// Además, después de recibir 5 golpes, se vuelve invulnerable temporalmente.
    /// </summary>
    /// <param name="dmg">Cantidad de daño recibido.</param>
    public void Damage(int dmg)
    {
        Debug.Log("El jefe ha detectado colisión.");

        // Solo recibe daño si es vulnerable.
        if (_isVulnerable == true)
        {
            Debug.Log("El jefe ha sido golpeado");

            // Reducimos la vida del jefe según el daño recibido.
            BossHealth -= dmg;

            // Aumentamos el contador de golpes.
            HitCount++;

            // Si la vida del jefe llega a 0, lo destruimos.
            if (BossHealth <= 0)
            {
                Destroy(gameObject);
            }

            // Si recibe 5 golpes, deja de ser vulnerable y se reinicia el contador.
            if (HitCount == 5)
            {
                _isVulnerable = false;
                HitCount = 0;
            }
        }
        else
        {
            Debug.Log("El jefe no es vulnerable en este momento.");
        }
    }

    /// <summary>
    /// Teletransporta al jefe al centro de la sala.
    /// </summary>
    private void TeleportBossToCenter()
    {
        // Cambia la posición del jefe al centro.
        transform.position = new Vector3(CenterPosition.x, CenterPosition.y, transform.position.z);

        Debug.Log("El jefe se ha teletransportado al centro de la sala.");
    }

    #endregion

} // Fin de la clase Boss_Life_Phase1
