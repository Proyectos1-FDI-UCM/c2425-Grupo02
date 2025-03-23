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
/// Clase que hereda de Enemy_StateMachine y que se encarga de controlar el comportamiento del enemigo 2.
/// Tiene todos los estados de Enemy_StateMachine, así como uno nuevo (FleeingState) que no está puesto en un enumerado por simplicidad (ya que solo es un estado).
/// El control de qué estados se tienen en cuenta (nuevos o bases) se lleva mediante el booleano "_baseState". Si es true solo se tendrán en cuenta los estados 
/// heredados de Enemy_StateMachine, y si es false solo se tendrán en cuenta los nuevos (implementados en esta clase)
/// </summary>
public class Enemy2_Behaviour : Enemy_StateMachine
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Velocidad de movimiento del enemigo cuando está huyendo
    /// </summary>
    [SerializeField] float FleeingSpeed;
    /// <summary>
    /// Tiempo que dura la huida del enemigo
    /// </summary>
    [SerializeField] float FleeingTime;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Booleano que indica si se usan los estados heredados de Enemy_StateMachine (true)
    /// o los implementados en esta clase (false)
    /// </summary>
    bool _baseState = true;
    /// <summary>
    /// Variable que guarda el tiempo que ha estado huyendo el enemigo (se resetea cuando llega al máximo)
    /// </summary>
    float _fleeingTimer = 0f;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Comprueba en cada frame si se usan los estados base o no, y en base a eso asigna el estado correspondiente
    /// </summary>
    protected override void Update() {
        if (_baseState) SetState();
        else
        {
            FleeState();
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

   

    #endregion
    
    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Comienza la corrutina de ataque (sin necesidad de mover la hitbox como en la clase de la que hereda)
    /// </summary>
    protected override void AttackingState() {
        StartCoroutine(Attacking());
    }

    /// <summary>
    /// Realiza el ataque y cuando termina, cambia a los estados nuevos (implementados en esta clase)
    /// </summary>
    /// <returns> Espera a que se acabe la corrutina de ataque del script de ataque </returns>
    protected override IEnumerator Attacking() {
        yield return base.Attacking();
        _baseState = false;
    }

    /// <summary>
    /// Método que calcula y asigna a "_dir" la dirección en la que se tiene que mover el enemigo para huir del jugador.
    /// Lo hace con una restricción de tiempo
    /// </summary>
    void SetInverseDir() {
        if (_dirTimer >= 0.25f)
        {
            _playerPosition = _player.transform.position;
            _dir = GetDirection((Vector2)transform.position - _playerPosition, _tolerancy);
            _dirTimer = 0f;
        }
        else _dirTimer += Time.deltaTime;
    }

    /// <summary>
    /// Huye del jugador durante un tiempo especificado ("FleeingTime") y después cambia a los estados base
    /// </summary>
    void FleeState() {
        if (_fleeingTimer >= FleeingTime)
        {
            _fleeingTimer = 0f;
            _baseState = true;
        }
        else
        {
            SetInverseDir();
            _rb.velocity = _dir * FleeingSpeed;
            _fleeingTimer += Time.deltaTime;
        }
    }

    #endregion   

} // class Enemy2_Behaviour 
// namespace
