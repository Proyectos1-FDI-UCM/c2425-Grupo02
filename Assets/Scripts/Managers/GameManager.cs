//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;


/// <summary>
/// Componente responsable de la gestión global del juego. Es un singleton
/// que orquesta el funcionamiento general de la aplicación,
/// sirviendo de comunicación entre las escenas.
///
/// El GameManager ha de sobrevivir entre escenas por lo que hace uso del
/// DontDestroyOnLoad. En caso de usarlo, cada escena debería tener su propio
/// GameManager para evitar problemas al usarlo. Además, se debería producir
/// un intercambio de información entre los GameManager de distintas escenas.
/// Generalmente, esta información debería estar en un LevelManager o similar.
/// </summary>
public class GameManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)

    [SerializeField] GameObject Player;
    
    /// <summary>
    /// Sonido de curación del jugador
    /// </summary>
    [SerializeField]
    private AudioClip healSFX;
    /// <summary>
    
   

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    /// <summary>
    /// Instancia única de la clase (singleton).
    /// </summary>
    private static GameManager _instance;
    /// <summary>
    /// Indica si el player ha terminado el combate en awkwardly long path
    /// </summary>
    private bool _initCombatFinish = false;
    /// <summary>
    /// Indice de checkpoints:
    /// 0 -> no hay checkpoint;
    /// 1 -> checkpoint akwardly long path;
    /// 2 -> checkpoint entrada laberinto;
    /// 3 -> checkpoint laberinto 2;
    /// 4 -> checkpoint laberinto 3;
    /// 5 -> checkpoint boss;
    /// </summary>
    private int _checkpointIndex = 0;
    /// <summary>
    /// posición que guarda el checkpoint actual, que será donde spawneará el player al pulsar continuar en el menú
    /// </summary>
    private Vector2 _checkpointSpawn = Vector2.zero;
    private int _checkpointScene = 1;
    /// <summary>
    /// contador objetos de misión
    /// </summary>
    private int _questObjectsCount;
    private UIManager _uiManager;
    /// <summary>
    /// indica el estado de la misión (0 = sin empezar; 1 = en progreso; 2 = terminada)
    /// </summary>
    private int _questState = 0;
    private bool _saveUsed = false;
    /// <summary>
    /// HashSet de diálogos leídos
    /// </summary>
    private HashSet<string> _readDialogues = new HashSet<string>();
    /// <summary>
    /// Hashset para desactivar diálogos trigger
    /// </summary>
    private HashSet<string> _disabledTrigDialogues = new HashSet<string>();
    /// <summary>
    /// Hashset que guarda qué heals se han recogido para que no vuelvan a aparecer al cargar escena
    /// </summary>
    private HashSet<int> _collectedHeals = new HashSet<int>();
    /// <summary>
    /// Hashset que guarda qué paquetes se han recogido para que no vuelvan a aparecer al cargar escena
    /// </summary>
    private HashSet<int> _collectedBoxes = new HashSet<int>();

    /// <summary>
    /// Guarda las posiciones a las que mandan los diferentes triggers de salida de escena 
    /// </summary>
    private Vector2 _spawnPosition;
    /// <summary>
    /// Indica si el player ha cogido la guadaña
    /// </summary>
    private bool _hasScythe;
    /// <summary>
    /// Cantidad de puntos de vida que tiene el jugador
    /// </summary>
    private int _health;

    //DEV CHEATS
    private bool _invulnerability = false;
    private bool _questCheat = false;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour

    /// <summary>
    /// Método llamado en un momento temprano de la inicialización.
    /// En el momento de la carga, si ya hay otra instancia creada,
    /// nos destruimos (al GameObject completo)
    /// </summary>
    protected void Awake()
    {
        _questObjectsCount = 0;


        if (_instance != null)
        {
            // No somos la primera instancia. Se supone que somos un
            // GameManager de una escena que acaba de cargarse, pero
            // ya había otro en DontDestroyOnLoad que se ha registrado
            // como la única instancia.
            // Si es necesario, transferimos la configuración que es
            // dependiente de la escena. Esto permitirá al GameManager
            // real mantener su estado interno pero acceder a los elementos
            // de la escena particulares o bien olvidar los de la escena
            // previa de la que venimos para que sean efectivamente liberados.
            TransferSceneState();

            // Y ahora nos destruímos del todo. DestroyImmediate y no Destroy para evitar
            // que se inicialicen el resto de componentes del GameObject para luego ser
            // destruídos. Esto es importante dependiendo de si hay o no más managers
            // en el GameObject.
            DestroyImmediate(this.gameObject);
        }
        else
        {
            // Somos el primer GameManager.
            // Queremos sobrevivir a cambios de escena.
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
            Init();
        } // if-else somos instancia nueva o no.
    }

    private void Update()
    {
        if (InputManager.Instance.InvulnerabilityWasPressedThisFrame()) 
        {
            _invulnerability = !_invulnerability;
        }

        else if (InputManager.Instance.CompleteQuestWasPressedThisFrame())
        {
            _questCheat = true;
            _questState = 2;
            _questObjectsCount = 3;
            UIManager.Instance.ShowQuestUI();
            UIManager.Instance.UpdateQuestProgress(_questObjectsCount);
        }
    }

    /// <summary>
    /// Método llamado cuando se destruye el componente.
    /// </summary>
    protected void OnDestroy()
    {
        if (this == _instance)
        {
            // Éramos la instancia de verdad, no un clon.
            _instance = null;
        } // if somos la instancia principal
    }




    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    /// <summary>
    /// Propiedad para acceder a la única instancia de la clase.
    /// </summary>
    public static GameManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }

    /// <summary>
    /// Getter para obtener el estado de la misión
    /// </summary>
    public int QuestState
    {
        get { return _questState; }
    }
    public bool SaveUsed
    {
        get { return _saveUsed; }
    }
    /// <summary>
    /// Getter para comprobar si ha cogido la guadaña
    /// </summary>
    public bool HasScythe
    {
        get { return _hasScythe; }
    }

    public bool GetInitCombatState
    {
        get { return _initCombatFinish; }
        set { _initCombatFinish = value; }
    }

    public int SavedCheckpoint
    {
        get { return _checkpointIndex; }
        set { _checkpointIndex = value; }
    }

    public HashSet<int> GetCollectedHeals
    {
        get { return _collectedHeals; }
    }

    public HashSet<int> GetCollectedBoxes
    {
        get { return _collectedBoxes; }
    }

    //DEV CHEATS

    public bool HasInvulnerability
    {
        get { return _invulnerability; }
    }

    public bool QuestCheatEnabled
    {
        get { return _questCheat; }
    }

    public void UpdateSave()
    {
        _saveUsed = true;
        SetNewCheckpoint(5, new Vector2(0, 126.75f), 5);
        UIManager.Instance.ShowCheckpointBarNotif();
    }

        ///<summary>
        /// Lo llamamos desde Healing_GameObjects si el jugador 
        /// Lo llamamos desde Healing_GameObjects si el jugador 
        /// colisiona contra dicho objeto
        /// Llama al método de curación del script de la salud del jugador para curarle cierta cantidad de vida
        ///</summary>
    public void HealCollected(GameObject Player, int id)
    {
        Player_Health playerHealth = Player.GetComponent<Player_Health>();

        if (Player != null) 
        {
            playerHealth.Heal(1);
            AudioManager.Instance.PlayAudio(healSFX, 0.5f);
            if (id != 99)
            {
                _collectedHeals.Add(id);
            }
        }

        else Debug.LogError("_player es null.");

    }
    public void SaveAndSendHealth(int Health)
    {
        _health = Health;
        UIManager.Instance.UpdateHealth(_health);
    }
    public int questState()
    {
        return _questState;
    }
    public int questObjectsCount()
    {
        return _questObjectsCount;
    }
    public void OnQuestObjectCollected(int id)
    {
        _questObjectsCount++;
        _collectedBoxes.Add(id);
        Debug.Log("Objetos de misión obtenidos: " + _questObjectsCount);

        if (_questObjectsCount == 1)
            Debug.Log("Misión comenzada");
                    
        else if (_questObjectsCount == 3)
        {
            _questState = 2;
          
            Debug.Log("Misión terminada");
            UIManager.Instance.HideQuestUI();
        }
        UIManager.Instance.UpdateQuestProgress(_questObjectsCount);
    }
    
    ///<summary>
    ///Marca el diálogo como leído al finalizar y después llama al método para actualizar el estado del juego
    /// </summary>
    public void MarkAsRead(string dialogueName)
    {
        if (!_readDialogues.Contains(dialogueName))
        {
            _readDialogues.Add(dialogueName);
        }
        UpdateState(dialogueName);
    }
    /// <summary>
    /// Comprueba si el diálogo ya ha sido leído
    /// </summary>
    /// <param name="dialogue"></param>
    /// <returns></returns>
    public bool HasBeenRead(string dialogueName)
    {
        return _readDialogues.Contains(dialogueName);
    }
    /// <summary>
    /// Comprueba si se ha deshabilitado el trigger dialogue
    /// </summary>
    /// <param name="triggerName"></param>
    /// <returns></returns>
    public bool TrigDialogueIsDisabled(string triggerName)
    {
        return _disabledTrigDialogues.Contains(triggerName);
    }
    public static bool HasInstance()
    {
        return _instance != null;
    }

    /// <summary>
    /// Cuando el jugador entra en el trigger que manda al cambio de escena, 
    /// este manda una posición al GameManager y lo guarda en una variable global
    /// </summary>
    /// <param name="spawn">Determina donde va a aparecer el jugador en escena</param>
    public void SetSpawnPoint(Vector2 spawn)
    {
        _spawnPosition = spawn;
    }

    /// <summary>
    /// Manda la información de aparición al script de movimiento del jugador
    /// </summary>
    /// <returns>Coordenadas de aparición del jugador</returns>
    public Vector2 GetSpawnPoint() { return _spawnPosition; }
    public int ReturnHealth() { return _health; }

    
   /// <summary>
   /// Método para establecer el índice, el vector de posición de spawn del player y la escena de spawn que está guardada en el nuevo checkpoint
   /// </summary>
   /// <param name="checkpoint"></param>
   /// <param name="pos"></param>
   /// <param name="scene"></param>
    public void SetNewCheckpoint(int checkpoint, Vector2 pos, int scene)
    {
        _checkpointIndex = checkpoint;
        _checkpointSpawn = pos;
        _checkpointScene = scene;
    }
    /// <summary>
    /// Método para reiniciar el checkpoint y el gamemanager y trasladar al jugador a la escena de introducción en la posición indicada
    /// en la nueva partida
    /// </summary>
    public void PrepareNewGame()
    {
        SetNewCheckpoint(0, Vector2.zero, 1);
        ResetGameManager();
        ChangeScene(1);
        SetSpawnPoint(Vector2.zero);
    }
    /// <summary>
    /// Método para resetear el gamemanager como corresponda y trasladar al jugador a la escena y posición indicada por el checkpoint
    /// </summary>
    public void PrepareContinue()
    {
        ResetGameManager();
        ChangeScene(_checkpointScene);
        SetSpawnPoint(_checkpointSpawn);
    }
    /// <summary>
    /// Deshabilita los trigger dialogue correspondiente y añade su nombre a un hashset para que, en caso de que el
    /// player regrese a la escena, los colliders del trigger no vuelva a inicializarse en su start
    /// </summary>
    public void DisableTrigDialogues(GameObject[] triggers)
    {
        foreach (GameObject trigger in triggers)
        {
            trigger.SetActive(false);
            _disabledTrigDialogues.Add(trigger.GetComponent<TriggerDialogue>().TriggerName);
        }
    }
    /// <summary>
    /// Método que cambia la escena actual por la indicada en el parámetro.
    /// </summary>
    /// <param name="index">Índice de la escena (en el build settings)
    /// que se cargará.</param>
    public void ChangeScene(int index)
    {
        // Antes y después de la carga fuerza la recolección de basura, por eficiencia,
        // dado que se espera que la carga tarde un tiempo, y dado que tenemos al
        // usuario esperando podemos aprovechar para hacer limpieza y ahorrarnos algún
        // tirón en otro momento.
        // De Unity Configuration Tips: Memory, Audio, and Textures
        // https://software.intel.com/en-us/blogs/2015/02/05/fix-memory-audio-texture-issues-in-unity
        //
        // "Since Unity's Auto Garbage Collection is usually only called when the heap is full
        // or there is not a large enough freeblock, consider calling (System.GC..Collect) before
        // and after loading a level (or put it on a timer) or otherwise cleanup at transition times."
        //
        // En realidad... todo esto es algo antiguo por lo que lo mismo ya está resuelto)
        System.GC.Collect();
        AudioManager.Instance.StopMusic();
        UnityEngine.SceneManagement.SceneManager.LoadScene(index);
        System.GC.Collect();
    } // ChangeScene

    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados

    /// <summary>
    /// Dispara la inicialización.
    /// </summary>
    private void Init()
    {
        Player_Health player_Health = Player.GetComponent<Player_Health>();
        _health = player_Health.ReturnHealth();
    }

    private void TransferSceneState()
    {
        // De momento no hay que transferir ningún estado
        // entre escenas
    }
    /// <summary>
    /// Actualiza las variables del juego al finalizar el diálogo si es necesario.
    /// En el diálogo de Minos, al terminar el primer diálogo se cambia _questState a 1 porque la misión ya ha comenzado
    /// </summary>
    private void UpdateState(string dialogueName)
    {
        if (dialogueName == "1IntroductionSpora" || dialogueName == "Scythe" || dialogueName == "MissionAccepted")
        {
            GameObject[] triggerDialogues = LevelManager.Instance.Triggers;
            DisableTrigDialogues(triggerDialogues);
            if (dialogueName == "MissionAccepted")
            {
                _questState = 1;
                Debug.Log("Quest has started");
                UIManager.Instance.ShowQuestUI();
            }
            else if (dialogueName == "Scythe")
            {
                GameObject scythe = GameObject.Find("Scythe_dialogue");
                scythe.SetActive(false);
                _hasScythe = true;
                UIManager.Instance.ShowShootIcon();
                if (Player != null)
                {
                    Player.GetComponent<Shoot>().enabled = true;
                    Player.GetComponent<MeleeAttack>().enabled = true;
                    LevelManager.Instance.StartInitCombat();
                    Debug.Log("You have the scythe");
                }
            }
        }
        else if (dialogueName == "MissionCompleted")
        {
            LevelManager.Instance.OpenDisco();
        }
        else if (dialogueName == "Bartender")
        {
            UpdateSave();
            LevelManager.Instance.ChangeBarStatue();
        }
        else if (dialogueName == "No")
        {
            LevelManager.Instance.EnableBoss();
        }
    }
    
    /// <summary>
    /// Metodo para resetear el gamemanager según los checkpoint
    /// </summary>
    private void ResetGameManager()
    {
        _health = 5;
        if (_checkpointIndex == 0)
        {
            _questObjectsCount = 0;
            _questState = 0;
            _saveUsed = false;
            _hasScythe = false;
            _initCombatFinish = false;
            _readDialogues.Clear();
            _disabledTrigDialogues.Clear();
            _collectedHeals.Clear();
            _collectedBoxes.Clear();
        }
        else if (_checkpointIndex == 1)
        {
            _hasScythe = false;
            _readDialogues = new HashSet<string> { "1IntroductionSpora" };
        }
        else
        {
            ResetCollectedObjects();
        }
    }
    /// <summary>
    /// Resetea los objetos curativos y cajas según el índice del checkpoint (incluye desde checkpoint 2 hasta checkpoint 4)
    /// </summary>
    private void ResetCollectedObjects()
    {
        if (_checkpointIndex == 2)
        {
            if (_collectedBoxes.Contains(0))
            {
                _collectedBoxes.Remove(0);
                _questObjectsCount--;
            }
            if (_collectedHeals.Contains(2))
            {
                _collectedHeals.Remove(2);
            }
            if (_collectedHeals.Contains(3))
            {
                _collectedHeals.Remove(3);
            }
        }
        else if (_checkpointIndex == 3)
        {
            if (_collectedBoxes.Contains(1))
            {
                _collectedBoxes.Remove(1);
                _questObjectsCount--;
            }
            if (_collectedHeals.Contains(5))
            {
                _collectedHeals.Remove(5);
            }
            if (_collectedHeals.Contains(6))
            {
                _collectedHeals.Remove(6);
            }
        }
        else if (_checkpointIndex == 4)
        {
            if (_collectedBoxes.Contains(2))
            {
                _collectedBoxes.Remove(2);
                _questObjectsCount--;
            }
            if (_collectedHeals.Contains(7))
            {
                _collectedHeals.Remove(7);
            }
        }
    }
    #endregion
} // class GameManager 
// namespace