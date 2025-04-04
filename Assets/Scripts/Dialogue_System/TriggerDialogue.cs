//---------------------------------------------------------
// Componente para mostrar un diálogo cuando el jugador entra en un trigger
// Lucía Mei Domínguez López
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Sirve para iniciar un diálogo cuando el jugador entra en un trigger
/// </summary>
public class TriggerDialogue : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// Diálogo que se manda al dialogue manager para que lo muestre. Tiene que ser un array para que funcione con el mismo método que los demás diálogos
    /// </summary>
    [SerializeField] private DialogueScript[] Dialogue;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Necesario para que al regresar a la escena no se vuelvan a activar los colliders
    /// </summary>
    private void Start()
    {
        if (GameManager.Instance.TrigDialogueIsDisabled(TriggerName))
        {
            gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OYEEEE");
        StartDialogue();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController
    public string TriggerName
    {
        get { return Dialogue[0].name; }
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Llama al DialogueManager y le pasa sus DialogueScripts para que los inicialice y a UIManager para que muestre la caja de diálogos
    /// </summary>
    private void StartDialogue()
    {
        DialogueManager.Instance.StartInteraction(Dialogue);
        UIManager.Instance.ShowDialogueUI();
    }
    #endregion   

} // class TriggerDialogue 
// namespace
