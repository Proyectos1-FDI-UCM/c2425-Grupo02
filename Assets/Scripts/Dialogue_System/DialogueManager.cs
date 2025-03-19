//---------------------------------------------------------
// Gestor de diálogos
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
/// Gestiona qué diálogos se muestran en pantalla y contiene los métodos necesarios para pasar los diálogos y finalizarlos
/// </summary>
public class DialogueManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private Image DialogueSprite;
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private GameObject Options;
    [SerializeField] private TextMeshProUGUI Option1Text;
    [SerializeField] private TextMeshProUGUI Option2Text;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private static DialogueManager _instance;
    private DialogueScript _currentDialogue;
    /// <summary>
    /// Indica si hay un diálogo en curso. Sirve para poder pasar diálogos solo si hay diálogoss
    /// </summary>
    private bool _dialogueOnGoing = false;
    /// <summary>
    /// indica si hay opciones en curso
    /// </summary>
    private bool _optionsOnGoing = false;
    /// <summary>
    /// indica si el diálogo acaba de empezar. Sirve para que no se omita la primera frase al ejecutarse el update
    /// </summary>
    private bool _justStarted = false;
    /// <summary>
    /// índice para el array de diálogos
    /// </summary>
    private int _i;
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
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// 
    /// En if (justStarted) se ignora el primer input que inicia la interacción para evitar que omita la primera frase
    /// y en el siguiente bloque permite al jugador pasar al siguiente diálogo si hay un diálogo en curso, no hay opciones en curso
    /// y pulsa el botón de interacción
    /// </summary>
    void Update()
    {
        if (_justStarted)
        {
            _justStarted = false;
        }
        else if (_dialogueOnGoing && InputManager.Instance.InteractWasPressedThisFrame() && !_optionsOnGoing)
        {
            NextLine();
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// la UI activa la caja de diálogos, recibe el nombre y los diálogos del NPC y los inicializa en sus atributos correspondientes.
    /// El array de diálogos de la UI se inicializa con el mismo tamaño que el array del NPC y copiamos el del NPC al de la UI para que los pueda usar
    /// Luego inicializa el índice que se va a usar para pasar los diálogos y muestra el nombre y el primer diálogo y finalmente se indica que hay un
    /// diálogo en curso y que el diálogo acaba de empezar.
    /// </summary>
    public static DialogueManager Instance
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
    /// <summary>
    /// Elige qué diálogos muestra y establece el nombre del personaje, el sprite y la línea de diálogo. Indica que hay un diálogo en curso y que acaba de empezar
    /// </summary>
    /// <param name="dialogueScripts"></param>
    public void InitDialogues(DialogueScript[] dialogueScripts)
    {
        ChooseDialogue(dialogueScripts);
        _i = 0;
        NameText.text = _currentDialogue.CharName;
        DialogueSprite.sprite = _currentDialogue.CharSprite;
        DialogueText.text = _currentDialogue.CharDialogue[_i].CharLine;
        _dialogueOnGoing = true;
        _justStarted = true;
    }

    public void optionsDialogue()
    {
        Debug.Log("Estamos en lo de options");
        Option1Text.text = "Final malo";
        Option2Text.text = "Nada";
        Options.gameObject.SetActive(true);
    }
    public void optionSelected()
    {
        _optionsOnGoing = false;
        NextLine();
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /// <summary>
    /// Elige qué diálogo va a mostrar según el estado del juego
    /// </summary>
    /// <param name="dialogueScripts"></param>
    private void ChooseDialogue(DialogueScript[] dialogueScripts)
    {
        if (dialogueScripts[0].CharName == "Minos")
        {
            if (GameManager.Instance.QuestState == 0)
            {
                _currentDialogue = dialogueScripts[0];
            }
            else if (GameManager.Instance.QuestState == 1)
            {
                _currentDialogue = dialogueScripts[1];
            }
            else 
            {
                _currentDialogue = dialogueScripts[2];
            }
        }
    }
    /// <summary>
    /// Si se encuentra en el último diálogo, pasa al siguiente, pero si no finaliza el diálogo
    /// </summary>
    private void NextLine()
    {
        if (_i < _currentDialogue.CharDialogue.Length - 1)
        {
            _i++;
            DialogueText.text = _currentDialogue.CharDialogue[_i].CharLine;
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
        UIManager.Instance.HideDialogueUI();
        Options.gameObject.SetActive(false);
        _dialogueOnGoing = false;
        LevelManager.Instance.EnablePlayerControls();
        LevelManager.Instance.EnableNPC();
        GameManager.Instance.UpdateState();
    }
    #endregion

} // class DialogueManager 
// namespace
