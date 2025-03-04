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
    Vector2 _position;
    Camera _cam;
    float _xLimit;
    float _yLimit;

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
    void Update() {
        MoveCamera();
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    


    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    
    void MoveCamera() {
        _position = _player.transform.position;
        _position.x = Mathf.Clamp(_position.x, -_xLimit, _xLimit);
        _position.y = Mathf.Clamp(_position.y, -_yLimit, _yLimit);
        transform.position = new Vector3( _position.x, _position.y, -10);
        Debug.Log(_xLimit);
    }

    #endregion

} // class Camara 


