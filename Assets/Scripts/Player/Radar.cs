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
    [SerializeField] float cooldown = 3f;
    [SerializeField] GameObject ondaPrefab; // Un círculo simple
    [SerializeField] Transform paquete;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints
    private bool puedeUsar = true;
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
    void Start()
    {
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if(InputManager.Instance.UseRadarWasPressedThisFrame())
        {
            UsarRadar();
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

    void UsarRadar()
    {
        // Crear onda
        GameObject onda = Instantiate(ondaPrefab, transform.position, Quaternion.identity);

        // Calcular dirección al paquete
        Vector2 direccion = (paquete.position - transform.position).normalized;

        // Mover la onda
        StartCoroutine(MoverOnda(onda, direccion));

        // Cooldown
        puedeUsar = false;
        Invoke("ResetearCooldown", cooldown);
    }

    IEnumerator MoverOnda(GameObject onda, Vector2 direccion)
    {
        float tiempo = 0f;
        Vector3 posInicial = onda.transform.position;

        while (tiempo < 2f)
        {
            tiempo += Time.deltaTime;

            // Mover hacia el paquete
            onda.transform.position += (Vector3)direccion * 100f * Time.deltaTime;

            // Hacer más transparente
            SpriteRenderer sr = onda.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color c = sr.color;
                c.a = 1f - (tiempo / 2f);
                sr.color = c;
            }

            yield return null;
        }

        Destroy(onda);
    }

    void ResetearCooldown()
    {
        puedeUsar = true;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 30), puedeUsar ? "[R] Radar listo" : "Radar en cooldown");
    }
    #endregion   

} // class Radar 
// namespace
