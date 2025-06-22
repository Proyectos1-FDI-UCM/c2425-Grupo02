//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using System.Collections;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Radar : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] float waveDuration = 2f; // Duración de la onda antes de desaparecer.
    [SerializeField] float waveSpeed = 5f; // Velocidad de movimiento de la onda.
    [SerializeField] GameObject ondaPrefab; // Prefab de la onda.
    [SerializeField] Transform paquete; // Referencia al paquete
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private bool puedeUsar = true; // Determina si se puede usar el radar o no.
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se ejecuta cada frame.
    /// - Llama al método del radar si se cumplen todas las condiciones.
    /// </summary>
    void Update()
    {
        if(InputManager.Instance.UseRadarWasPressedThisFrame() && paquete != null && puedeUsar == true) // Si se pulsa la r, existe el paquete y se puede usar
        {
            puedeUsar = false; // Como está la onda en pantalla, no se puede mandar otra.
            UsarRadar(); // Llamamos al método.
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    /// <summary>
    /// Controla todo el comportamiento de la onda del radar.
    /// - Crea la onda.
    /// - Empieza la corrutina del movimiento de la onda.
    /// </summary>
    void UsarRadar()
    {
        GameObject onda = Instantiate(ondaPrefab, transform.position, Quaternion.identity); // Creamos la onda a partir del prefab.
        Vector2 direccion = (paquete.position - transform.position).normalized; // Calculamos la dirección del paquete.
        RotateTowardsTarget(onda, direccion); // Rotamos la onda hacia el paquete.
        StartCoroutine(MoverOnda(onda, direccion)); // Comenzamos el movimiento de la onda.
    }
    /// <summary>
    /// Corrutina del movimiento de la onda.
    /// - Se repite mientras dure la onda.
    /// - La va haciendo progresivamente más transparente hasta que desaparece.
    /// </summary>
    IEnumerator MoverOnda(GameObject onda, Vector2 direccion)
    {
        float tiempo = 0f; // Tiempo que irá incrementando.
        Vector3 posInicial = onda.transform.position; // Posición inicial de la onda.
        while (tiempo < waveDuration) // Mientras que el valor del tiempo sea menor que la duración de la onda.
        {
            tiempo += Time.deltaTime; // Sumamos valor al tiempo.
            onda.transform.position += (Vector3)direccion * waveSpeed * Time.deltaTime; // Movemos la onda hacia el paquete.
            SpriteRenderer sr = onda.GetComponent<SpriteRenderer>(); // Obtenemos el sprite renderer.
            if (sr != null) // Mientras exista el sprite.
            {
                Color c = sr.color; // Obtenemos el color.
                c.a = 1f - (tiempo / 2f); // Aplicamos la transparencia en ese tiempo.
                sr.color = c; // Aplicamos el color después de la transparencia al sprite.
            }
            yield return null; // Pausamos y continuamos en el siguiente frame.
        }
        puedeUsar = true; // Se puede usar de nuevo el radar.
        Destroy(onda); // Destruimos la onda.
    }
    /// <summary>
    /// Rota la onda para que se vea bien el sprite.
    /// - Usa interpolación (Slerp) para girar la onda.
    /// </summary>
    private void RotateTowardsTarget(GameObject onda, Vector2 direccion)
    {
        float angle = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg - 90f; // Convierte a ángulo la dirección.
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, angle)); // Crea la rotación.
        onda.transform.localRotation = Quaternion.Slerp(onda.transform.rotation, q, 100f); // Rota la onda.
    }

    #endregion   

} // class Radar 
// namespace
