//---------------------------------------------------------
// Componente para iniciar los diálogos del NPC
// Lucía Mei Domínguez López
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using

/// <summary>
/// Sirve para iniciar los diálogos con el NPC
/// </summary>
public class Interactive : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Array de los diálogos del NPC
    /// </summary>
    [SerializeField] private DialogueScript[] DialogueScripts;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// indica si el player puede interactuar con el NPC
    /// </summary>
    private bool _canStart = false;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void OnTriggerStay2D(Collider2D collision)
    {
        _canStart = true;
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        _canStart = false;
    }
    private void Start()
    {
        if ((DialogueScripts[0].name == "Scythe" && GameManager.Instance.HasBeenRead("Scythe")) || DialogueScripts[0].name == "DiscoDoor" && GameManager.Instance.QuestState == 2)
        { 
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// Si está al lado del NPC y pulsa el botón de interacción, empiezan los diálogos y tanto los controles como el NPC se deshabilitan para que no se actualicen
    /// los diálogos mientras hay otros en curso y el player no pueda moverse ni atacar ni dashear
    /// </summary>
    void Update()
    {
        if (_canStart && InputManager.Instance.InteractWasPressedThisFrame())
        {
            StartDialogue();
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    #endregion
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Llama al DialogueManager y le pasa sus DialogueScripts para que los inicialice y a UIManager para que muestre la caja de diálogos
    /// </summary>
    private void StartDialogue()
    {
        DialogueManager.Instance.StartInteraction(DialogueScripts);
        UIManager.Instance.ShowDialogueUI();
    }
    #endregion

} // class Interactive 
// namespace
