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
public class Enemy_movement : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    //[SerializeField] Enemy1_attack AttackScript;    //Script de ataque correspondiente al enemigo
    [SerializeField] float MovementSpeed;           //Velocidad de movimiento del enemigo
    [SerializeField] float RestTime;                //Tiempo que el enemigo pasa quieto después de atacar
    [SerializeField] float AttackCooldown;          //Tiempo que pasa entre cada uno de los tres ataques

    #endregion


    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    Enemy1_attack AttackScript;
    SpriteRenderer _spriteRend;
    GameObject _player;                     //Jugador en la escena
    Rigidbody2D _rb;                        //Componente rigidBody del enemigo
    Animator _anim;                         //Componente animator del enemigo
    Vector2 _dir = Vector2.zero;            //Vector que marca la dirección hacia la que se mueve el enemigo
    float _restTimer = 0f;                  //Temporizador que cuenta el tiempo de descanso
    float _dirTimer = 0f;                   //Temporizador que cuenta el tiempo necesario para que el enemigo cambie su dirección de movimiento
    int _rangeLayer;                        //Entero que representa la capa donde esta el objeto que marca el rango efctivo del enemigo
    bool _isResting = false;                //Bandera que marca si el enemigo está descansando
    bool _onRange = false;                  //Bandera que marca si el enemigo está a la distancia necesaria del jugador para ejecutar su ataque

    #endregion


    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    void Start() {
        _player = FindObjectOfType<Movement>().gameObject;
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        AttackScript = GetComponent<Enemy1_attack>();
        _rangeLayer = FindObjectOfType<Enemy_range>().gameObject.layer;
        _spriteRend = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate() {
        if (!_isResting)
        {
            if (!_onRange)
            {
                Vector2 player_position = _player.transform.position;

                if (_dirTimer >= 0.25f)
                {
                    _dir = GetDirection(player_position - (Vector2)transform.position);
                    _dirTimer = 0f;
                }
                else _dirTimer += Time.deltaTime;

                _rb.velocity = _dir * MovementSpeed;
            }
            else
            {
                _rb.velocity = Vector2.zero;
                AttackScript.Attack(_dir);
                _anim.SetTrigger("_Attack");
                _isResting = true;
            }
        }
        else
        {
            _rb.velocity = Vector2.zero;
            _restTimer += Time.deltaTime;

            if (_restTimer >= RestTime)
            {
                _restTimer = 0f;
                _isResting = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.layer == _rangeLayer) _onRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == _rangeLayer) _onRange = false;
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    public Vector2 GetDir() {
        return _dir;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    /// <summary>
    /// Esta función toma un vector y calcula su dirección (8 direcciones posibles).
    /// Tiene un poco de margen en las direcciones no diagonales.
    /// También llama al método privado SetAnim.
    /// </summary>
    /// <param name="v"> Vector cuya dirección se quiere calcular </param>
    /// <returns></returns>
    Vector2 GetDirection(Vector2 v) {
        Vector2 res;
        float ang = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        ang = (ang + 360) % 360;

        //Derecha
        if (ang <= 15 && ang >= 0 || ang <= 360 && ang >= 345) res = Vector2.right;
        //Arriba
        else if (ang <= 105 && ang >= 75) res = Vector2.up;
        //Izquierda
        else if (ang <= 195 && ang >= 165) res = Vector2.left;
        //Abajo
        else if (ang <= 285 && ang >= 255) res = Vector2.down;
        //1er cuadrante
        else if (ang <= 90 && ang >= 0) res = (Vector2.right + Vector2.up).normalized;
        //2do cuadrante
        else if (ang <= 180 && ang > 90) res = (Vector2.up + Vector2.left).normalized;
        //3er cuadrante
        else if (ang <= 270 && ang > 180) res = (Vector2.left + Vector2.down).normalized;
        //4to cuadrante
        else res = (Vector2.down + Vector2.right).normalized;

        SetAnim(res);

        return res;
    }

    /// <summary>
    /// Dado un vector, lo evalua y se encarga de asignar el valor correcto a los booleanos del controlador de animaciones del enemigo.
    /// También se encarga de voltear el sprite si es necesario.
    /// </summary>
    /// <param name="v"> Vector que va a ser evaluado </param>
    void SetAnim(Vector2 v) {
        _spriteRend.flipX = false;

        if (v.x > 0)
        {
            _anim.SetBool("_MvSide", true);
        }
        else if (v.x < 0)
        {
            _anim.SetBool("_MvSide", true);
            _spriteRend.flipX = true;
            
        }
        else
        {
            _anim.SetBool("_MvSide", false);
        }

        if (v.y > 0)
        {
            _anim.SetBool("_MvUp", true);
            _anim.SetBool("_MvDown", false);
        }
        else if (v.y < 0)
        {
            _anim.SetBool("_MvUp", false);
            _anim.SetBool("_MvDown", true);
        }
        else
        {
            _anim.SetBool("_MvUp", false);
            _anim.SetBool("_MvDown", false);
        }
    }

    #endregion

} // class Enemy1 
// namespace
