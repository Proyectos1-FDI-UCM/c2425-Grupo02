//---------------------------------------------------------
// Script para manejo de vidas del jugador
// Isabel Serrano Martín
// Astra damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Gestiona la vida del jugador, contiene métodos que suman o quitan vida
/// dependiendo de con qué colisiona el jugador
/// </summary>
public class Player_Health : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Cantidad de vidas del jugador
    /// </summary>
    [SerializeField]
    private int Health;
    [SerializeField] private int MaxHealth;

    [SerializeField]
    private int scene;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    private void Start()
    {
        Health = GameManager.Instance.ReturnHealth();
        GameManager.Instance.SaveAndSendHealth(Health);
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Curación del jugador utilizando objetos del juego.
    /// </summary>
    ///  <param name="HealthAdded"> Número que se le va a sumar a Health </param>
    public void Heal(int HealthAdded)
    {
        if (Health + HealthAdded <= MaxHealth)
        {
            Health += HealthAdded;
            //GameManager.Instance.GetHealth();
            GameManager.Instance.SaveAndSendHealth(Health);
            Debug.Log("Vida curada: " + HealthAdded);
            Debug.Log("Vidas restantes: " + Health);
        }
    }

    /// <summary>
    /// Método que resta un entero pasado por parámetro a las vidas del jugador.
    /// </summary>
    /// <param name="dmg"> Número que se le va a restar a Health </param>
    public void Damage(int dmg) {
        if (!GameManager.Instance.HasInvulnerability)
        {
            Health -= dmg;
            Debug.Log("Salud restante" + Health);
            GameManager.Instance.SaveAndSendHealth(Health);
            //GameManager.Instance.GetHealth();
            if (Health <= 0)
            {
                GameManager.Instance.ChangeScene(scene);
            }
        }
    }

    /// <summary>
    /// Devuelve la vida actual al GameManager, quien la almacena y mantiene entre escenas
    /// </summary>
    /// <returns></returns>
    public int ReturnHealth() { return Health; }

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
