//---------------------------------------------------------
// Componente del jugador para interactuar con NPCs
// Lucía Mei Domínguez López
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.InputSystem;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Player_Interact : MonoBehaviour
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
    private Rigidbody2D rb;
    private Movement playerMovement;
    private float rayDist = 1f;
    private LayerMask interactives; //layer de NPCs interactuables

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
        rb = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<Movement>();
        interactives = LayerMask.GetMask(LayerMask.LayerToName(14));
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        //raycast para comprobar si el jugador está mirando hacia un NPC interactuable
        RaycastHit2D hit = Physics2D.Raycast(rb.position, playerMovement.GetLastDir(), rayDist, interactives);
        if (hit.collider != null && InputManager.Instance.InteractWasPressedThisFrame()) //si pulsa botón de interación y el raycast colisiona con un NPC interactuable
        {
            hit.collider.gameObject.GetComponent<Interactive>().SetDialogues(); //llama a la función de diálogos del NPC
            LevelManager.Instance.DisablePlayerControls(); //LevelManager deshabilita los controles
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    private void Interact()
    {

    }
    #endregion   

} // class Player_Interact 
// namespace
