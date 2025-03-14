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

    LevelManager _level;
    GameObject _player;
    Vector2 _playerPosition;
    Camera _cam;
    float _xLimit;
    float _yLimit;
    Vector2 _maxDistance = new Vector2(15, 15);       //Máxima distancia que se puede separar la cámara del jugador

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

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
    /// Update is called every frame, if the MonoBehaviour is enabled.
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


