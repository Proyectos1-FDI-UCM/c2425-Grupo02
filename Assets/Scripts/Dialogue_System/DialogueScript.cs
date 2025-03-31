//---------------------------------------------------------
// Componente para el contenido de los diálogos
// Lucía Mei Domínguez López
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

/// <summary>
/// Contiene el nombre del personaje, el sprite, las líneas de diálogo y opciones si tiene.
/// </summary>
[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue")]
public class DialogueScript : ScriptableObject
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] private DialogueLine[] Lines;
    [SerializeField] private bool Ending;

    /// <summary>
    /// CLase para la línea de diálogo. Contiene la línea de diálogo y el array de opciones
    /// </summary>
    [System.Serializable]
    public class DialogueLine
    {
        [SerializeField] private string Name;
        [SerializeField] private Sprite Sprite;
        [SerializeField] [TextArea(2,5)] private string DialogueText;
        [SerializeField] private DialogueOption[] Options;
    /// <summary>
    /// Getter para obtener el nombre del NPC
    /// </summary>
    public string CharName
    {
        get { return Name; }
    }
            /// <summary>
    /// Getter para obtener el sprite del NPC
    /// </summary>
    public Sprite CharSprite
    {
        get { return Sprite; }
    }
    /// <summary>
    /// Getter para obtener la línea de diálogo del NPC
    /// </summary>
    public string CharLineText
    {
        get { return DialogueText; }
    }
    /// <summary>
    /// Getter para el texto de la línea
    /// </summary>
    public DialogueOption[] CharOptions
    {
        get { return Options; }
    }
    }
    /// <summary>
    /// CLase para las opciones de diálogo. Contiene el texto de la opción y el diálogo al que le lleva después
    /// </summary>
    [System.Serializable]
    public class DialogueOption
    {
        [SerializeField] [TextArea(2, 5)] private string OptionText;
        /// <summary>
        /// El siguiente diálogo que se carga después de elegir una opción
        /// </summary>
        [SerializeField] private DialogueScript NextDialogue;
        /// <summary>
        /// Getter del texto de opción
        /// </summary>
        public string CharOptionText
        {
            get { return OptionText; }
        }
        /// <summary>
        /// Getter del siguiente diálogo
        /// </summary>
        public DialogueScript Next
        {
            get { return NextDialogue; }
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos


    /// <summary>
    /// Getter para obtener los diálogos del NPC
    /// </summary>
    public DialogueLine[] CharLines
    {
        get { return Lines; }
    }
    /// <summary>
    /// Getter para comprobar si el diálogo lleva a un final del juego
    /// </summary>
    public bool GameEnding
    {
        get { return Ending; }
    }
    #endregion
} // class DialogueScript 
// namespace
