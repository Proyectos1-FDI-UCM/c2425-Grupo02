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
public class IramisBulletSpawn : MonoBehaviour
{
    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // públicos y de inspector se nombren en formato PascalCase
    // (palabras con primera letra mayúscula, incluida la primera letra)
    // Ejemplo: MaxHealthPoints

    /// <summary>
    /// Tiempo transcurrido entre disparos
    /// </summary>
    [SerializeField]
    private float _shootCooldown;

    /// <summary>
    /// Velocidad de la bala
    /// </summary>
    [SerializeField]
    private float _bulletSpeed;

    /// <summary>
    /// GameObject de la bala
    /// </summary>
    [SerializeField]
    private int _bulletNumber;

    /// <summary>
    /// GameObject de la bala
    /// </summary>
    [SerializeField]
    private GameObject _bullet;

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    // Documentar cada atributo que aparece aquí.
    // El convenio de nombres de Unity recomienda que los atributos
    // privados se nombren en formato _camelCase (comienza con _, 
    // primera palabra en minúsculas y el resto con la 
    // primera letra en mayúsculas)
    // Ejemplo: _maxHealthPoints

    private float _shootCooldownTimer = 0f;

    private IramisBullet _bulletMovement;

    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    // Por defecto están los típicos (Update y Start) pero:
    // - Hay que añadir todos los que sean necesarios
    // - Hay que borrar los que no se usen 

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        _shootCooldownTimer -= Time.deltaTime;

        if(_shootCooldownTimer <= 0f)
        {
            RadialShot(transform.position, transform.up * _bulletSpeed);
            _shootCooldownTimer = _shootCooldown;
        }
    }
    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    // Documentar cada método que aparece aquí con ///<summary>
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)
    // Ejemplo: GetPlayerController



    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados
    // Documentar cada método que aparece aquí
    // El convenio de nombres de Unity recomienda que estos métodos
    // se nombren en formato PascalCase (palabras con primera letra
    // mayúscula, incluida la primera letra)

    /// <summary>
    /// Instancia una bala con el origen y la velocidad indicados
    /// </summary>
    private void Shoot(Vector2 origin, Vector2 speed)
    {
        Instantiate(_bullet, origin, Quaternion.identity);
        _bulletMovement = _bullet.GetComponent<IramisBullet>();

        _bullet.transform.position = origin;
        _bulletMovement.SetSpeed(speed.x, speed.y);
    }

    /// <summary>
    /// Instancia el número de balas indicado alrededor del spawner
    /// </summary>
    private void RadialShot(Vector2 origin, Vector2 aimDirection)
    {
        float _angleBetweenBullets = 360f / _bulletNumber;

        for (int i = 0; i < _bulletNumber; i++)
        {
            float _bulletDirectionAngle = _angleBetweenBullets * i;

            Vector2 _bulletDirection = Rotate(aimDirection, _bulletDirectionAngle);
            Shoot(transform.position, _bulletDirection * _bulletSpeed);
        }
    }

    /// <summary>
    /// Rota la velocidad de la bala la cantidad de grados indicada
    /// </summary>
    private Vector2 Rotate(Vector2 _originalVector, float _rotateAngleInDegrees)
    {
        Quaternion rotation = Quaternion.AngleAxis(_rotateAngleInDegrees, Vector3.forward);
        return rotation * _originalVector;
    }

    #endregion   

} // class BulletSpawn 
// namespace
