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
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Bullet : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] private float speed;
    [SerializeField] private GameObject player;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private Rigidbody2D rb;
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
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); //inicializamos rigidbody
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Debug_Enemy>() != null)
        {
            Destroy(collision.gameObject);
        }        
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        Debug.Log("You can shoot");
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        //transform.position += -transform.up * speed * Time.deltaTime;
        Vector2 bulletDir = -transform.up * speed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + bulletDir);
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);
        //obtiene posición de la bala y lo convierte al espacio de la cámara
        //(0,1) = esquina superior izquierda    (1,1) = esquina superior derecha
        //(0,0) = esquina inferior izquierda    (1,0) = esquina inferior derecha
        if (viewPos.x < 0 || viewPos.x > 1 || viewPos.y < 0 || viewPos.y > 1) Destroy(gameObject); //destruye la bala si se sale de los márgenes de cámara
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
   
    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion   

} // class Bullet 
// namespace
