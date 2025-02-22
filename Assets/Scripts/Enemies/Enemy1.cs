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
public class Enemy1 : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] float MovementSpeed;   //Velocidad de movimiento del enemigo
    [SerializeField] float RestTime;        //Tiempo que el enemigo pasa quieto después de atacar
    [SerializeField] float AttackCooldown;  //Tiempo que pasa entre cada uno de los tres ataques

    #endregion


    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    GameObject _player;         //Jugador en la escena
    Rigidbody2D _rb;            //Componente rigidBody del enemigo
    float _timer = 0f;          //Temporizador para calcular el estado del enemigo (descanso, perseguir, etc)
    bool _isResting = false;    //Bandera que marca si el enemigo está descansando
    bool _onRange = false;      //Bandera que marca si el enemigo está a la distancia necesaria del jugador para ejecutar su ataque

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Start() {
        _player = FindObjectOfType<Movement>().gameObject;
        _rb = GetComponent<Rigidbody2D>();
    }

    void Update() {
        if (!_isResting)
        {
            if (!_onRange)
            {
                Debug.Log("Persiguiendo");
                Vector2 player_position = _player.transform.position;
                Vector2 dir = GetDirection((player_position - (Vector2)transform.position));
                _rb.velocity = dir * MovementSpeed;
            }
            else
            {
                _rb.velocity = Vector2.zero;
                Attack();
                Debug.Log("Ataque");
                _isResting = true;
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
            Debug.Log("Descansando");
            _timer += Time.deltaTime;

            if (_timer >= RestTime)
            {
                _timer = 0f;
                _isResting = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        _onRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        _onRange = false;
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Esta función toma un vector y calcula su dirección.
    /// </summary>
    /// <param name="v"> Vector cuya dirección se quiere calcular </param>
    /// <returns></returns>
    Vector2 GetDirection(Vector2 v) {
        Vector2 res;
        float ang = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        ang = (ang + 360) % 360;
        
        if (ang == 0) res = Vector2.right;

        else if (ang == 90) res = Vector2.up;

        else if (ang == 180) res = Vector2.left;

        else if (ang == 270) res = Vector2.down;

        else if (ang <= 90 && ang > 0) res = (Vector2.right + Vector2.up).normalized;

        else if (ang <= 180 && ang > 90) res = (Vector2.up + Vector2.left).normalized;

        else if (ang <= 270 && ang > 180) res = (Vector2.left + Vector2.down).normalized;

        else res = (Vector2.down + Vector2.right).normalized;

        return res;
    }

    void Attack() {

    }

    #endregion

} // class Enemy1 
// namespace
