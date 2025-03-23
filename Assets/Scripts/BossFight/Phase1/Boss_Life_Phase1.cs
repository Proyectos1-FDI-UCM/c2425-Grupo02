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

    [SerializeField] Vector3 CenterPosition;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private bool _pillar1Destroyed;
    private bool _pillar2Destroyed;
    private bool _pillar3Destroyed;
    private bool _pillar4Destroyed;

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

        _pillar1Destroyed = false;
        _pillar2Destroyed = false;
        _pillar3Destroyed = false;
        _pillar4Destroyed = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        OnPillarDestroy();
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
        switch (pillar)
        {
            case 1:
                Pillar1 = true;
                Debug.Log("Pillar 1 destroyed");
                break;
            case 2:
                Pillar2 = true;
                Debug.Log("Pillar 2 destroyed");
                break;
            case 3:
                Pillar3 = true;
                Debug.Log("Pillar 3 destroyed");
                break;
            case 4:
                Pillar4 = true;
                Debug.Log("Pillar 4 destroyed");
                break;
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    private void OnPillarDestroy()
    {
        if (Pillar1 && !_pillar1Destroyed)
        {
            _pillar1Destroyed = true;
            TeleportBossToCenter();
        }
        else if (Pillar2 && !_pillar2Destroyed)
        {
            _pillar2Destroyed = true;
            TeleportBossToCenter();
        }
        else if (Pillar3 && !_pillar3Destroyed)
        {
            _pillar3Destroyed = true;
            TeleportBossToCenter();
        }
        else if (Pillar4 && !_pillar4Destroyed)
        {
            _pillar4Destroyed = true;
            TeleportBossToCenter();
        }
    }

    private void TeleportBossToCenter()
    {
        transform.position = CenterPosition;
        Debug.Log("El jefe se ha teletransportado al centro de la sala.");
    }
    #endregion

} // class Boss_Life_Phase1 
// namespace
