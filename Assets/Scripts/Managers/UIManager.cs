//---------------------------------------------------------
// Gestor de la UI
// Lucía Mei Domínguez López
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class UIManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private Canvas MissionCompletedCanvas;

    //PROTOSISTEMA DE DIÁLOGOS
    //SISTEMA DE DIÁLOGOS
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject options;
    [SerializeField] private TextMeshProUGUI option1Text;
    [SerializeField] private TextMeshProUGUI option2Text;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private static UIManager _instance;
    /// <summary>
    /// dialogueOnGoing -> indica si hay un diálogo en curso. Sirve para poder pasar diálogos solo si hay diálogoss
    /// optionsOnGoing -> indica si hay opciones en curso
    /// justStarted -> indica si el diálogo acaba de empezar. Sirve para que no se omita la primera frase al ejecutarse el update
    /// currentName -> nombre del personaje que se muestra en pantalla
    /// currentDialogues -> array de diálogos del personaje que está hablando
    /// currentOptions -> opciones del NPC con el que el player está interactuando
    /// i -> índice para el array de diálogos
    /// </summary>
    //SISTEMA DE DIÁLOGOS
    private bool dialogueOnGoing = false;
    private bool optionsOnGoing = false;
    private bool justStarted = false;
    private string currentName;
    private string[] currentDialogues;
    private string[] currentOptions;
    private int i;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    protected void Awake()
    {
        if (_instance == null)
        {
            // Somos la primera y única instancia
            _instance = this;
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// 
    /// Desactiva los canvas de diálogo y de misión al inicio
    /// </summary>
    void Start()
    {
        dialogueUI.SetActive(false);
        MissionCompletedCanvas.gameObject.SetActive(false); //interfaz desactivada
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// 
    /// En if (justStarted) se ignora el primer input que inicia la interacción para evitar que omita la primera frase
    /// y en el siguiente bloque permite al jugador pasar al siguiente diálogo si hay un diálogo en curso, no hay opciones en curso
    /// y pulsa el botón de interacción
    /// </summary>
    void Update()
    {
        if (justStarted)
        {
            justStarted = false;
        }
        else if (dialogueOnGoing && InputManager.Instance.InteractWasPressedThisFrame() && !optionsOnGoing)
        {
            NextDialogue();
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public static UIManager Instance
    {
        get
        {
            Debug.Assert(_instance != null);
            return _instance;
        }
    }
    public static bool HasInstance()
    {
        return _instance != null;
    }

    public void Inform()
    {
        MissionCompletedCanvas.gameObject.SetActive(true);
    }
    #endregion

    //SISTEMA DE DIÁLOGOS
    /// <summary>
    /// la UI activa la caja de diálogos, recibe el nombre y los diálogos del NPC y los inicializa en sus atributos correspondientes.
    /// El array de diálogos de la UI se inicializa con el mismo tamaño que el array del NPC y copiamos el del NPC al de la UI para que los pueda usar
    /// Luego inicializa el índice que se va a usar para pasar los diálogos y muestra el nombre y el primer diálogo y finalmente se indica que hay un
    /// diálogo en curso y que el diálogo acaba de empezar.
    /// </summary>
    public void InitDialogues(string name, string[] dialogues, bool decisions)
    {
        dialogueUI.gameObject.SetActive(true);
        if (!decisions)
        {
            options.gameObject.SetActive(false);
        }
        else
        {
            optionsOnGoing = true;
            optionsDialogue();
        }
        currentName = name;
        currentDialogues = new string[dialogues.Length];
        Array.Copy(dialogues, 0, currentDialogues, 0, dialogues.Length); 

        i = 0;
        //
        nameText.text = currentName;
        dialogueText.text = currentDialogues[i];
        dialogueOnGoing = true;
        justStarted = true;
    }
    /// <summary>
    /// la UI recibe el nombre y los diálogos del NPC y los inicializa en sus atributos correspondientes
    /// </summary>
    public void optionsDialogue()
    {
        Debug.Log("Estamos en lo de options");
        option1Text.text = "Final malo";
        option2Text.text = "Nada";
        options.gameObject.SetActive(true);
    }
    public void optionSelected()
    {
        optionsOnGoing = false;
        NextDialogue();
    }
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    //SISTEMA DE DIÁLOGOS
    /// <summary>
    /// Si se encuentra en el último diálogo, pasa al siguiente, pero si no finaliza el diálogo
    /// </summary>
    private void NextDialogue()
    {
        if (i < currentDialogues.Length-1)
        {
            i++;
            dialogueText.text = currentDialogues[i];
        }
        else
        {
            EndDialogue();
        }
    }
    /// <summary>
    /// Al final el diálogo, se desactiva la caja de diálogos, se indica que ya no hay un diálogo en curso, el levelmanager habilita los controles del player,
    /// habilita al NPC para que pueda actualizar sus diálogos y se actualiza el estado del juego.
    /// </summary>
    private void EndDialogue()
    {
        dialogueUI.gameObject.SetActive(false); //desactivamos caja de diálogos
        options.gameObject.SetActive(false);
        dialogueOnGoing = false; //ya no hay un diálogo en curso
        LevelManager.Instance.EnablePlayerControls(); //LevelManager habilita los controles del _player
        LevelManager.Instance.EnableNPC();
        GameManager.Instance.UpdateState();
    }
    #endregion   

} // class UIManager 
// namespace
