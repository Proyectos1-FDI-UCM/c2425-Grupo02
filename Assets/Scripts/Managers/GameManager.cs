//---------------------------------------------------------
// Contiene el componente GameManager
// Guillermo Jiménez Díaz, Pedro Pablo Gómez Martín
// TemplateP1
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor.SearchService;
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
    /// Guarda las posiciones a las que mandan los diferentes triggers de salida de escena 
    /// </summary>
    private Vector2 _spawnPosition;
    /// <summary>
    /// Indica si el player ha cogido la guadaña
    /// </summary>
    [SerializeField] private bool _hasScythe;
    /// <summary>
    /// Cantidad de puntos de vida que tiene el jugador
    /// </summary>
    private int _health;

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
    public void UpdateSave()
    {
        _saveUsed = true;
    }

        ///<summary>
        /// Lo llamamos desde Healing_GameObjects si el jugador 
        /// Lo llamamos desde Healing_GameObjects si el jugador 
        /// colisiona contra dicho objeto
        /// Llama al método de curación del script de la salud del jugador para curarle cierta cantidad de vida
        ///</summary>
    public void HealCollected(GameObject Player)
    {
        Player_Health playerHealth = Player.GetComponent<Player_Health>();

        if (Player != null) 
        {
            playerHealth.Heal(1);
            _health = playerHealth.ReturnHealth();
            AudioManager.Instance.PlayAudio(healSFX, 0.5f);
        }

        else Debug.LogError("_player es null.");

    }
    /// <summary>
    /// </summary>
    /// <returns></returns>
    /// 
    ///<summary>
    ///Se llama desde QuestObjects si el jugador colisiona contra dicho objeto. Va actualizando el número de objetos de misión obtenidos por el jugador
    ///</summary>
    public void Damage(int Health)
    {
        UIManager.UpdateHealth(Health);
    }
    public int questState()
    {
        return _questState;
    }
    public int questObjectsCount()
    {
        return _questObjectsCount;
    }
    public void OnQuestObjectCollected()
    {
        _questObjectsCount++;
        Debug.Log("Objetos de misión obtenidos: " + _questObjectsCount);

        if (_questObjectsCount == 1)
            Debug.Log("Misión comenzada");
                    
        else if (_questObjectsCount == 3)
        {
            _questState = 2;
            if (UIManager.HasInstance())
            {
                UIManager.Instance.Inform();
            }
            else
            {
                Debug.LogError("UIManager no está inicializado.");
            }
            Debug.Log("Misión terminada");
            UIManager.Instance.HideQuestUI();
        }
        UIManager.UpdateQuestProgress(_questObjectsCount);
    }
    
    ///<summary>
    ///Marca el diálogo como leído al finalizar
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

    public void GetHealth()
    {
        Player_Health playerHealth = Player.GetComponent<Player_Health>();
        _health = playerHealth.ReturnHealth();
    }

    public int ReturnHealth() { return _health; }

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
            DisableTrigDialogues();
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
        else if (dialogueName == "No")
        {
            LevelManager.Instance.EnableBoss();
        }
    }
    /// <summary>
    /// Deshabilita los collides del trigger dialogue correspondiente y añade su nombre a un hashset para que, en caso de que el
    /// player regrese a la escena, los colliders del trigger no vuelva a inicializarse en su start
    /// </summary>
    private void DisableTrigDialogues()
    {
        GameObject triggerDialogue = FindObjectOfType<TriggerDialogue>().gameObject;
        triggerDialogue.SetActive(false);
        _disabledTrigDialogues.Add(triggerDialogue.GetComponent<TriggerDialogue>().TriggerName);
    }
    
    #endregion
} // class GameManager 
// namespace