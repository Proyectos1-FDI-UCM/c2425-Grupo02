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
    [SerializeField] private GameObject dialogueUI;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private static UIManager _instance;

    //PROTOSISTEMA DE DIÁLOGOS
    private bool dialogueOnGoing = false;
    private bool justStarted = false;
    private string currentName;
    private string[] currentDialogues;
    private int i; //índice para array de diálogos

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

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        dialogueUI.SetActive(false);
        MissionCompletedCanvas.gameObject.SetActive(false); //interfaz desactivada
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //PROTOSISTEMA DE DIÁLOGOS
        if (justStarted) //Ignora el primer input de interacción, ya que es el que inicializa los diálogos
                         //y no queremos que muestre el segundo diálogo antes que el primero
        {
            justStarted = false;
        }
        else if (dialogueOnGoing && InputManager.Instance.InteractWasPressedThisFrame()) //si hay un diálogo en curso y pulsa el botón de interacción
                                                                                         //se pasa al siguiente diálogo
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

    //PROTOSISTEMA DE DIÁLOGOS
    public void InitDialogues(string name, string[] dialogues) //la UI recibe el nombre y los diálogos del NPC y los inicializa en sus atributos correspondientes
    {
        dialogueUI.gameObject.SetActive(true); //activa la caja de diálogos
        currentName = name;
        currentDialogues = new string[dialogues.Length]; //inicializa su array de diálogos con el mismo tamaño que el array del NPC
        Array.Copy(dialogues, 0, currentDialogues, 0, dialogues.Length); //copiamos el array de diálogos del NPC al array de diálogos de la UI para que los pueda usar
        i = 0; //inicializamos el índice que se va a usar para pasar los diálogos

        //mostramos el nombre y el primer diálogo
        nameText.text = currentName;
        dialogueText.text = currentDialogues[i];
        dialogueOnGoing = true; //hay un diálogo en curso
        justStarted = true; //acaba de empezar el diálogo
    }

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    private void Init()
    {
    }

    //PROTOSISTEMA DE DIÁLOGOS
    private void NextDialogue()
    {
        if (i < currentDialogues.Length-1) //si no estamos en el último diálogo, pasamos al siguiente
        {
            i++;
            dialogueText.text = currentDialogues[i];
        }
        else
        {
            EndDialogue(); //fin diálogo
        }
    }
    private void EndDialogue()
    {
        dialogueUI.gameObject.SetActive(false); //desactivamos caja de diálogos
        dialogueOnGoing = false; //ya no hay un diálogo en curso
        LevelManager.Instance.EnablePlayerControls(); //LevelManager habilita los controles del _player
    }
    #endregion   

} // class UIManager 
// namespace
