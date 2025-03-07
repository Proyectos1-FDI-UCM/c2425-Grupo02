//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Enemy1_Attack : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] int Damage;            //Cantidad de vidas que resta el enemigo al jugador con cada golpe de su ataque
    [SerializeField] float AttackCooldown;  //Tiempo que pasa entre cada golpe del ataque

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    GameObject _hitbox;             //Objeto que contiene el collider del ataque (desde ahora será llamado "hitbox" en los comentarios)
    Rigidbody2D _rb;
    Enemy1_Movement _mov;            //Script de movimiento del enemigo
    Vector2 _dir;                   //Dirección en la que se mueve el enemigo
    bool _attacking = false;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _hitbox = transform.GetChild(0).gameObject;
        _mov = GetComponent<Enemy1_Movement>();
        _hitbox.SetActive(false);
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.GetComponent<Player_Health>() != null)
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
    public void Attack() { 
         if(!_attacking) StartCoroutine(AttackCoroutine());
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Método que dictamina la posición y rotación de la hitbox del ataque
    /// </summary>
    /// <param name="v"> Vector de dirección del jugador. Es usado para determinar la posición y rotación de la hitbox </param>
    /// <param name="offset"> Float que indica cuánto desplazamos la hitbox con respecto al centro del enemigo </param>
    void SetDir(Vector2 v, float offset) {
        Vector2 res;
        _hitbox.transform.rotation = transform.rotation;

        if(v.y == 0)
        {
            if (v.x > 0) res = new Vector2(offset, 0);
            else if (v.x < 0) res = new Vector2(-offset, 0);
            else res = Vector2.zero;
        }
        else
        {
            if (v.y > 0) res = new Vector2(0, offset);
            else res = new Vector2(0, -offset);

            _hitbox.transform.Rotate(0, 0, 90, Space.World);

        }
        _hitbox.transform.localPosition = res;
    }

    /// <summary>
    /// Corrutina que activa y desactiva la hitbox 3 veces seguidas
    /// </summary>
    /// <returns></returns>
    IEnumerator AttackCoroutine() {
        _attacking = true;
        _rb.mass += 1000;
        _dir = _mov.GetDir();
        SetDir(_dir, 0.25f);

        for (int i = 0; i < 3; i++)
        {
            _hitbox.SetActive(true);
            yield return new WaitForSecondsRealtime(0.1f);
            _hitbox.SetActive(false);
            yield return new WaitForSecondsRealtime(AttackCooldown);
        }
        _rb.mass -= 1000;
        _attacking = false;
    }

    #endregion   

} // class Enemy1_attack 
// namespace
