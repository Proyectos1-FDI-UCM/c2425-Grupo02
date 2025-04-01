//---------------------------------------------------------
// Script que gestiona el comportamiento de los proyectiles del jefe.
//
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que gestiona los proyectiles disparados por el jefe.
/// - Los proyectiles se mueven en línea recta.
/// - Se destruyen tras un tiempo o al colisionar con un objeto.
/// - Si impactan al jugador, le causan daño.
/// </summary>
public class BossProyectile : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [Range(1, 10)]
    [SerializeField] private float speed = 10f; // Velocidad del proyectil.

    [Range(1, 10)]
    [SerializeField] private float lifeTime = 3f; // Tiempo antes de que el proyectil se destruya automáticamente.

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    private Rigidbody2D rb; // Referencia al Rigidbody2D del proyectil para controlar su movimiento.

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se ejecuta al inicio del objeto.
    /// - Obtiene el Rigidbody2D del proyectil.
    /// - Programa la destrucción del proyectil después de `lifeTime` segundos.
    /// </summary>
    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Busca el Rigidbody2D adjunto al proyectil.
        Destroy(gameObject, lifeTime); // Destruye el proyectil después del tiempo de vida definido.
    }

    /// <summary>
    /// Se ejecuta en cada frame fijo (FixedUpdate es mejor para físicas que Update).
    /// - Aplica velocidad constante al proyectil en la dirección en la que fue disparado.
    /// </summary>
    void FixedUpdate()
    {
        rb.velocity = transform.up * speed; // Mueve el proyectil en la dirección en la que apunta.
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Se ejecuta cuando el proyectil colisiona con otro objeto.
    /// - Si colisiona con el jugador, le causa daño.
    /// - Independientemente de con qué colisione, el proyectil se destruye.
    /// </summary>
    /// <param name="collision">Objeto con el que ha colisionado.</param>
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Comprueba si el objeto con el que colisionó tiene el script Player_Health.
        if (collision.gameObject.GetComponent<Player_Health>() != null)
        {
            // Si es el jugador, le causa 1 punto de daño.
            collision.gameObject.GetComponent<Player_Health>().Damage(1);
        }

        // Destruye el proyectil tras la colisión.
        Destroy(gameObject);
    }

    #endregion   

} // Fin de la clase BossProyectile
