//---------------------------------------------------------
// Componente de los diálogos del NPC de misión Minos
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
public class Minos : Interactive
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

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// se vuelve true cuando el player entra en el trigger del NPC y se vuelve false cuando sale para que solo pueda interactuar con él cuando está al lado
    /// </summary>
    private void OnTriggerStay2D(Collider2D collision)
    {
        CanStart = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        CanStart = false;
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Si está al lado del NPC y pulsa el botón de interacción, empiezan los diálogos y tanto los controles como el NPC se deshabilitan para que no se actualicen
    /// los diálogos mientras hay otros en curso y el player no pueda moverse ni atacar ni dashear
    /// </summary>
    void Update()
    {
        if (CanStart && InputManager.Instance.InteractWasPressedThisFrame())
        {
            SetDialogues();
            LevelManager.Instance.DisablePlayerControls(); //LevelManager deshabilita los controles
            LevelManager.Instance.DisableNPC();
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// El NPC le pasa su nombre y diálogos a la UI. Está en virtual para que en cada hija se puedan modificar 
    /// las condiciones para mostrar un diálogo u otro
    /// 
    /// 0 -> Primer encuentro con Minos en el que te encarga la misión
    /// 1 -> La misión está en progreso y Minos aún espera sus paquetes
    /// 2 -> Has terminado la misión
    /// </summary>
    public override void SetDialogues()
    {
        Name = "Minos";
        if (GameManager.Instance.QuestState == 0)
        {
            Dialogues = new string[]
            { "Oye, tú, se nos han perdido unos paquetes.",
            "Tienes que buscarlos que buscarlos en el bosque ese que huele a choto.",
            "¿A qué esperas? Mueve el culo. -_-" };
        }
        else if(GameManager.Instance.QuestState == 1)
        {
            Dialogues = new string[]
            { "¿Qué haces aquí? Todavía te faltan paquetes." };
        }
        else if (GameManager.Instance.QuestState == 2)
        {
            Dialogues = new string[]
            { "Madre mía, ¡¡¡has tardado un siglo para traerme unos malditos paquetes!!!",
            "Espabila, que la vida te va a comer."};
        }
        UIManager.Instance.InitDialogues(Name, Dialogues, false);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class Minos 
// namespace
