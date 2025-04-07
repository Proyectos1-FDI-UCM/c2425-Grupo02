//---------------------------------------------------------
// Script que gestiona el comportamiento de los pilares en la fase 1 del combate.
//
// Adrián Arbas Perdiguero
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System; // Biblioteca base de C# (aunque aquí no se usa realmente).
using UnityEngine; // Necesario para trabajar con Unity (GameObjects, Transform, etc.).

/// <summary>
/// Este script representa un pilar en la fase 1 del combate.
/// Cada pilar tiene vida, un identificador y está vinculado al jefe.
/// Cuando un pilar es destruido, activa la vulnerabilidad del jefe.
/// </summary>
public class Pillar : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] int PillarHealth; // Vida total del pilar.
    [SerializeField] int PillarID; // Identificador único del pilar.
    [SerializeField] GameObject Boss; // Referencia al jefe en la escena.

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    // No hay atributos privados en este momento, pero podrían añadirse si es necesario.

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se ejecuta cuando el objeto se activa en la escena.
    /// Encuentra automáticamente al jefe en la escena y lo almacena en la variable `Boss`.
    /// </summary>
    void Start()
    {
        Boss = FindObjectOfType<Boss_Life_Phase1>().gameObject; // Busca y guarda el jefe en la variable Boss.
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Reduce la vida del pilar cuando recibe daño.
    /// Si el pilar es destruido, notifica al jefe para que se vuelva vulnerable.
    /// </summary>
    /// <param name="dmg">Cantidad de daño recibido.</param>
    public void Damage(int dmg)
    {
        // Obtenemos si el jefe es vulnerable en este momento.
        bool _isVulnerable = Boss.GetComponent<Boss_Life_Phase1>().getIsVulnerable();

        // Solo se puede dañar el pilar si el jefe NO es vulnerable.
        if (!_isVulnerable)
        {
            // Reducimos la vida del pilar.
            PillarHealth -= dmg;

            // Si la vida del pilar llega a 0, se destruye.
            if (PillarHealth <= 0)
            {
                // Informamos al jefe de que este pilar ha sido destruido.
                Boss.GetComponent<Boss_Life_Phase1>().SetPillarBool(PillarID);

                // Destruimos el GameObject del pilar.
                Destroy(gameObject);
            }
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    // Actualmente no hay métodos privados en este script.

    #endregion

} // Fin de la clase Pillar
