//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class NewBehaviourScript : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints
    [SerializeField] private Button continuarButt;
    [SerializeField] private GameObject continuarGameObject;
    [SerializeField] private Button nuevaButt;
    [SerializeField] private Button creditosButt;
    [SerializeField] private Image continuar;
    [SerializeField] private Image nueva;
    [SerializeField] private Image creditos;
    [SerializeField] private Sprite continuarSpr;
    [SerializeField] private Sprite nuevaSpr;
    [SerializeField] private Sprite creditosSpr;
    [SerializeField] private Sprite continuarSprHigh;
    [SerializeField] private Sprite nuevaSprHigh;
    [SerializeField] private Sprite creditosSprHigh;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

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
        
    }
    private void FixedUpdate()
    {
        Vector2 option = VectorDirection();
        if (option != Vector2.zero)
        {
            if (option == Vector2.down)
            {
                creditos.sprite = creditosSpr;
                nueva.sprite = nuevaSprHigh;
                continuar.sprite = continuarSpr;
            }//button.onClick.Invoke();
            else if (option == Vector2.up)
            {
                if (continuarGameObject.activeSelf == true)
                {
                    creditos.sprite = creditosSpr;
                    nueva.sprite = nuevaSpr;
                    continuar.sprite = continuarSprHigh;
                }
            }
            else if (option == Vector2.right)
            {
                creditos.sprite = creditosSprHigh;
                nueva.sprite = nuevaSpr;
                continuar.sprite = continuarSpr;
            }
            else if (option == Vector2.left && creditos.sprite == creditosSprHigh)
            {
                creditos.sprite = creditosSpr;
                nueva.sprite = nuevaSprHigh;
                continuar.sprite = continuarSpr;
            }
        }
        if (InputManager.Instance.InteractWasPressedThisFrame())
        {
            if (creditos.sprite == creditosSprHigh)
            {
                creditosButt.onClick.Invoke();
            }
            else if (nueva.sprite == nuevaSprHigh)
            {
                nuevaButt.onClick.Invoke();
            }
            else if (continuar.sprite == continuarSprHigh)
            {
                continuarButt.onClick.Invoke();
            }
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
    public Vector2 VectorDirection()  //
    {
        Vector2 moveInput = InputManager.Instance.MovementVector;
        Vector2 vector = Vector2.zero;
        if (((moveInput.x > 0 && moveInput.y > 0 || moveInput.x > 0 && moveInput.y < 0) && moveInput.x > moveInput.y) || (moveInput.x == 1 && moveInput.y == 0)) vector = new Vector2(1, 0);//derecha
        else if (((moveInput.x > 0 && moveInput.y > 0 || moveInput.x > 0 && moveInput.y < 0) && moveInput.x < moveInput.y) || (moveInput.x == 0 && moveInput.y == -1)) vector = new Vector2(0, -1);//abajo
        else if (((moveInput.x < 0 && moveInput.y > 0 || moveInput.x < 0 && moveInput.y < 0) && moveInput.x > moveInput.y) || (moveInput.x == -1 && moveInput.y == 0)) vector = new Vector2(-1,0);//izquierda
        else if (((moveInput.x < 0 && moveInput.y > 0 || moveInput.x > 0 && moveInput.y < 0) && moveInput.x < moveInput.y) || (moveInput.x == 0 && moveInput.y == 1)) vector = new Vector2(0,1);//arriba
        else vector = new Vector2(0, 0);

        return vector;
    }
    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    #endregion

} // class NewBehaviourScript 
// namespace
