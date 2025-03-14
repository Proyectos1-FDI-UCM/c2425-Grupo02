//---------------------------------------------------------
// Script para manejo de vidas del jugador
// Isabel Serrano Martín
// Astra damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Player_Health : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    //cantidad de vidas del jugador
    [SerializeField]
    private int Health;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// Inicialización de las vidas del jugador    
    /// </summary>
    void Start()
    {
        Health = 3; 
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Curación del jugador utilizando objetos del juego.
    /// </summary>
    public void Heal(int HealthAdded)
    {
        Health += HealthAdded;
        Debug.Log("Vida curada: " + HealthAdded);
        Debug.Log("Vidas restantes: " + Health);
    }

    /// <summary>
    /// Método que resta un entero pasado por parámetro a las vidas del jugador.
    /// </summary>
    /// <param name="dmg"> Número que se le va a restar a Health </param>
    public void Damage(int dmg) {
        Health -= dmg;
        Debug.Log("Salud restante" + Health);
    }
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class Player_Health 
// namespace
