//---------------------------------------------------------
// Componente de disparo del jugador
// Lucía Mei Domínguez López
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using UnityEngine;

/// <summary>
/// Clase que usa el player para instanciar una bala
/// </summary>
public class Shoot : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----

    #region Atributos del Inspector (serialized fields)
    /// <summary>
    /// prefab de la bala
    /// </summary>
    [SerializeField] private GameObject bullet;
    /// <summary>
    /// modificador posición de instancia de la bala
    /// </summary>
    [SerializeField] private float posMod;
    /// <summary>
    /// Sonido del proyectil
    /// </summary>
    [SerializeField] private AudioClip proyectileSFX;
    /// <summary>
    /// Delay del disparo
    /// </summary>
    [SerializeField] private float DelayTime;


    #endregion

    // ---- ATRIBUTOS PRIVADOS ----

    #region Atributos Privados (private fields)

    private GameObject _newBullet;
    private Movement _playerMovement;
    private bool _noBulletInGame = true;
    private float _timer = 0;
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----

    #region Métodos de MonoBehaviour
    void Awake()
    {
        _playerMovement = GetComponent<Movement>();
    }
    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// 
    /// Dispara si no hay otra bala instanciada
    /// </summary>
    void Update()
    {
        _timer += Time.deltaTime;
        Debug.Log("Timer: " +  _timer);
        if (InputManager.Instance.FireWasPressedThisFrame() && _timer >= DelayTime)
        {
            if (_noBulletInGame)
            {
                ShootNewBullet();
                _timer = 0;
            }
        }
        if (_newBullet == null)
        {
            _noBulletInGame = true;
        }
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----

    #region Métodos públicos

    #endregion

    // ---- MÉTODOS PRIVADOS ----

    #region Métodos Privados
    /// <summary>
    /// Se posiciona y gira la bala según la dirección en la que mira el player
    /// instancePos -> posición en la que se instancia la bala
    /// Orden en el que se comprueban las posiciones de la bala -> derecha, izquierda, arriba, abajo
    /// </summary>
    private void ShootNewBullet()
    {
        Vector2 lastDir = _playerMovement.GetLastDir();
        Quaternion bulletRotation;
        Vector3 instancePos = transform.position;

        if (lastDir.x > 0)
        {
            instancePos.x += posMod; 
            bulletRotation = Quaternion.Euler(0, 0, 90);
        }
        else if (lastDir.x < 0)
        {
            instancePos.x -= posMod; 
            bulletRotation = Quaternion.Euler(0, 0, -90);
        }
        else if (lastDir.y > 0)
        {
            instancePos.y += posMod;
            bulletRotation = Quaternion.Euler(0, 0, 180);
        }
        else
        {
            instancePos.y -= posMod;
            bulletRotation = Quaternion.Euler(0, 0, 0);
        }

        _newBullet = Instantiate(bullet, instancePos, bulletRotation);
        _noBulletInGame = false;
        AudioManager.Instance.PlayAudio(proyectileSFX, 0.5f);
    }

    #endregion
} // class TestFire 
// namespace
