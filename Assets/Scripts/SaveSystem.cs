//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class SaveSystem : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    public int checkpointIndex;
    public Vector2 checkpointSpawn;
    public int checkpointScene;
    public int health;
    public int questState;
    public int questObjectsCount;
    public bool hasScythe;
    public bool initCombatFinish;
    public bool saveUsed;
    public static List<string> readDialogues;
    public static List<string> disabledTrigDialogues;
    public static List<int> collectedHeals;
    public static List<int> collectedBoxes;
    /// <summary>
    /// Construimos el SaveSystem
    /// </summary>
    public SaveSystem()
    {
        // Asignamos los valores por defecto
        checkpointIndex = 0;
        checkpointSpawn = Vector2.zero;
        checkpointScene = 1;
        health = 5;
        questState = 0;
        questObjectsCount = 0;
        hasScythe = false;
        initCombatFinish = false;
        saveUsed = false;
        readDialogues = new List<string>();
        disabledTrigDialogues = new List<string>();
        collectedHeals = new List<int>();
        collectedBoxes = new List<int>();
    }
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    // Strings constantes para no tener que reescribir todo el texto cada vez.
    private const string CHECKPOINT_INDEX_KEY = "CheckpointIndex";
    private const string CHECKPOINT_SPAWN_X_KEY = "CheckpointSpawnX";
    private const string CHECKPOINT_SPAWN_Y_KEY = "CheckpointSpawnY";
    private const string CHECKPOINT_SCENE_KEY = "CheckpointScene";
    private const string HEALTH_KEY = "Health";
    private const string QUEST_STATE_KEY = "QuestState";
    private const string QUEST_OBJECTS_COUNT_KEY = "QuestObjectsCount";
    private const string HAS_SCYTHE_KEY = "HasScythe";
    private const string INIT_COMBAT_FINISH_KEY = "InitCombatFinish";
    private const string READ_DIALOGUES_KEY = "ReadDialogues";
    private const string DISABLED_TRIG_DIALOGUES_KEY = "DisabledTrigDialogues";
    private const string COLLECTED_HEALS_KEY = "CollectedHeals";
    private const string COLLECTED_BOXES_KEY = "CollectedBoxes";
    private const string HAS_SAVE_DATA_KEY = "HasSaveData";
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

    /// <summary>
    /// Guarda toda la información en sus respectivos PlayerPrefs.
    /// </summary>
    public static void SaveData()
    {
        GameManager gm = GameManager.Instance;
        PlayerPrefs.SetInt(CHECKPOINT_INDEX_KEY, gm.SavedCheckpoint);
        PlayerPrefs.SetInt(CHECKPOINT_SCENE_KEY, SceneManager.GetActiveScene().buildIndex);
        Checkpoint checkpointScript = FindObjectOfType<Checkpoint>();
        Vector2 spawnPoint = checkpointScript.SpawnPoint();
        PlayerPrefs.SetFloat(CHECKPOINT_SPAWN_X_KEY, spawnPoint.x);
        PlayerPrefs.SetFloat(CHECKPOINT_SPAWN_Y_KEY, spawnPoint.y);
        PlayerPrefs.SetInt(HEALTH_KEY, gm.ReturnHealth());
        PlayerPrefs.SetInt(QUEST_STATE_KEY, gm.QuestState);
        PlayerPrefs.SetInt(QUEST_OBJECTS_COUNT_KEY, gm.questObjectsCount());
        PlayerPrefs.SetInt(HAS_SCYTHE_KEY, gm.HasScythe ? 1 : 0);
        PlayerPrefs.SetInt(INIT_COMBAT_FINISH_KEY, gm.InitCombatStateHasFinished ? 1 : 0);
        // Al haber listas de cosas por guardar, obtenemos el tamaño, la convertimos en una lista, y los guardamos con bucles for.
        PlayerPrefs.SetInt(COLLECTED_BOXES_KEY, gm.GetCollectedBoxes.Count());
        PlayerPrefs.SetInt(COLLECTED_HEALS_KEY, gm.GetCollectedHeals.Count());
        PlayerPrefs.SetInt(DISABLED_TRIG_DIALOGUES_KEY, gm.GetTrigDialogues.Count());
        PlayerPrefs.SetInt(READ_DIALOGUES_KEY, gm.GetReadDialogues.Count());
        collectedBoxes = gm.GetCollectedBoxes.ToList();
        collectedHeals = gm.GetCollectedHeals.ToList();
        disabledTrigDialogues = gm.GetTrigDialogues.ToList();
        readDialogues = gm.GetReadDialogues.ToList();
        for (int i = 0; i< gm.GetCollectedBoxes.Count(); i++)
        {
            PlayerPrefs.SetInt("box_" + i.ToString(),collectedBoxes[i]);
        }
        for (int i = 0; i < gm.GetCollectedHeals.Count(); i++)
        {
            PlayerPrefs.SetInt("heal_" + i.ToString(), collectedHeals[i]);
        }
        for (int i = 0; i < gm.GetTrigDialogues.Count(); i++)
        {
            PlayerPrefs.SetString("dialogue_" + i.ToString(), disabledTrigDialogues[i]);
        }
        for (int i = 0; i < gm.GetReadDialogues.Count(); i++)
        {
            PlayerPrefs.SetString("read_" + i.ToString(), readDialogues[i]);
        }
        // Marcamos que hay datos de guardado.
        PlayerPrefs.SetInt(HAS_SAVE_DATA_KEY, 1);
        // Guardamos toda la info.
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Usando toda la información guardada, la cargamos al cargar el juego.
    /// - Se comprueba si hay datos de guardado y si hay un game manager existente
    /// - Tratamos de cargar todo. SI se puede, se carga. Si no, muestra un error.
    /// </summary>
    public static bool LoadGame()
    {
        if (!HasSaveData()) // Si no hay datos de guardado, no lo carga
        {
            Debug.Log("No hay datos de guardado disponibles");
            return false;
        }

        if (!GameManager.HasInstance()) // Si no hay game manager, no lo carga
        {
            Debug.Log("No se puede cargar: GameManager no existe");
            return false;
        }

        GameManager gm = GameManager.Instance; // Guardamos el gamemanager

        try // Tratamos de cargar todos los datos
        {
            int checkpointIndex = PlayerPrefs.GetInt(CHECKPOINT_INDEX_KEY, 0);
            int sceneIndex = PlayerPrefs.GetInt(CHECKPOINT_SCENE_KEY);
            float spawnX = PlayerPrefs.GetFloat(CHECKPOINT_SPAWN_X_KEY, 0f);
            float spawnY = PlayerPrefs.GetFloat(CHECKPOINT_SPAWN_Y_KEY, 0f);
            Vector2 checkpointSpawn = new Vector2(spawnX, spawnY);
            int health = PlayerPrefs.GetInt(HEALTH_KEY, 5);
            int questState = PlayerPrefs.GetInt(QUEST_STATE_KEY, 0);
            int questObjectsCount = PlayerPrefs.GetInt(QUEST_OBJECTS_COUNT_KEY, 0);
            bool hasScythe = PlayerPrefs.GetInt(HAS_SCYTHE_KEY, 0) == 1;
            bool initCombatFinish = PlayerPrefs.GetInt(INIT_COMBAT_FINISH_KEY, 0) == 1;
            // Llamamos al método del game manager que carga los datos.
            gm.LoadGameData(checkpointIndex, sceneIndex, checkpointSpawn, health, questState, questObjectsCount,
                           hasScythe, initCombatFinish);
            int boxesCount = PlayerPrefs.GetInt(COLLECTED_BOXES_KEY);
            int healsCount = PlayerPrefs.GetInt(COLLECTED_HEALS_KEY);
            int trigDialogues = PlayerPrefs.GetInt(DISABLED_TRIG_DIALOGUES_KEY);
            int readDialogues = PlayerPrefs.GetInt(READ_DIALOGUES_KEY);
            List<int> heals = new List<int>(healsCount);
            List<int> boxes = new List<int>(boxesCount);
            List<string> disabledTrigDialgs = new List<string>(trigDialogues);
            List<string> readDialgs = new List<string>(readDialogues);
            for (int i = 0; i < boxesCount; i++)
            {
                boxes.Add(PlayerPrefs.GetInt("box_" + i.ToString()));
            }
            for (int i = 0; i < healsCount; i++)
            {
                heals.Add(PlayerPrefs.GetInt("heal_" + i.ToString()));
            }
            for (int i = 0; i < trigDialogues; i++)
            {
                disabledTrigDialgs.Add(PlayerPrefs.GetString("dialogue_" + i.ToString()));
            }
            for (int i = 0; i < readDialogues; i++)
            {
                readDialgs.Add(PlayerPrefs.GetString("read_" + i.ToString()));
            }
            // Llamamos al método del game manager que carga los datos de las listas.
            gm.LoadCollectedItemsAndDialogues(heals, boxes, disabledTrigDialgs, readDialgs);
            return true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error al cargar el juego: {e.Message}");
            return false;
        }
    }
    /// <summary>
    /// Borra todos los datos de guardado al ser llamado
    /// </summary>
    public static void DeleteSaveData()
    {
        PlayerPrefs.DeleteKey(CHECKPOINT_INDEX_KEY);
        PlayerPrefs.DeleteKey(CHECKPOINT_SPAWN_X_KEY);
        PlayerPrefs.DeleteKey(CHECKPOINT_SPAWN_Y_KEY);
        PlayerPrefs.DeleteKey(CHECKPOINT_SCENE_KEY);
        PlayerPrefs.DeleteKey(HEALTH_KEY);
        PlayerPrefs.DeleteKey(QUEST_STATE_KEY);
        PlayerPrefs.DeleteKey(QUEST_OBJECTS_COUNT_KEY);
        PlayerPrefs.DeleteKey(HAS_SCYTHE_KEY);
        PlayerPrefs.DeleteKey(INIT_COMBAT_FINISH_KEY);
        PlayerPrefs.DeleteKey(READ_DIALOGUES_KEY);
        PlayerPrefs.DeleteKey(DISABLED_TRIG_DIALOGUES_KEY);
        PlayerPrefs.DeleteKey(COLLECTED_HEALS_KEY);
        PlayerPrefs.DeleteKey(COLLECTED_BOXES_KEY);
        PlayerPrefs.DeleteKey(HAS_SAVE_DATA_KEY);       
        PlayerPrefs.Save();
    }
    /// <summary>
    /// Comprueba si hay datos de guardado
    /// </summary>
    public static bool HasSaveData()
    {
        return PlayerPrefs.GetInt(HAS_SAVE_DATA_KEY, 0) == 1;
    }
    /// <summary>
    /// Guardado automático que se llamará desde los checkpoint.
    /// </summary>
    public static void AutoSave()
    {
        if (GameManager.HasInstance())
        {
            SaveData();
            Debug.Log("Guardado automático realizado");
        }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class SaveSystem 
// namespace
