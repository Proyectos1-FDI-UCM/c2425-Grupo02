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

    //atribuimos un GameObject al jugador
<<<<<<< HEAD
    [SerializeField] private GameObject player;
=======
   
    [SerializeField] float MapWidth;
    [SerializeField] float MapHeight;
>>>>>>> camara_sergio

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;
   

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
    /*
    Para cuando tengamos que mantener la toroidalidad activada solo si es un espacio exterior
    public bool Outside()
    {
        return SceneManager.GetActiveScene().buildIndex == (índice escena);
    }
    */
    public Vector2 GetMapSize()
    {
<<<<<<< HEAD
        float mapWidth, mapHeight;
        
        if (SceneManager.GetActiveScene().buildIndex == 2) //escena cámara Adrián
        {
            mapWidth = 50;
            mapHeight = 50;
        }
        else
        {
            mapWidth = 16;
            mapHeight = 10;
        }
        Vector2 mapSize = new Vector2(mapWidth, mapHeight);
        return mapSize;
=======
        return new Vector2(MapWidth, MapHeight);
>>>>>>> camara_sergio
    }

    public void EnablePlayerControls()
    {
        player.GetComponent<Movement>().enabled = true;
        player.GetComponent<Shoot>().enabled = true;
        player.GetComponent<MeleeAttack>().enabled = true;
        player.GetComponent<Dash>().enabled = true;
        player.GetComponent<Player_Interact>().enabled = true;
    }
    public void DisablePlayerControls()
    {
        player.GetComponent<Movement>().enabled = false;
        player.GetComponent<Shoot>().enabled = false;
        player.GetComponent<MeleeAttack>().enabled = false;
        player.GetComponent<Dash>().enabled = false;
        player.GetComponent<Player_Interact>().enabled = false;
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

    
  


    #endregion
} // class LevelManager 
  // namespace