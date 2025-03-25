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
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class IramisPhase2_StateMachine : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Velocidad de movimiento del enemigo
    /// </summary>
    [SerializeField] protected float MovementSpeed;
    /// <summary>
    /// Tiempo que transcurre para que el jefe intente disparar en el estado "Chasing"
    /// </summary>
    [SerializeField] protected float ShootingThreshold;
    /// <summary>
    /// Tiempo que dura el estado "Shooting"
    /// </summary>
    [SerializeField] protected float ShootingTimeLimit;
    /// <summary>
    /// Tiempo que dura el estado "Resting" del enemigo
    /// </summary>
    [SerializeField] protected float RestTime;
    /// <summary>
    /// Índice de la capa de colisión que marca el rango de ataque del enemigo
    /// </summary>
    [SerializeField] protected int EnemyRangeLayer;
    /// <summary>
    /// GameObject del spawner de balas para el estado "Shooting"
    /// </summary>
    [SerializeField] protected GameObject ShotSpawner;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    /// <summary>
    ///Interfaz de script de ataque del jefe
    /// </summary>
    protected IAttack _attack;
    /// <summary>
    /// Componente sprite renderer del jefe
    /// </summary>
    protected SpriteRenderer _spriteRend;
    /// <summary>
    /// Jugador en la escena
    /// </summary>
    protected GameObject _player;
    /// <summary>
    /// Componente rigidbody del jefe
    /// </summary>
    protected Rigidbody2D _rb;
    /// <summary>
    /// Restricciones aplicadas inicialmente para el componente rigidbody del jefe
    /// </summary>
    protected RigidbodyConstraints2D _constraints;
    /// <summary>
    /// Componente animator del jefe
    /// </summary>
    protected Animator _anim;
    /// <summary>
    /// Posición del jugador
    /// </summary>
    protected Vector2 _playerPosition;
    /// <summary>
    /// Dirección en la que se mueve el jefe
    /// </summary>
    protected Vector2 _dir;
    /// <summary>
    /// Tolerancia para salir de las direcciones no diagonales en el metodo "SetDir"
    /// </summary>
    protected int _tolerance = 15;
    /// <summary>
    /// Indica el tiempo máximo que tiene que pasar para que el enemigo cambie su dirección
    /// </summary>
    protected float _dirTime = 0.25f;
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
    /// Valor que indica si el jefe activa el estado de disparo durante el estado "Chasing"
    /// </summary>
    protected int _shoot;
    /// <summary>
    /// Tiempo inicial de enfriamiento del estado "Shooting"
    /// </summary>
    protected float _shootThresholdTime = 0f;
    /// <summary>
    /// Tiempo inicial transcurrido del estado "Shooting"
    /// </summary>
    protected float _shootTime = 0f;

    /// <summary>
    /// Enumerado con cad uno de los estados en los que puede estar el enemigo
    /// </summary>
    protected enum State
    {
        Chasing,
        Attacking,
        Resting,
        Shooting
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

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Se accede al jugador en escena y se accede a los componentes necesarios del enemigo.
    /// Se entra en el estado "Spawning" y se define "Resting" como _lastState
    /// </summary>
    protected virtual void Start()
    {
        _currentState = State.Resting;
        _attack = GetComponent<IAttack>();
        _player = FindObjectOfType<Movement>().gameObject;
        _rb = GetComponent<Rigidbody2D>();
        _constraints = _rb.constraints;
        _anim = GetComponent<Animator>();
        _spriteRend = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// En cada frame (fijos) se ejecuta SetState()
    /// </summary>
    protected virtual void FixedUpdate()
    {
        SetState();
    }

    /// <summary>
    /// Si el trigger de rango del enemigo entra en contacto con un trigger que se encuentre en la capa que 
    /// determina el rango de los enemigos, se marca _on range como true
    /// </summary>
    /// <param name="collision"> collider del objeto con el que se activó el triggger </param>
    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == EnemyRangeLayer) _onRange = true;
    }

    /// <summary>
    /// Si el trigger de rango del enemigo deja de estar en contacto con un trigger que se encuentre en la capa que 
    /// determina el rango de los enemigos, se marca _on range como false
    /// </summary>
    /// <param name="collision">  collider del objeto con el que se activó el triggger </param>
    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == EnemyRangeLayer) _onRange = false;
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController

    /// <summary>
    /// Se comprueba que el último estado es distinto al actual, y si esto se cumple, se entra en el estado correspondiente
    /// </summary>
    protected void SetState()
    {
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

                case State.Shooting:
                    ShootingState();
                    break;

            }
        }
    }

    /// <summary>
    /// Método público para obtener la dirección en la que se mueve el enemigo
    /// </summary>
    /// <returns></returns>
    public Vector2 GetDir()
    {
        return _dir;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)


    /// <summary>
    /// Esta función toma un vector y calcula su dirección (8 direcciones posibles).
    /// Tiene un poco de margen en las direcciones no diagonales.
    /// También llama al método privado SetAnim.
    /// </summary>
    /// <param name="v"> Vector2 cuya dirección se quiere calcular </param>
    /// <param name="t"> Tolerancia para salir de las direcciones no diagonales </param>
    /// <returns> Vector2 con la dirección hacia la que mira el enemigo </returns>
    protected Vector2 GetDirection(Vector2 v, int t)
    {
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
    protected virtual void SetAnim(Vector2 v)
    {
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
    protected void SetDir(float t)
    {
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
    /// Establece "Spawning" como el último estado (para poder ser llamada varias veces en el update).
    /// Si el jugador está a rango, entra en el estado "Attacking", si no lo persigue
    /// </summary>
    protected virtual void ChasingState()
    {
        _lastState = State.Resting;
        if (_onRange) _currentState = State.Attacking;

        else if (_shootThresholdTime >= ShootingThreshold)
        {
            _shootThresholdTime = 0f;
            _shoot = Random.Range(1, 4);

            if (_shoot == 1)
            {
                _currentState = State.Shooting;
                Instantiate(ShotSpawner, transform.position, Quaternion.identity);
            }
        }

        else
        {
            SetDir(_dirTime);
            _rb.velocity = _dir * MovementSpeed;
            _shootThresholdTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// Busca la dirección del jugador, y comienza la corrutina "Attacking"
    /// </summary>
    protected virtual void AttackingState()
    {
        _playerPosition = _player.transform.position;
        _dir = GetDirection(_playerPosition - (Vector2)transform.position, _tolerance);
        _rb.velocity = _dir * 0f;
        StartCoroutine(Attacking());
    }

    /// <summary>
    /// Establece "Attacking" como el último estado, activa la animación de ataque 
    /// y congela la posición del enemigo durante su duración.
    /// Establece "Resting" como el estado actual y devuelve las restricciones iniciales al rigidbody
    /// </summary>
    /// <returns> Espera a que la corrutina de ataque del script de ataque del enemigo acabe </returns>
    protected virtual IEnumerator Attacking()
    {
        _lastState = State.Attacking;
        _anim.SetTrigger("_Attack");
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return _attack.Attack();

        _rb.constraints = _constraints;
        _currentState = State.Resting;
    }

    /// <summary>
    /// Establece "Shooting" como el último estado, comienza la invocación de balas
    /// y reduce la velocidad del jefe
    /// </summary>
    protected virtual void ShootingState()
    {
        _lastState = State.Shooting;

        SetDir(_dirTime);
        _rb.velocity = _dir * (MovementSpeed * 0.25f);

        if (_shootTime >= ShootingTimeLimit)
        {
            _shootTime = 0f;
            _currentState = State.Chasing;
        }

        else _shootTime += Time.deltaTime;
    }

    /// <summary>
    /// Inicia la corrutina "Resting"
    /// </summary>
    protected virtual void RestingState()
    {
        StartCoroutine(Resting());
    }

    /// <summary>
    /// Establece "Resting" como el último estado y congela la posición del rigidbody.
    /// Al acabar, aplica al rigidbody sus restricciones iniciales y establece "Chasing" como el estado actual
    /// </summary>
    /// <returns> La duración de la corrutina viene dada por el atributo serializado "RestTime" </returns>
    protected virtual IEnumerator Resting()
    {
        _lastState = State.Resting;
        _rb.constraints = RigidbodyConstraints2D.FreezeAll;

        yield return new WaitForSeconds(RestTime);

        _rb.constraints = _constraints;
        _currentState = State.Chasing;
    }

    #endregion

} // class IramisPhase2_Behaviour 
// namespace
