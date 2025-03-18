//---------------------------------------------------------
// Componente de los nombres y diálogos de los NPCs para el sistema de diálogos
// Lucía Mei Domínguez López
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase padre que la que heredan los NPCs que contiene un string de nombre, un array de strings donde se guardan los diálogos y los métodos necesarios para 
/// mandar la información que debe mostrar la UI en la caja de diálogos
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
    /// <summary>
    /// _name -> nombre del NPC que la UI mostrará en la caja de diálogos
    /// _canStart -> indica si el player puede interactuar con el NPC
    /// _dialogues -> diálogo guardado en un array de strings
    /// _options -> opciones de diálogo que puede elegir guardadas en un array de strings
    /// </summary>
    private string _name; 
    private bool _canStart = false;
    private string[] _dialogues;
    private string[] _options = { "Opción 1", "Opción 2" };
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Getters y setters para modificar los nombres, diálogos y opciones en las clases hijas de NPCs
    /// </summary>
    public bool CanStart
    {
        get { return _canStart; }
        protected set { _canStart = value; }
    }
    public string Name
    {
        get { return _name; }
        protected set { _name = value; }
    }
    public string[] Dialogues
    {
        get { return _dialogues; }
        protected set { _dialogues = value; }
    }
    public string[] Options
    {
        get { return _options; }
        protected set { _options = value; }
    }
    /// <summary>
    /// El NPC le pasa su nombre y diálogos a la UI. Está en virtual para que en cada hija se puedan modificar 
    /// las condiciones para mostrar un diálogo u otro
    /// </summary>
    public virtual void SetDialogues()
    {
        UIManager.Instance.InitDialogues(_name, _dialogues, false);
    }
    #endregion
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    #endregion

} // class Interactive 
// namespace
