//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase pública de la que heredará la clase de comportamiento de cada enemigo
/// Esta clase se encarga de asignar uno de tres estados (spawning, chasing y resting) al enemigo.
///     
///     Spawning: Al ser instanciado en escena, el enemigo no se moverá ni tendrá collider durante el tiempo especificado por SpawnTime.
///               Después de esto, pasará a chasing.
///     
///     Chasing: El enemigo perseguirá al jugador hasta llegar a rango para atacarlo.
///              Al llegar a rango de ataque, pasará a attacking.
///                 
///     Attacking: El enemigo ejecutará una corrutina de ataque que variará dependiendo de la clase de ataque del enemigo.
///                Después de esto, pasará a resting.
///     
///     Resting: Se activa una corrutina durante la que el enemigo no se moverá ni podrá ser movido pero si tendrá colisiones.
///              Después de esto, volverá a chasing.
///                 
/// Más estados podrán ser añadidos en las clases que hereden de esta.
/// </summary>
public class Enemy_StateMachine : MonoBehaviour {
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] protected float SpawnTime;               //Tiempo que tarda en spawnear el enemigo
    [SerializeField] protected float MovementSpeed;           //Velocidad de movimiento del enemigo
    [SerializeField] protected float RestTime;                //Tiempo que el enemigo pasa quieto después de atacar
    [SerializeField] protected int EnemyRangeLayer;           //Layer de rango del enemigo

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (protected fields)

    protected IAttack _attack;                        //Interfaz de script de ataque del enemigo
    protected SpriteRenderer _spriteRend;             //Componente sprite renderer del enemigo
    protected GameObject _player;                     //Jugador en la escena
    protected Rigidbody2D _rb;                        //Componente RigidBody2D del enemigo
    protected RigidbodyConstraints2D _constraints;    //Restricciones aplicadas inicialmente para el componente RigidBody2D
    protected Animator _anim;                         //Componente animator del enemigo
    protected Vector2 _playerPosition;                //Posición del jugador en la escena
    protected Vector2 _dir;                           //Vector que marca la dirección hacia la que se mueve el enemigo
    protected float _dirTimer = 0f;                   //Temporizador que cuenta el tiempo necesario para que el enemigo cambie su dirección de movimiento
    protected bool _onRange = false;                  //Bandera que marca si el enemigo está a la distancia necesaria del jugador para ejecutar su ataque

    protected enum State {
        Spawning,
        Chasing,
        Attacking,
        Resting
    }

    protected State _currentState;
    protected State _lastState;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    protected virtual void Start() {
        _currentState = State.Spawning;
        _lastState = State.Resting;
        _attack = GetComponent<IAttack>();
        _player = FindObjectOfType<Movement>().gameObject;
        _rb = GetComponent<Rigidbody2D>();
        _constraints = _rb.constraints;
        _anim = GetComponent<Animator>();
        _spriteRend = GetComponent<SpriteRenderer>();
    }

    protected virtual void Update() {
        SetState();
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == EnemyRangeLayer) _onRange = true;
    }

    protected virtual void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == EnemyRangeLayer) _onRange = false;
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

     protected void SetState() {
        if (_currentState != _lastState)
        {
            _rb.velocity = Vector2.zero;
            switch (_currentState)
            {
                case State.Chasing:
                    ChasingState();
                    break;

                case State.Attacking:
                    AttackingState();
                    break;

                case State.Resting:
                    RestingState();
                    break;

                case State.Spawning:
                    SpawningState();
                    break;
            }
        }
    }

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
    /// <param name="v"> Vector2 cuya dirección se quiere calcular </param>
    /// <returns> Vector2 con la dirección hacia la que mira el enemigo </returns>
    protected Vector2 GetDirection(Vector2 v) {
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
    protected void SetAnim(Vector2 v) {
        _spriteRend.flipX = false;

        string side = "_MvSide";
        string up = "_MvUp";
        string down = "_MvDown";

        if (v.x > 0)
        {
            _anim.SetBool(side, true);
        }
        else if (v.x < 0)
        {
            _anim.SetBool(side, true);
            _spriteRend.flipX = true;

        }
        else
        {
            _anim.SetBool(side, false);
        }

        if (v.y > 0)
        {
            _anim.SetBool(up, true);
            _anim.SetBool(down, false);
        }
        else if (v.y < 0)
        {
            _anim.SetBool(up, false);
            _anim.SetBool(down, true);
        }
        else
        {
            _anim.SetBool(up, false);
            _anim.SetBool(down, false);
        }
    }

    protected void SetDir() {
        _playerPosition = _player.transform.position;
        if (_dirTimer >= 0.25f)
        {
            _dir = GetDirection(_playerPosition - (Vector2)transform.position);
            _dirTimer = 0f;
        }
        else _dirTimer += Time.deltaTime;
    }

    //States

    protected virtual void SpawningState() {
        StartCoroutine(Spawning());
    }

    protected virtual void ChasingState() {
        _lastState = State.Spawning;
        if (_onRange) _currentState = State.Attacking;
        else
        {
            SetDir();
            _rb.velocity = _dir * MovementSpeed;
        }
    }

    protected virtual void AttackingState() {
        _playerPosition = _player.transform.position;
        _dir = GetDirection(_playerPosition - (Vector2)transform.position);
        _rb.velocity = _dir * 0f;
        StartCoroutine(Attacking());
    }

    protected virtual void RestingState() {
        StartCoroutine(Resting());
    }

    protected virtual IEnumerator Spawning() {
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        collider.enabled = false;

        yield return new WaitForSeconds(SpawnTime);

        collider.enabled = true;
        _currentState = State.Chasing;
    }

    protected virtual IEnumerator Attacking() {
        _lastState = State.Attacking;
        _anim.SetTrigger("_Attack");
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return _attack.Attack();

        _rb.constraints = _constraints;
        _currentState = State.Resting;
    }

    protected virtual IEnumerator Resting() {
        _lastState = State.Resting;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(RestTime);

        _rb.constraints = _constraints;
        _currentState = State.Chasing;
    }

    #endregion

} // class Enemy1 
// namespace
