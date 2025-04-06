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
using static Unity.Collections.AllocatorManager;

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
    /// Objetos de la boss fight
    /// </summary>
    [SerializeField] private GameObject[] BossGameObjects;
    /// <summary>
    /// Enemigos combate inicial
    /// </summary>
    [SerializeField] private GameObject[] InitialEnemies;
    /// <summary>
    /// Bloqueadores de salidas para que el player no se salga del mapa
    /// </summary>
    [SerializeField] private GameObject[] Blocks;
    /// <summary>
    /// Salidas de escenas
    /// </summary>
    [SerializeField] private GameObject[] SceneExits;
    /// <summary>
    /// Solo para la escena outside of the party y Boss room
    /// </summary>
    [SerializeField] private GameObject Iramis;
    /// <summary>
    /// Scene exit de la entrada a la discoteca
    /// </summary>
    [SerializeField] private GameObject ExitToDisco;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static LevelManager _instance;
    private GameObject _player;
    /// <summary>
    /// Indica si ha empezado el combate inicial de Akwardly long path
    /// </summary>
    private bool _initCombatStarted = false;
    /// <summary>
    /// Contador de enemigos en akwardly long path
    /// </summary>
    private int _nInitEnemies = 0;
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
    public bool InitCombatStarted
    {
        get { return _initCombatStarted; }
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

    /// <summary>
    /// Activa los enemigos iniciales, suma el contador por cada uno de ellos, desactiva las salidas de escena 
    /// e indica que se ha iniciado el combate
    /// </summary>
    public void StartInitCombat()
    {
        Debug.Log("Init Combat started");
        if (InitialEnemies != null)
        {
            Debug.Log("Hay initial enemies");
            foreach (GameObject enemy in InitialEnemies)
            {
                enemy.SetActive(true);
                _nInitEnemies++;
                Debug.Log("Enemy added");
            }
        }
        else
        {
            Debug.Log("ERROR: no hay initial enemies");
        }

        if (SceneExits != null)
        {
            foreach (GameObject exit in SceneExits)
            {
                exit.SetActive(false);
            }
        }
        else
        {
            Debug.Log("ERROR: no hay exits");
        }

        if (Blocks != null)
        {
            foreach (GameObject block in Blocks)
            {
                block.gameObject.SetActive(true);
            }
        }
        else
        {
            Debug.Log("ERROR: no hay bloques");
        }
        _initCombatStarted = true;
        Debug.Log("Enemy number: " + _nInitEnemies);
    }
    /// <summary>
    /// Cuando un enemigo muere, se resta 1 al contador
    /// </summary>
    public void SubEnemyCount()
    {
        _nInitEnemies--;
        if (_nInitEnemies == 0)
        {
            InitCombatEnded();
        }
        Debug.Log("Enemy number: " + _nInitEnemies);
    }
    public void InitCombatEnded()
    {
        if (Blocks != null)
        {
            foreach (GameObject block in Blocks)
            {
                block.gameObject.SetActive(false);
            }
        }
        if (SceneExits != null)
        {
            foreach (GameObject exit in SceneExits)
            {
                exit.SetActive(true);
            }
        }
        else
        {
            Debug.Log("ERROR: no hay exits");
        }
        _initCombatStarted = false;
    }
    /// <summary>
    /// 
    /// </summary>
    public void ShowIramis()
    {
        Iramis.SetActive(true);
    }
    /// <summary>
    /// 
    /// </summary>
    public void HideIramis()
    {
        Iramis.SetActive(false);
    }
    public void OpenDisco()
    {
        if (ExitToDisco != null) 
        {
            ExitToDisco.SetActive(true);
            GameObject discoDoor = GameObject.Find("DiscoDoor_Dialogue");
            discoDoor.SetActive(false);
        }
    }
    /// <summary>
    /// Activa los objetos de la boss fight y desactiva la salida y el gameobject del diálogo de Iramis
    /// </summary>
    public void EnableBoss()
    {
        foreach (GameObject exit in SceneExits)
        {
            exit.SetActive(false);
        }
        foreach (GameObject obj in BossGameObjects)
        {
            obj.SetActive(true);
        }
        Pillar[] pillars = FindObjectsOfType<Pillar>();
        foreach (Pillar pillar in pillars)
        {
            pillar.enabled = true;
        }
        Healing_GameObjects[] heals = FindObjectsOfType<Healing_GameObjects>();
        foreach (Healing_GameObjects heal in heals)
        {
            foreach (var comp in heal.GetComponentsInChildren<MonoBehaviour>())
            {
                comp.enabled = true;
            }
        }
        HideIramis();
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