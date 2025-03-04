//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Enemy_Health : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] int Health; //Vidas del enemigo

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    Enemy_Spawn _spawn;         //Spawn que instanció al enemigo

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    


    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Cuando se llama, se le resta el daño pasado por parametro a Health.
    /// Si Health llega a 0 el enmigo se destruye.
    /// </summary>
    /// <param name="dmg"> Entero que se le resta a Health </param>
    public void Damage(int dmg) {
        Health -= dmg;
        if (Health <= 0)
        {
            _spawn.SubstractEnemy();
            Destroy(gameObject);
        }
    }

    public void SetSpawn(Enemy_Spawn spawn) {
        _spawn = spawn;
    }

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class Enemy_health 
// namespace
