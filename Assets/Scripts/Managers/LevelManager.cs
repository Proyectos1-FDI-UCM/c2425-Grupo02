//---------------------------------------------------------
// Gestor de escena. Podemos crear uno diferente con un
// nombre significativo para cada escena, si es necesario
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

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
    /// Objetos que aparecen después de derrotar a los enemigos del long path (el segundo Spora y la poción)
    /// </summary>
    [SerializeField] private GameObject[] EndLongPathObj;
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
    [SerializeField] private GameObject DiscoDoor;
    /// <summary>
    /// Scene exit de la entrada a la discoteca
    /// </summary>
    [SerializeField] private GameObject ExitSceneDisco;
    /// <summary>
    /// Trigger dialogues de la escena
    /// </summary>
    [SerializeField] private GameObject[] TriggerDialogues;
    /// <summary>
    /// Estatua de checkpoint de la barra de bartender
    /// </summary>
    [SerializeField] private GameObject BarCheckpointStatue;
    /// <summary>
    /// Sprite de checkpoint activado
    /// </summary>
    [SerializeField] private Sprite CheckpointActive;
    /// <summary>
    /// Scene exit de la escena de introducción
    /// </summary>
    [SerializeField] private GameObject ExitIntro;
    /// <summary>
    /// Guadaña de akwardly long path
    /// </summary>
    [SerializeField] private GameObject ScytheInLongPath;
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
    private void Start()
    {
        if (BarCheckpointStatue != null && GameManager.Instance.SavedCheckpoint == 5)
        {
            ChangeBarStatue();
        }
        if (!GameManager.Instance.InitCombatStateHasFinished)
        {
            if (EndLongPathObj != null)
            {
                foreach (GameObject obj in EndLongPathObj) 
                { 
                    obj.SetActive(false);
                }
            }
        }
        if (GameManager.Instance.QuestState != 2)
        {
            if (ExitSceneDisco != null) 
            { 
                ExitSceneDisco.SetActive(false);
            }
        }
        if (GameManager.Instance.QuestCheatEnabled)
        {
            OpenDisco();
        }
        if (ExitIntro != null && !GameManager.Instance.HasBeenRead("1IntroductionSpora"))
        {
            ExitIntro.SetActive(false);
        }
    }

    private void Update()
    {
        if (InputManager.Instance.CompleteQuestWasPressedThisFrame())
        {
            OpenDisco();
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
    /// Habilita salida de la introducción
    /// </summary>
    public void EnableExitIntro()
    {
        ExitIntro.SetActive(true);
    }
    /// <summary>
    /// Activa los enemigos iniciales, suma el contador por cada uno de ellos, desactiva las salidas de escena 
    /// e indica que se ha iniciado el combate
    /// </summary>
    public void StartInitCombat()
    {
        Destroy(ScytheInLongPath);
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
        if (Blocks  != null)
        {
            foreach (GameObject block in Blocks)
            {
                block.SetActive(true);
            }
        }
        _initCombatStarted = true;
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
    /// <summary>
    /// Getter de los trigger dialogues de la escena
    /// </summary>
    public GameObject[] Triggers
    {
        get { return TriggerDialogues; }
    }
    /// <summary>
    /// Método que se llama cuando completas la misión de los paquetes. Habilita la entrada a la discoteca y se deshabilita el trigger dialogue
    /// que te impide pasar
    /// </summary>
    public void OpenDisco()
    {
        if (DiscoDoor != null && ExitSceneDisco != null) 
        {
            DiscoDoor.SetActive(false);
            ExitSceneDisco.SetActive(true);
            TriggerDialogues[1] = DiscoDoor;
            GameManager.Instance.DisableTrigDialogues(TriggerDialogues);
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
        HideIramis();
    }
    /// <summary>
    /// Después de hablar con la bartender, se cambia el sprite de la estatua del checkpoint al del checkpoint activado
    /// </summary>
    public void ChangeBarStatue()
    {
        if (BarCheckpointStatue != null)
        {
            BarCheckpointStatue.GetComponent<SpriteRenderer>().sprite = CheckpointActive;
        }
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
        _player.GetComponent<Dash>().enabled = true;
        if (GameManager.Instance.HasScythe)
        {
            _player.GetComponent<Shoot>().enabled = true;
            _player.GetComponent<MeleeAttack>().enabled = true;
        }
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
    /// <summary>
    /// Método que se llama cuando el combate inicial en long path termina. Se activa al segundo Spora y los heals de la escena
    /// </summary>
    private void InitCombatEnded()
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
        if (EndLongPathObj != null)
        {
            foreach (GameObject obj in EndLongPathObj)
            {
                obj.SetActive(true);
            }
        }
        GameManager.Instance.InitCombatStateHasFinished = true;
        _initCombatStarted = false;
    }
    #endregion
} // class LevelManager 
  // namespace