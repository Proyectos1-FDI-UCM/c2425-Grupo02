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
    [SerializeField] private float Velocity;
    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Rigidbody para el movimiento
    /// </summary>
    private Rigidbody2D _rb;
    private Animator _animator;
    private Dash _dash;
    /// <summary>
    /// última dirección en la que mira el player que se obtiene en GetLastDir()
    /// </summary>
    private Vector2 _lastDir;
    private Vector2 _lastDir2;
    /// <summary>
    /// vector que contiene las dimensiones del mapa
    /// </summary>
    private Vector2 _mapSize;
    /// <summary>
    /// Posición donde va a aparecer el jugador al cargar una escena
    /// </summary>
    private Vector2 _spawnPos;
    /// Sonido de dash del jugador
    /// </summary>

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    /// <summary>
    /// Inicializamos rigidbody, el animator y el dash
    /// </summary>
    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _dash = GetComponent<Dash>();   
    }
    /// <summary>
    /// Coge el tamaño del mapa en el primer frame
    /// </summary>
    private void Start()
    {
        _mapSize = LevelManager.Instance.GetMapSize();
        _spawnPos = GameManager.Instance.GetSpawnPoint();
        _rb.transform.position = _spawnPos;
        _rb.velocity = Vector2.zero;
       
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
        _mapSize = LevelManager.Instance.GetMapSize();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        _lastDir = GetLastDir();
        Vector2 movement = InputManager.Instance.MovementVector;
        _rb.velocity = movement * Velocity;
        _animator.SetFloat("moveX", _lastDir.x);
        _animator.SetFloat("moveY", _lastDir.y);
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
                if (moveInput.x > 0) _lastDir = new Vector2(1, 0); 
                else _lastDir = new Vector2(-1, 0); 
            }
            else 
            {
                if (moveInput.y > 0) _lastDir = new Vector2(0, 1);
                else _lastDir = new Vector2(0, -1); 
            }
        }
        return _lastDir;
    }
    /// <summary>
    /// lastdir para el dash
    /// </summary>
    /// <returns></returns>
    public Vector2 GetLastDir2()  //
    {
        Vector2 moveInput = InputManager.Instance.MovementVector;


        if (moveInput.x == 0 && moveInput.y > 0) _lastDir2 = new Vector2(0, 1);
        else if (moveInput.x == 0 && moveInput.y < 0) _lastDir2 = new Vector2(0, -1);
        else if (moveInput.x > 0 && moveInput.y == 0) _lastDir2 = new Vector2(1, 0);
        else if (moveInput.x < 0 && moveInput.y == 0) _lastDir2 = new Vector2(-1, 0);
        else if (moveInput.x > 0 && moveInput.y > 0) _lastDir2 = new Vector2(1, 1);
        else if (moveInput.x > 0 && moveInput.y < 0) _lastDir2 = new Vector2(1, -1);
        else if (moveInput.x < 0 && moveInput.y > 0) _lastDir2 = new Vector2(-1, 1);
        else if (moveInput.x < 0 && moveInput.y < 0) _lastDir2 = new Vector2(-1, -1);
        else _lastDir2 = new Vector2(0, 0);

        return _lastDir2;
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
        if (transform.position.x > _mapSize.x / 2)
        {
            offset.x = -_mapSize.x; 
        }
        else if (transform.position.x < -_mapSize.x / 2)
        {
            offset.x = _mapSize.x;
        }
        else if (transform.position.y > _mapSize.y / 2)
        {
            offset.y = -_mapSize.y;
        }
        else if (transform.position.y < -_mapSize.y / 2)
        {
            offset.y = _mapSize.y;
        }
        _rb.transform.Translate(offset, Space.World);
    }
    #endregion

} //class Movement
  // namespace