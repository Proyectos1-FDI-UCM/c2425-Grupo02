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
using static DialogueScript;
// Añadir aquí el resto de directivas using


/// <summary>
/// Gestiona qué diálogos se muestran en pantalla y contiene los métodos necesarios para pasar los diálogos y finalizarlos
/// </summary>
public class DialogueManager : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField]private Image DialogueSprite;
    [SerializeField] private TextMeshProUGUI DialogueText;
    [SerializeField] private GameObject Options;
    [SerializeField] private TextMeshProUGUI Option1Text;
    [SerializeField] private TextMeshProUGUI Option2Text;
    
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private static DialogueManager _instance;
    private DialogueOption _currentOption1;
    private DialogueOption _currentOption2;

    /// <summary>
    /// Diálogo actual que se muestra en la UI
    /// </summary>
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
    private void Start()
    {
        Options.SetActive(false);
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
    /// Elige qué diálogos muestra y establece el nombre del personaje, el sprite y la línea de diálogo. 
    /// Luego indica que hay un diálogo en curso y que acaba de empezar.
    /// </summary>
    /// <param name="dialogueScripts"></param>
    public void StartInteraction(DialogueScript[] dialogueScripts)
    {
        Time.timeScale = 0;
        LevelManager.Instance.DisableBehaviours();
        ChooseDialogue(dialogueScripts);
        InitDialogues();
        _dialogueOnGoing = true;
        _justStarted = true;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Elige qué diálogo va a mostrar según el nombre del personaje y el estado del juego
    /// </summary>
    /// <param name="dialogueScripts"></param>
    private void ChooseDialogue(DialogueScript[] dialogueScripts)
    {
        if (dialogueScripts.Length > 1)
        {
            if (dialogueScripts[0].CharLines[0].CharName == "Minos")
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
            else if (dialogueScripts[0].CharLines[0].CharName == "Bartender")
            {
                if (!GameManager.Instance.SaveUsed)
                {
                    _currentDialogue = dialogueScripts[0];
                    GameManager.Instance.UpdateSave();
                }
                else
                {
                    _currentDialogue = dialogueScripts[1];
                }
            }
            else
            {
                int i = 0;
                while (i < dialogueScripts.Length - 1 && GameManager.Instance.HasBeenRead(dialogueScripts[i].name))
                {
                    i++;
                }
                _currentDialogue = dialogueScripts[i];
            }
        }
        else
        {
            _currentDialogue = dialogueScripts[0];
        }
    }


    /// <summary>
    /// Inicializa el índice de líneas de diálogo a 0 y muestra el nombre, el sprite y la primera línea de diálogo en pantalla
    /// </summary>
    private void InitDialogues()
    {
        _i = 0;
        NameText.text = _currentDialogue.CharLines[_i].CharName;
        if (_currentDialogue.CharLines[_i].CharSprite == null)
        {
            DialogueSprite.gameObject.SetActive(false);
        }
        else
        {
            DialogueSprite.gameObject.SetActive(true);
            DialogueSprite.sprite = _currentDialogue.CharLines[_i].CharSprite;
        }
        DialogueText.text = _currentDialogue.CharLines[_i].CharLineText;
    }

    /// <summary>
    /// Si no se encuentra en la última línea de diálogo, pasa al siguiente. 
    /// En caso contrario, carga las opciones si hay o termina el diálogo si no las hay
    /// </summary>
    private void NextLine()
    {
        
        
        if (_i < _currentDialogue.CharLines.Length - 1)
        {
            _i++;
            NameText.text = _currentDialogue.CharLines[_i].CharName;
            if (_currentDialogue.CharLines[_i].CharSprite == null)
            {
                DialogueSprite.gameObject.SetActive(false);
            }
            else
            {
                DialogueSprite.gameObject.SetActive(true);
                DialogueSprite.sprite = _currentDialogue.CharLines[_i].CharSprite;
            }
            DialogueText.text = _currentDialogue.CharLines[_i].CharLineText;

            if (_currentDialogue.name == "FirstMeeting" && _i == 5)
            {
                LevelManager.Instance.ShowIramis();
            }
            else if (_currentDialogue.name == "FirstMeeting" && _i == 19)
            {
                LevelManager.Instance.HideIramis();
            }
        }
        else if (_currentDialogue.CharLines[_i].CharOptions.Length > 0)
        {
            LoadOptions();
        }
        else
        {
            EndDialogue();
        }
    }

    /// <summary>
    /// Se muestra la UI de decisiones, se borran los listeners de los botones, asignamos la opción correspondiente a cada botón
    /// y se le añade un listener que se usará para cargar el siguiente diálogo. Después, se asigna el valor de las opciones 1 y 2 actuales
    /// y se muestran sus textos correspondientes en pantalla
    /// </summary>
    private void LoadOptions()
    {
        _optionsOnGoing = true;
        Options.SetActive(true);
        Button[] buttons = Options.GetComponentsInChildren<Button>();
        for (int i = 0; i < buttons.Length; i++) 
        {
            buttons[i].onClick.RemoveAllListeners();
            DialogueOption chosenOption = _currentDialogue.CharLines[_i].CharOptions[i];
            buttons[i].onClick.AddListener(() => LoadNextDialogue(chosenOption));
        }
        _currentOption1 = _currentDialogue.CharLines[_i].CharOptions[0];
        _currentOption2 = _currentDialogue.CharLines[_i].CharOptions[1];
        Option1Text.text = _currentOption1.CharOptionText;
        Option2Text.text = _currentOption2.CharOptionText;
    }
    /// <summary>
    /// Carga el siguiente diálogo según la decisión que haya tomado el jugador, oculta la UI de decisiones, inicializa el siguiente diálogo
    /// y marca que ya no hay opciones en curso
    /// </summary>
    private void LoadNextDialogue(DialogueOption option)
    {
        if (option == _currentOption1)
        {
            _currentDialogue = _currentOption1.Next;
        }
        else
        {
            _currentDialogue = _currentOption2.Next;
        }
        Options.SetActive(false);
        InitDialogues();
        _optionsOnGoing = false;
    }
    /// <summary>
    /// Si el diálogo lleva a un final del juego, carga la escena que te muestra qué final has obtenido.
    /// Si has tenido el primer diálogo con Minos, el estado del juego se actualiza para indicar que hay una misión en curso
    /// Al final del diálogo, se desactiva la caja de diálogos, se indica que ya no hay un diálogo en curso, 
    /// el levelmanager habilita los controles del player y
    /// habilita al NPC para que pueda actualizar sus diálogos y el GameManager actualiza el estado del juego.
    /// </summary>
    private void EndDialogue()
    {
        if (_currentDialogue.GameEnding == true)
        {
            GameManager.Instance.ChangeScene(10);
        }
        _dialogueOnGoing = false;
        GameManager.Instance.MarkAsRead(_currentDialogue.name);
        UIManager.Instance.HideDialogueUI();
        Time.timeScale = 1;
        LevelManager.Instance.EnableBehaviours();
    }
    #endregion

} // class DialogueManager 
// namespace
