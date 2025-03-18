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
public class Enemy2_Behaviour : Enemy_StateMachine
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    [SerializeField] float FleeingSpeed;
    [SerializeField] float FleeingTime;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    bool _baseState = true;     //Inidca si se tienen en cuenta los estados de la clase base
    float _fleeingTimer = 0f;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    protected override void FixedUpdate() {
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

    protected override void AttackingState() {
        StartCoroutine(Attacking());
    }

    protected override IEnumerator Attacking() {
        yield return base.Attacking();
        _baseState = false;
    }

    void SetInverseDir() {
        _playerPosition = _player.transform.position;
        if (_dirTimer >= 0.25f)
        {
            _dir = GetDirection((Vector2)transform.position - _playerPosition, _tolerance);
            _dirTimer = 0f;
        }
        else _dirTimer += Time.deltaTime;
    }

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
