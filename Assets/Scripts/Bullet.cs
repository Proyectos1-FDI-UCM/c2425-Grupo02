//---------------------------------------------------------
// Componente de la bala del jugador
// Lucía Mei Domínguez López
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.EventSystems;
// Añadir aquí el resto de directivas using

/// <summary>
/// Clase de la bala
/// </summary>
public class Bullet : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields
    /// <summary>
    /// Velocidad de la bala
    /// </summary>
    [SerializeField] private float Velocity;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Rigidbody de la bala
    /// </summary>
    private Rigidbody2D _rb;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se inicializa el rigidbody
    /// </summary>
    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// Si colisiona con un enemigo, le produce daño.
    /// Al final, se destruye la bala
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Enemy_Health>() != null)
        {
            collision.gameObject.GetComponent<Enemy_Health>().Damage(1);
        }

        if (collision.gameObject.GetComponent<Boss_Life_Phase1>() != null)
        {
            collision.gameObject.GetComponent<Boss_Life_Phase1>().Damage(1);
        }
        Destroy(gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnDestroy()
    {
        Debug.Log("You can shoot");
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        _rb.velocity = -transform.up * Velocity;
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    #endregion

} // class Bullet 
// namespace
