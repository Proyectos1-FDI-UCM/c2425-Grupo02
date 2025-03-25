//---------------------------------------------------------
// Breve descripción del contenido del archivo
// Responsable de la creación de este archivo
// Nombre del juego
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------

using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;


/// <summary>
/// Clase que se encarga de definir un a zona en la que spawnean enemigos aleatoriamente.
/// Desde el editor se define una zona mediante un sprite, y se indican un tipo de enemigo, el número de enemigos
/// que se spawnean por iteración y el número de iteraciones.
/// Cada vez que se derrotan a todos los enemigos de una iteración salen a la vez todos los de la siguiente iteración.
/// 
/// La clase se basa en un diccionario "_cellDict" en el que las claves son indices y los valores son Vector2 que corresponden a 
/// celdas de un objeto de tipo CustomGrid. Cuando se llama al método que instancia a los enemigos, se crea un array con todos los índices
/// del diccionario (las claves) pero ordenados de forma aleatoria. Se accede a los valores del diccionario a través de este array
/// </summary>
public class Enemy_Spawn : MonoBehaviour {

    // ---- ATRIBUTOS DEL INSPECTOR ----
    #region Atributos del Inspector (serialized fields)

    /// <summary>
    /// Enemigo que se va a instanciar
    /// </summary>
    [SerializeField] GameObject Enemy;
    /// <summary>
    /// Número de enemigos que se van a spawnear
    /// </summary>
    [SerializeField] int EnemyNumber;
    /// <summary>
    /// Número de iteraciones del spawn
    /// </summary>
    [SerializeField] int Iterations;
    /// <summary>
    /// Si es true, los enemigos solo se instanciarán si el jugador entra en la zona de spawn, y además, este no podrá
    /// salir de ahí hasta que se eliminen a los enemigos de todas las iteraciones.
    /// </summary>
    [SerializeField] bool LimitZone;
    /// <summary>
    /// Lista con las celdas en las que no pueden spawnear enemigos
    /// </summary>
    [SerializeField] List<Vector2Int> BannedCells = new();

    #endregion

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)
    /// <summary>
    /// Collider de la zona de spawn, para detectar cuándo entra el jugador
    /// </summary>
    BoxCollider2D _collider;
    /// <summary>
    /// Sprite de la zona del spawn
    /// </summary>
    SpriteRenderer _sprite;
    /// <summary>
    /// Cuadrícula que delimita la zona de spawn
    /// </summary>
    CustomGrid _grid;
    /// <summary>
    /// Diccionario que asigna a un índice a cada celda en la que puede spawnear un enemigo
    /// </summary>
    Dictionary<int, Vector2> _cellDict;
    /// <summary>
    /// Lista de celdas en las que no puede spawnear un enemigo
    /// </summary>
    HashSet<Vector2Int> _bannedCells = new();
    /// <summary>
    /// Ancho de la cuadrícula
    /// </summary>
    int _gridWidth; 
    /// <summary>
    /// Alto de la cuadrícula
    /// </summary>
    int _gridLength;   
    /// <summary>
    /// Iteración actual del spawn (empieza siempre en 0)
    /// </summary>
    int _currentIteration = 0; 
    /// <summary>
    /// Enemigos vivos spawneados por el spawn
    /// </summary>
    int _currentEnemies;
    
    #endregion

    // ---- MÉTODOS DE MONOBEHAVIOUR ----
    #region Métodos de MonoBehaviour

    /// <summary>
    /// Se obtienen los componentes del objeto necesarios y se limita el número de enemigos a spawnear para que no se rompa.
    /// También se ajusta el tamaño de la CustomGrid y el collider al tamaño del sprite, y se calcula el diccionario de celdas disponibles
    /// </summary>
    void Start() {
        _collider = GetComponent<BoxCollider2D>();
        _sprite = GetComponent<SpriteRenderer>();
        _gridWidth = Mathf.FloorToInt(_sprite.size.x);
        _gridLength = Mathf.FloorToInt(_sprite.size.y);
        _sprite.forceRenderingOff = true;

        _grid = new CustomGrid(_gridWidth, _gridLength, 1, gameObject);
        if (EnemyNumber > _gridWidth * _gridLength) EnemyNumber = _gridWidth * _gridLength;
        if (EnemyNumber == _gridWidth * _gridLength) EnemyNumber -= BannedCells.Count;
        if (EnemyNumber < 0) EnemyNumber = 0;
        _bannedCells = BannedCells.ToHashSet();
        SetDict();

        StartSpawn();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="collision"> Colisión detectada por el trigger </param>
    void OnTriggerEnter2D(Collider2D collision) {
        SpawnEnemies();
        InstantiateLimitZone();
        _collider.enabled = false;
    }

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    /// <summary>
    /// Resta un enemigo al número de enemigos actuales. Si se ha completado la última iteración, se desactiva el spawn
    /// </summary>
    public void SubstractEnemy() {
        _currentEnemies--;
        if (_currentEnemies <= 0)
        {
            if (_currentIteration < Iterations)
            {
                SpawnEnemies();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Método que se encarga de instanciar enemigos por primera vez si "LimitZone" es false, y también desactiva el collider del spawn.
    /// En caso contrario, ajusta el tamaño del collider del spawn al tamaño de la CustomGrid
    /// </summary>
    void StartSpawn() {
        if (!LimitZone)
        {
            SpawnEnemies();
            _collider.enabled = false;
        }
        else
        {
            Vector2 newSize = new(_grid.GetWidth(), _grid.GetHeight());
            _collider.size = newSize;
            _collider.transform.Translate(newSize / 2);
        }
    }

    void InstantiateLimitZone() {
        Vector2 origin = gameObject.transform.position;

        Vector2[] arr = new Vector2[]
        {
            //izquierda
            new(origin.x, origin.y + _grid.GetHeight() / 2),
            //derecha
            new(origin.x + _grid.GetWidth(), origin.y + _grid.GetHeight() / 2),
            //arriba
            new(origin.x + _grid.GetHeight() / 2, origin.y + _grid.GetHeight()),
            //abajo
            new(origin.x, origin.y + _grid.GetHeight())
        };

        for (int i = 0; i < 4; i++)
        {
            BoxCollider2D col = new();
            if (i < 2)
            {
                col.size = new Vector2();
            }
            else
            {
                //col.size =
            }
            Instantiate(col, arr[i], Quaternion.identity);
        }
    }

    /// <summary>
    /// Método que se encarga de instanciar enemigos y avanzar de iteración del spawn.
    /// El método itera sobre un array de enteros que son las claves del diccionario de celdas, pero están desordenadas
    /// de forma aleatoria
    /// </summary>
    void SpawnEnemies() {
        int[] cells = GetShuffledKeys();
        int it = 0;
        for (int i = 0; i < EnemyNumber; i++)
        {
            Vector2 pos = _cellDict[cells[it]];
            GameObject enemy = Instantiate(Enemy, pos, Quaternion.identity);
            enemy.GetComponent<Enemy_Health>().SetSpawn(gameObject.GetComponent<Enemy_Spawn>());
            it++;
        }
        _currentEnemies = EnemyNumber;
        _currentIteration++;
    }

    /// <summary>
    /// Método que se encarga de obtener un diccionario que asigna un índice a cada celda que no se encuentre en _bannedCells
    /// </summary>
    void SetDict() {
        _cellDict = new Dictionary<int, Vector2>();

        int it = 0;
        for (int i = 0; i < _gridWidth; i++)
        {
            for (int j = 0;  j < _gridLength; j++)
            {
                if (!_bannedCells.Contains(new Vector2Int(i, j)))
                {
                    _cellDict[it] = _grid.GetCells()[i, j];
                    it++;
                }
            }
        }
    }

    /// <summary>
    /// Método que crea un array de enteros con las claves del diccionario "_cellDict" pero ordenadas de forma aleatoria
    /// </summary>
    /// <returns> Array con las claves de _cellDict desordenadas </returns>
    int[] GetShuffledKeys() {
        int[] res = new int[_cellDict.Count];

        int it = 0;
        foreach (int key in _cellDict.Keys)
        {
            res[it] = key;
            it++;
        }

        Shuffle(ref res);
        return res;
    }

    /// <summary>
    /// Método que se encarga de ordenar de forma aleatoria los elementos de un array.
    /// Se recorre el array al revés y se va intercambiando el elemento de la última posición (no intercambiada aún) 
    /// con el de otra posición aleatoria
    /// </summary>
    /// <param name="arr"> Array que se va a desordenar </param>
    void Shuffle(ref int[] arr) {
        
        for (int i = arr.Length - 1; i >= 0; i--)
        {
            int index = Random.Range(0, i + 1);
            int elem = arr[index];
            arr[index] = arr[i];
            arr[i] = elem;
        }
    }

    #endregion   

} // class SpawnScript 
// namespace
