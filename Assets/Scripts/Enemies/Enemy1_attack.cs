//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System;
using UnityEngine;
// Añadir aquí el resto de directivas using


/// <summary>
/// Antes de cada class, descripción de qué es y para qué sirve,
/// usando todas las líneas que sean necesarias.
/// </summary>
public class Enemy1_attack : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    GameObject _hitbox;
    GameObject _player;
    Enemy_movement _mov;
    Vector2 _dir;
    Action _disableHitboxAct;
    int _layer;
    bool _activeHitbox = false;

    #endregion
    
    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour
    
    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 
    
    /// <summary>
    /// Start is called on the frame when a script is enabled just before 
    /// any of the Update methods are called the first time.
    /// </summary>
    void Start()
    {
        _hitbox = transform.GetChild(0).gameObject;
        _player = FindObjectOfType<Movement>().gameObject;
        _mov = GetComponent<Enemy_movement>();
        _layer = _player.layer;
        _hitbox.SetActive(false);
        _disableHitboxAct = DisableHitbox;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (!_hitbox.activeSelf) 
        {
            _dir = _mov.GetDir();
            SetDir(_dir, 0.25f);
        }
    }
    #endregion

    private void OnTriggerEnter2D(Collider2D collision) {
        if(collision.gameObject.layer == _layer && _activeHitbox)
        {
            _player.GetComponent<Player_Health>();
        }
    }

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    /// <summary>
    /// 
    /// </summary>
    public void Attack(Vector2 dir) {
        _hitbox.SetActive(true);
        Invoke("DisableHitbox", 0.25f);
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    void SetDir(Vector2 v, float offset) {
        Vector2 res;
        _hitbox.transform.rotation = transform.rotation;

        if(v.y == 0)
        {
            if (v.x > 0) res = new Vector2(offset, 0);
            else if (v.x < 0) res = new Vector2(-offset, 0);
            else res = Vector2.zero;
        }
        else
        {
            if (v.y > 0) res = new Vector2(0, offset);
            else res = new Vector2(0, -offset);

            _hitbox.transform.Rotate(0, 0, 90, Space.World);

        }
        _hitbox.transform.localPosition = res;
    }

    void DisableHitbox() {
        _hitbox.SetActive(false);
    }

    #endregion   

} // class Enemy1_attack 
// namespace
