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
public class Camara : MonoBehaviour {
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// LevelManager de la escena
    /// </summary>
    LevelManager _level;
    /// <summary>
    /// Jugador en la escena
    /// </summary>
    GameObject _player;
    /// <summary>
    /// Posición del jugador en la escena
    /// </summary>
    Vector2 _playerPosition;
    /// <summary>
    /// Componente camera
    /// </summary>
    Camera _cam;
    /// <summary>
    /// Longitud horizontal del mapa (límite horizontal)
    /// </summary>
    float _xLimit;
    /// <summary>
    /// Longitud vertical del mapa (límite vertical)
    /// </summary>
    float _yLimit;
    /// <summary>
    /// Máxima distancia que se puede separar la cámara del jugador antes de transportarse directamente a él
    /// </summary>
    Vector2 _maxDistance = new Vector2(15, 15);

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se obtienen componentes de la cámara, así como el level manager y jugador de la escena.
    /// Se establecen los límites del mapa.
    /// </summary>
    void Start() {
        _cam = GetComponent<Camera>();
        _level = FindObjectOfType<LevelManager>();
        _player = FindObjectOfType<Player_Health>().gameObject;
        Vector2 limits = _level.GetMapSize() / 2;
        float height = _cam.orthographicSize;
        float width = height * _cam.aspect;
        _xLimit = limits.x - width;
        _yLimit = limits.y - height;
    }

    /// <summary>
    /// En cada frame (fijo) se llama a "MoveCamera"
    /// </summary>
    void FixedUpdate() {
        MoveCamera();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    


    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    
    /// <summary>
    /// Si la distancia entre el jugador y la cámara es menor que _maxDistance, esta se mueve siguiendo al jugador suavemente.
    /// En caso contrario, se transporta instantáneamente hasta el jugador.
    /// La cámara no puede moverse más allá de los límites del mapa (marcados por el level manager)
    /// </summary>
    void MoveCamera() {
        _playerPosition = _player.transform.position;
        _playerPosition.x = Mathf.Clamp(_playerPosition.x, -_xLimit, _xLimit);
        _playerPosition.y = Mathf.Clamp(_playerPosition.y, -_yLimit, _yLimit);
        Vector2 dis = _playerPosition - (Vector2)transform.position;
        if (Mathf.Abs(dis.x) >= _maxDistance.x || Mathf.Abs(dis.y) >= _maxDistance.y)
        {
            transform.position = new Vector3(_playerPosition.x, _playerPosition.y, -10);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(_playerPosition.x, _playerPosition.y, -10), Time.deltaTime * 3f);
        }
    }

    #endregion

} // class Camara 


