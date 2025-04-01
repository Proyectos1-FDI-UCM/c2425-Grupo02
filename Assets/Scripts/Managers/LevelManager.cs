//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Componente que se encarga de la gestión de un nivel concreto.
/// Este componente es un singleton, para que sea accesible para todos
/// los objetos de la escena, pero no tiene el comportamiento de
/// DontDestroyOnLoad, ya que solo vive en una escena.
///
/// Contiene toda la información propia de la escena y puede comunicarse
/// con el GameManager para transferir información importante para
/// la gestión global del juego (información que ha de pasar entre
/// escenas)
/// </summary>
public class LevelManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Ancho del mapa de la escena
    /// </summary>
    [SerializeField] float MapWidth;
    /// <summary>
    /// alto del mapa de la escena
    /// </summary>
    [SerializeField] float MapHeight;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;
    private GameObject _player;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    protected void Awake()
    {
        if (_instance == null)
        {
            // Somos la primera y única instancia
            _instance = this;
            Init();
        }

        _player = FindObjectOfType<Player_Health>().gameObject;

    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static LevelManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Devuelve cierto si la instancia del singleton está creada y
    /// falso en otro caso.
    /// Lo normal es que esté creada, pero puede ser útil durante el
    /// cierre para evitar usar el LevelManager que podría haber sido
    /// destruído antes de tiempo.
    /// </summary>
    /// <returns>Cierto si hay instancia creada.</returns>
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Devuelve las dimensiones del mapa de la escena
    /// </summary>
    /// <returns></returns>
    public Vector2 GetMapSize()
    {
        return new Vector2(MapWidth, MapHeight);
    }

    /// <summary>
    /// Activa controles del player y el interactive de los NPCs
    /// </summary>
    public void EnableBehaviours()
    {
        EnablePlayerControls();
        EnableNPC();
    }

    /// <summary>
    /// Activa controles del player y el interactive de los NPCs
    /// </summary>
    public void DisableBehaviours()
    {
        DisablePlayerControls();
        DisableNPC();
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        // De momento no hay nada que inicializar
    }

    /// <summary>
    /// Activa los controles del player
    /// </summary>
    private void EnablePlayerControls()
    {
        _player.GetComponent<Movement>().enabled = true;
        _player.GetComponent<Shoot>().enabled = true;
        _player.GetComponent<MeleeAttack>().enabled = true;
        _player.GetComponent<Dash>().enabled = true;
    }

    /// <summary>
    /// Activa los NPCs para que se pueda interactuar con ellos y manden sus diálogos a DialogueManager
    /// </summary>
    private void EnableNPC()
    {
        Interactive[] NPCs = FindObjectsOfType<Interactive>();
        foreach (Interactive NPC in NPCs)
        {
            NPC.enabled = true;
        }
    }

    /// <summary>
    /// Desactiva los controles del player
    /// </summary>
    private void DisablePlayerControls()
    {
        _player.GetComponent<Movement>().enabled = false;
        _player.GetComponent<Shoot>().enabled = false;
        _player.GetComponent<MeleeAttack>().enabled = false;
        _player.GetComponent<Dash>().enabled = false;
    }

    /// <summary>
    /// Desactiva los NPCs para que se pueda interactuar con ellos y manden sus diálogos a DialogueManager
    /// </summary>
    private void DisableNPC()
    {
        Interactive[] NPCs = FindObjectsOfType<Interactive>();
        foreach (Interactive NPC in NPCs)
        {
            NPC.enabled = false;
        }
    }
    #endregion
} // class LevelManager 
  // namespace