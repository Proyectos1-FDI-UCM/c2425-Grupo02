//---------------------------------------------------------
// Colisiones entre objetos curativos y jugador para aumentar la vida.
// Isabel Serrano Martín
// Astra damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Llamada al GameManager al colisionar un objeto curativo con el 
/// player
/// </summary>
public class Healing_GameObjects : MonoBehaviour
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
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos


    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Cada vez que el jugador choca contra el GameObject, se asigna una 
    /// variable del tipo "vidas del jugador" a dicha colisión,
    /// si no es nula, llama a una instancia del LevelManager y destruye el 
    /// objeto curativo.
    /// </summary>
   
    private void OnTriggerEnter2D(Collider2D collision) 
    {      
        Player_Health playerHealth = collision.gameObject.GetComponent<Player_Health>(); 

        if (collision.gameObject.GetComponent<Movement>() != null)
        {
            GameManager.Instance.HealCollected(collision.gameObject);
            Destroy(gameObject);
        }
    }

    #endregion   

} // class Healing_GameObjects 
// namespace
