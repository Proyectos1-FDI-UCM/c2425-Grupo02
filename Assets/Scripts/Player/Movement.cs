// ---------------------------------------------------------
// Componente de movimiento del jugador
// Isabel Serrano Martín y Lucía Mei Domínguez López
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


/// <summary>
/// Componente que comunica con el InputManager para 
/// permitir el movimiento del jugador
/// </summary>
public class Movement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Velocidad del movimiento
    /// </summary>
    [SerializeField] private float Speed = 1.0f;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Rigidbody para el movimiento
    /// </summary>
    private Rigidbody2D rb;
    private Animator animator;
    private Dash dash;
    /// <summary>
    /// última dirección en la que mira el player que se obtiene en GetLastDir()
    /// </summary>
    private Vector2 lastDir;
    private Vector2 lastDir2;
    /// <summary>
    /// vector que contiene las dimensiones del mapa
    /// </summary>
    private Vector2 mapSize;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Inicializamos rigidbody, el animator y el dash
    /// </summary>
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        dash = GetComponent<Dash>();   
    }
    /// <summary>
    /// Coge el tamaño del mapa en el primer frame
    /// </summary>
    private void Start()
    {
        mapSize = LevelManager.Instance.GetMapSize();
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /// <summary>
    /// Si cambia de escena, cambia el tamaño del mapa para que sea el de la escena que se ha cargado
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        mapSize = LevelManager.Instance.GetMapSize();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        lastDir = GetLastDir();

        Vector2 movement = InputManager.Instance.MovementVector * Speed;

        //hacerlo con rb.velocity
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);

        animator.SetFloat("moveX", lastDir.x);
        animator.SetFloat("moveY", lastDir.y);
        ApplyToroidality();
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// Vector que muestra la última dirección en la que mira el player necesario para las animaciones y el disparo
    /// Coge el vector de movimiento del player y comprueba si se mueve más en vertical 
    /// y le asigna una de las cuatro direcciones que se comprueban en el
    /// siguiente orden -> derecha, izquierda, arriba, abajo
    /// </summary>
    /// <returns></returns>
    public Vector2 GetLastDir()  
    {
        Vector2 moveInput = InputManager.Instance.MovementVector; 

        if (moveInput != Vector2.zero)
        {
            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y)) 
            {
                if (moveInput.x > 0) lastDir = new Vector2(1, 0); 
                else lastDir = new Vector2(-1, 0); 
            }
            else 
            {
                if (moveInput.y > 0) lastDir = new Vector2(0, 1);
                else lastDir = new Vector2(0, -1); 
            }
        }
        return lastDir;
    }
    /// <summary>
    /// lastdir para el dash
    /// </summary>
    /// <returns></returns>
    public Vector2 GetLastDir2()  //
    {
        Vector2 moveInput = InputManager.Instance.MovementVector;


        if (moveInput.x == 0 && moveInput.y > 0) lastDir2 = new Vector2(0, 1);
        else if (moveInput.x == 0 && moveInput.y < 0) lastDir2 = new Vector2(0, -1);
        else if (moveInput.x > 0 && moveInput.y == 0) lastDir2 = new Vector2(1, 0);
        else if (moveInput.x < 0 && moveInput.y == 0) lastDir2 = new Vector2(-1, 0);
        else if (moveInput.x > 0 && moveInput.y > 0) lastDir2 = new Vector2(1, 1);
        else if (moveInput.x > 0 && moveInput.y < 0) lastDir2 = new Vector2(1, -1);
        else if (moveInput.x < 0 && moveInput.y > 0) lastDir2 = new Vector2(-1, 1);
        else if (moveInput.x < 0 && moveInput.y < 0) lastDir2 = new Vector2(-1, -1);
        else lastDir2 = new Vector2(0, 0);

        return lastDir2;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Aplica la toroidalidad cuando el player se sale de los límites del mapa
    /// Orden en el que se comprueban las posiciones del player -> derecha, izquierda, arriba, abajo
    /// </summary>
    private void ApplyToroidality()
    {
        Vector2 offset = Vector2.zero;
        if (transform.position.x > mapSize.x / 2)
        {
            offset.x = -mapSize.x; 
        }
        else if (transform.position.x < -mapSize.x / 2)
        {
            offset.x = mapSize.x;
        }
        else if (transform.position.y > mapSize.y / 2)
        {
            offset.y = -mapSize.y;
        }
        else if (transform.position.y < -mapSize.y / 2)
        {
            offset.y = mapSize.y;
        }
        rb.transform.Translate(offset, Space.World);
    }
    #endregion

} //class Movement
  // namespace