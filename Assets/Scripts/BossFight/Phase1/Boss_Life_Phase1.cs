//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Boss_Life_Phase1 : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    [SerializeField] int BossHealth; //Vida del Boss

    [SerializeField] bool Pillar1;
    [SerializeField] bool Pillar2;
    [SerializeField] bool Pillar3;
    [SerializeField] bool Pillar4;

    [SerializeField] Vector2 CenterPosition;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private bool _isVulnerable = false;

    private int HitCount = 0;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        Pillar1 = false;
        Pillar2 = false;
        Pillar3 = false;
        Pillar4 = false;

        HitCount = 0;

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {

    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    public void SetPillarBool(int pillar)
    {
        Debug.Log($"Se ha llamado a SetPillarBool con pilar {pillar}");

        if (pillar == 1) Pillar1 = true;
        else if (pillar == 2) Pillar2 = true;
        else if (pillar == 3) Pillar3 = true;
        else if (pillar == 4) Pillar4 = true;

        _isVulnerable = true;

        Debug.Log($"Después de ejecutar SetPillarBool, isVulnerable = {_isVulnerable}");
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    public void Damage(int dmg)
    {
        Debug.Log("El jefe ha detectado colisión.");

        if (_isVulnerable == true)
        {
            Debug.Log("El jefe ha sido golpeado");
            BossHealth -= dmg;
            HitCount++;
            if (BossHealth <= 0)
            {
                Destroy(gameObject);
            }

            if (HitCount == 5)
            {
                _isVulnerable = false;
                HitCount = 0;
            }
        }

        else Debug.Log("El jefe no es vulnerable en este momento.");

    }

    private void TeleportBossToCenter()
    {
        transform.position = new Vector3(CenterPosition.x, CenterPosition.y, transform.position.z);
        Debug.Log("El jefe se ha teletransportado al centro de la sala.");
    }

    #endregion

} // class Boss_Life_Phase1 
// namespace
