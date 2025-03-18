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

    /// <summary>
    /// Tiempo que dura el estado "Spawning" del enemigo
    /// </summary>
    [SerializeField] protected float SpawnTime;            
    /// <summary>
    /// Velocidad de movimiento del enemigo
    /// </summary>
    [SerializeField] protected float MovementSpeed;
    /// <summary>
    /// Tiempo que dura el estado "Resting" del enemigo
    /// </summary>
    [SerializeField] protected float RestTime;
    /// <summary>
    /// Índice de la capa de colisión que marca el rango de ataque del enemigo
    /// </summary>
    [SerializeField] protected int EnemyRangeLayer;           

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (protected fields)
    /// <summary>
    ///Interfaz de script de ataque del enemigo
    /// </summary>
    protected IAttack _attack;
    /// <summary>
    /// Componente sprite renderer del enemigo
    /// </summary>
    protected SpriteRenderer _spriteRend;   
    /// <summary>
    /// Jugador en la escena
    /// </summary>
    protected GameObject _player;   
    /// <summary>
    /// Componente rigidbody del enemigo
    /// </summary>
    protected Rigidbody2D _rb;                        
    /// <summary>
    /// Restricciones aplicadas inicialmente para el componente rigidbody del enemigo
    /// </summary>
    protected RigidbodyConstraints2D _constraints;
    /// <summary>
    /// Componente animator del enemigo
    /// </summary>
    protected Animator _anim; 
    /// <summary>
    /// Posición del jugador
    /// </summary>
    protected Vector2 _playerPosition;
    /// <summary>
    /// Dirección en la que se mueve el enemigo
    /// </summary>
    protected Vector2 _dir;
    /// <summary>
    /// Tolerancia para salir de las direcciones no diagonales en el metodo "SetDir"
    /// </summary>
    protected int _tolerance;
    /// <summary>
    /// Indica el tiempo máximo que tiene que pasar para que el enemigo cambie su dirección
    /// </summary>
    protected float _dirTime;
    /// <summary>
    /// Temporizador que cuenta el tiempo necesario para que el enemigo cambie su dirección de movimiento.
    /// Se aplica en la función "SetDir"
    /// </summary>
    protected float _dirTimer = 0f;
    /// <summary>
    /// Bandera que marca si el enemigo está a la distancia necesaria del jugador para ejecutar su ataque
    /// </summary>
    protected bool _onRange = false;

    /// <summary>
    /// Enumerado con cad uno de los estados en los que puede estar el enemigo
    /// </summary>
    protected enum State {
        Spawning,
        Chasing,
        Attacking,
        Resting
    }

    /// <summary>
    /// Estado actual del enemigo
    /// </summary>
    protected State _currentState;
    /// <summary>
    /// Último estado en el que estuvo el enemigo antes de estar en el actual
    /// </summary>
    protected State _lastState;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se accede al jugador en escena y se accede a los componentes necesarios del enemigo.
    /// Se entra en el estado "Spawning" y se define "Resting" como _lastState.
    /// Se definen _tolerance y _dirTime
    /// </summary>
    protected virtual void Start() {
        _currentState = State.Spawning;
        _lastState = State.Resting;
        _attack = GetComponent<IAttack>();
        _player = FindObjectOfType<Movement>().gameObject;
        _rb = GetComponent<Rigidbody2D>();
        _constraints = _rb.constraints;
        _anim = GetComponent<Animator>();
        _spriteRend = GetComponent<SpriteRenderer>();
        _tolerance = 15;
        _dirTime = 0.25f;
    }

    /// <summary>
    /// En cada frame (fijos) se ejecuta SetState()
    /// </summary>
    protected virtual void FixedUpdate() {
        SetState();
    }

    /// <summary>
    /// Si el trigger de rango del enemigo entra en contacto con un trigger que se encuentre en la capa que 
    /// determina el rango de los enemigos, se marca _on range como true
    /// </summary>
    /// <param name="collision"> collider del objeto con el que se activó el triggger </param>
    protected virtual void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.layer == EnemyRangeLayer) _onRange = true;
    }

    /// <summary>
    /// Si el trigger de rango del enemigo deja de estar en contacto con un trigger que se encuentre en la capa que 
    /// determina el rango de los enemigos, se marca _on range como false
    /// </summary>
    /// <param name="collision">  collider del objeto con el que se activó el triggger </param>
    protected virtual void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.layer == EnemyRangeLayer) _onRange = false;
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Se comprueba que el último estado es distinto al actual, y si esto se cumple, se entra en el estado correspondiente
    /// </summary>
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

    /// <summary>
    /// Método público para obtener la dirección en la que se mueve el enemigo
    /// </summary>
    /// <returns></returns>
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
    /// <param name="t"> Tolerancia para salir de las direcciones no diagonales </param>
    /// <returns> Vector2 con la dirección hacia la que mira el enemigo </returns>
    protected Vector2 GetDirection(Vector2 v, int t) {
        Vector2 res;
        float ang = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
        ang = (ang + 360) % 360;

        //Derecha
        if (ang <= 0 + t && ang >= 0 || ang <= 360 && ang >= 360 - t) res = Vector2.right;
        //Arriba
        else if (ang <= 90 + t && ang >= 90 - t) res = Vector2.up;
        //Izquierda
        else if (ang <= 180 + t && ang >= 180 - t) res = Vector2.left;
        //Abajo
        else if (ang <= 270 + t && ang >= 270 - t) res = Vector2.down;
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
    protected virtual void SetAnim(Vector2 v) {
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


    /// <summary>
    /// Cambia la dirección del enemigo con una restricción de tiempo
    /// </summary>
    /// <param name="t"> Tiempo necesario para que cambie la dirección de movimiento del enemigo </param>
    protected void SetDir(float t) {
        if (_dirTimer >= t)
        {
            _playerPosition = _player.transform.position;
            _dir = GetDirection(_playerPosition - (Vector2)transform.position, _tolerance);
            _dirTimer = 0f;
        }
        else _dirTimer += Time.deltaTime;
    }

    //States

    /// <summary>
    /// Inicia la corrutina "Spawning"
    /// </summary>
    protected virtual void SpawningState() {
        StartCoroutine(Spawning());
    }

    /// <summary>
    /// Corrutina que desactiva el collider del enemigo durante su duración.
    /// Cuando acaba, establece el estado "Chasing" como estado actual
    /// </summary>
    /// <returns> La duración viene dada por el atributo serializado "SpawnTime" </returns>
    protected virtual IEnumerator Spawning() {
        Collider2D collider = gameObject.GetComponent<Collider2D>();
        collider.enabled = false;

        yield return new WaitForSeconds(SpawnTime);

        collider.enabled = true;
        _currentState = State.Chasing;
    }

    /// <summary>
    /// Establece "Spawning" como el último estado (para poder ser llamada varias veces en el update).
    /// Si el jugador está a rango, entra en el estado "Attacking", si no lo persigue
    /// </summary>
    protected virtual void ChasingState() {
        _lastState = State.Spawning;
        if (_onRange) _currentState = State.Attacking;
        else
        {
            SetDir(_dirTime);
            _rb.velocity = _dir * MovementSpeed;
        }
    }

    /// <summary>
    /// Busca la dirección del jugador, y comienza la corrutina "Attacking"
    /// </summary>
    protected virtual void AttackingState() {
        _playerPosition = _player.transform.position;
        _dir = GetDirection(_playerPosition - (Vector2)transform.position, _tolerance);
        _rb.velocity = _dir * 0f;
        StartCoroutine(Attacking());
    }

    /// <summary>
    /// Establece "Attacking" como el último estado, activa la animación de ataque 
    /// y congela la posición del enemigo durante su duración.
    /// Establece "Resting" como el estado actual
    /// </summary>
    /// <returns> Espera a que la corrutina de ataque del script de ataque del enemigo acabe </returns>
    protected virtual IEnumerator Attacking() {
        _lastState = State.Attacking;
        _anim.SetTrigger("_Attack");
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return _attack.Attack();

        _currentState = State.Resting;
    }

    /// <summary>
    /// Inicia la corrutina "Resting"
    /// </summary>
    protected virtual void RestingState() {
        StartCoroutine(Resting());
    }

    /// <summary>
    /// Establece "Resting" como el último estado.
    /// Al acabar, aplica al rigidbody sus restricciones iniciales y establece "Chasing" como el estado actual
    /// </summary>
    /// <returns> La duración de la corrutina viene dada por el atributo serializado "RestTime" </returns>
    protected virtual IEnumerator Resting() {
        _lastState = State.Resting;

        yield return new WaitForSeconds(RestTime);

        _rb.constraints = _constraints;
        _currentState = State.Chasing;
    }

    #endregion

} // class Enemy1 
// namespace
