//---------------------------------------------------------
// Componente de los nombres y diálogos de  los NPCs correspondientes
// Lucía Mei Domínguez López
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Interactive : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private string charName = "NPC"; //nombre NPC
    private bool interactionDone = false; //si es true, activa los nuevos diálogos
    private bool canStart = false;
    //arrays de diálogos
    private string[] firstDialogues = {"Hola, quiero encargarte una misión.", "Busca 3 paquetes y entrégamelos.", "A ver si los encuentras :P"};
    private string[] secondDialogues = { "Ya has hablado conmigo, ¿qué más quieres que te diga? :/" };
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
    private void OnEnable()
    {
        
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        canStart = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        canStart = false;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (canStart && InputManager.Instance.InteractWasPressedThisFrame())
        {
            SetDialogues();
            LevelManager.Instance.DisablePlayerControls(); //LevelManager deshabilita los controles
            LevelManager.Instance.DisableInteractive(); //LevelManager deshabilita que el NPC cambie diálogos
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    public void SetDialogues() //el NPC le pasa su nombre y diálogos a la UI
    {
        if (!interactionDone) //diálogo que se manda si es la primera vez que interactúa con él
        {
            UIManager.Instance.InitDialogues(charName, firstDialogues);
            interactionDone = true;
        }
        else //diálogo que se muestra si ya ha interactuado con él
        {
            UIManager.Instance.InitDialogues(charName, secondDialogues);
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

} // class Interactive 
// namespace
