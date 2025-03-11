//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEditorInternal;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Enemy2_attack : MonoBehaviour, IAttack
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] int Damage;   //Cantidad de vidas que resta el enemigo al jugador con cada golpe de su ataque

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    GameObject _hitbox;             //Objeto que contiene el collider del ataque (desde ahora será llamado "hitbox" en los comentarios)
    Enemy2_movement _mov;            //Script de movimiento del enemigo
    Vector2 _dir;                   //Dirección en la que se mueve el enemigo

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start() {
        _hitbox = transform.GetChild(0).gameObject;
        _mov = GetComponent<Enemy2_movement>();
        _hitbox.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.GetComponent<Player_Health>() != null)
        {
            collision.GetComponent<Player_Health>().Damage(Damage);
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos



    /// <summary>
    /// Método que llama a la corrutina encargada de activar la hitbox y la desactiva pasado un breve lapso de tiempo.
    /// </summary>

    public IEnumerator Attack() {
        _hitbox.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        _hitbox.SetActive(false);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados



    #endregion   


} // class Enemy2_attack 
// namespace
