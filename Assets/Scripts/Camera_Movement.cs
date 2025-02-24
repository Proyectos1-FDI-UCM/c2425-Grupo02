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
public class Camara : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    [SerializeField] GameObject target;

    [SerializeField] float MaxRight;
    [SerializeField] float MaxLeft;

    [SerializeField] float MaxHight;
    [SerializeField] float MinHight;

    [SerializeField] float speed;

    [SerializeField] bool runniing = true;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    private float playerPositionX;
    private float playerPositionY;

    private float PosX;
    private float PosY;
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
        PosX = playerPositionX + MaxRight;
        PosY = playerPositionY + MaxLeft;
        transform.position = Vector3.Lerp(transform.position, new Vector3(PosX, PosY, -1), 1);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        MoveCamera();
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
    void MoveCamera()
    {
        if (runniing)
        {
            if (target)
            {
                playerPositionX = target.transform.position.x;
                playerPositionY = target.transform.position.y;

                if (playerPositionX > MaxRight && playerPositionX < MaxLeft)
                {
                    PosX = playerPositionX;
                }

                if (playerPositionY > MaxHight && playerPositionY < MinHight)
                {
                    PosY = playerPositionY;
                }
            }

            transform.position = Vector3.Lerp(transform.position, new Vector3(PosX, PosY, -1), speed * Time.deltaTime);
        }

    }

    #endregion

} // class Camara 
