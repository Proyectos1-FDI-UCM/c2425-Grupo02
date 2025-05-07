//---------------------------------------------------------
// Script que define la clase CustomGrid, que genera una cuadrícula a la que se le pueden añadir los métodos necesarios para el juego
// Sergio López Gómez
// Astra Damnatorum
// Proyectos 1 - Curso 2024-25
//---------------------------------------------------------


using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UIElements;
// Añadir aquí el resto de directivas using


/// <summary>
/// Clase que define una cuadrícula conformada por celdas.
/// Las celdas se guardan en un array bidimensional en el que los índices coinciden con la posición relativa
/// de cada celda en la cuadrícula.
/// Cada celda contiene un vector con las coordenadas (globales) de su centro
/// </summary>
public class CustomGrid {

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    /// <summary>
    /// Origen del objeto que cotiene el grid
    /// </summary>
    Vector2 _origin;         
    /// <summary>
    /// Anchira de la cudrícula
    /// </summary>
    int _width;          
    /// <summary>
    /// Altura de la cuadrícula
    /// </summary>
    int _length;
    /// <summary>
    /// Tamaño de cada celda
    /// </summary>
    int _cellSize;
    /// <summary>
    /// Array que contiene las celdas
    /// </summary>
    Vector2[,] _cells;

    #endregion

    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos

    //Constructor

    /// <summary>
    /// Constructor no asociado a un GameObject.
    /// LLama al método Initialize para inicializar los atributos comunes a ambos constructores
    /// </summary>
    /// <param name="width"> Ancho de la cuadrícula </param>
    /// <param name="length"> Alto de la cuadrícula </param>
    /// <param name="cellSize"> Tamaño de cada celda </param>
    public CustomGrid(int width, int length, int cellSize) {
        Initialize(width, length, cellSize);
    }

    /// <summary>
    /// Constructor asociado a un GameObject.
    /// LLama al método Initialize para inicializar los atributos comunes a ambos constructores
    /// </summary>
    /// <param name="width"> Ancho de la cuadrícula </param>
    /// <param name="length"> Alto de la cuadrícula </param>
    /// <param name="cellSize"> Tamaño de cada celda </param>
    /// <param name="gameObject"> GameObject que contendrá la cuadrícula </param>
    public CustomGrid(int width, int length, int cellSize, GameObject gameObject) {
        Vector2 org = gameObject.transform.position;
        _origin = new(org.x, org.y);
        Initialize(width, length, cellSize);
    }

    //Getters

    /// <summary>
    /// Getter del punto de origen
    /// </summary>
    /// <returns> El punto de origen de la cuadrícula </returns>
    public Vector2 GetOrigin() {
        return _origin;
    }

    /// <summary>
    /// Getter del ancho de la cuadrícula
    /// </summary>
    /// <returns> Ancho de la cuadrícula </returns>
    public int GetWidth() {
        return _width;
    }

    /// <summary>
    /// Getter del alto de la cuadrícula
    /// </summary>
    /// <returns> Alto de la cuadrícula </returns>
    public int GetHeight() {
        return _length;
    }

    /// <summary>
    /// Getter del tamaño de celda
    /// </summary>
    /// <returns> Tamaño de cada celda de la cuadrícula </returns>
    public int GetCellSize() {
        return _cellSize;
    }

    /// <summary>
    /// Getter del array que contiene las celdas 
    /// </summary>
    /// <returns> Array de celdas con la posición del centro de cada celda </returns>
    public Vector2[,] GetCells() {
        return _cells;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    /// <summary>
    /// Se encarga de asociar los valores especificados en el constructor a los atributos básicos de la clase.
    /// Si el atributo _origin es nulo, atribuye a cada celda coordenadas globales, 
    /// en caso contrario serán coordenadas locales con respecto a un gameObject
    /// </summary>
    /// <param name="width"> Ancho de la cuadrícula </param>
    /// <param name="length"> Alto de la cuadrícula </param>
    /// <param name="cellSize"> Tamaño de cada celda </param>
    void Initialize(int width, int length, int cellSize) {
        _width = width;
        _length = length;
        _cellSize = cellSize;
        _cells = new Vector2[width, length];

        switch (_origin != null)
        {
            case true:
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _length; j++)
                {
                    _cells[i, j] = GetLocalPos(i, j);
                }
            }
                break;

            case false:
            for (int i = 0; i < _width; i++)
            {
                for (int j = 0; j < _length; j++)
                {
                    _cells[i, j] = GetWorldPos(i, j);
                }
            }
                break;
        }        
    }

    /// <summary>
    /// Obtiene las coordenadas globales del centro de una celda a partir de su posición relativa en la cuadrícula.
    /// Se llama en el constructor no asociado a un gameObject para rellenar el array _cells
    /// </summary>
    /// <param name="x"> Índice x en el array bidimensional _cells </param>
    /// <param name="y"> Índice y en el array bidimensional _cells </param>
    /// <returns> Vector con las coordenadas cel centro de una celda </returns>
    Vector2 GetWorldPos(float x, float y) {
        x += _cellSize * 0.5f;
        y += _cellSize * 0.5f;
        return new Vector2(x, y) * _cellSize;
    }

    /// <summary>
    /// Obtiene las coordenadas locales con respecto a un gameObject del centro de una celda a partir de su posición relativa en la cuadrícula.
    /// Se hace a partir del método GetWorldPos.
    /// Se llama en el constructor asociado a un gameObject para rellenar el array _cells
    /// </summary>
    /// <param name="x"> Índice x en el array bidimensional _cells </param>
    /// <param name="y"> Índice y en el array bidimensional _cells </param>
    /// <returns> Vector con las coordenadas cel centro de una celda </returns>
    Vector2 GetLocalPos(float x, float y) {
        Vector2 pos = GetWorldPos(x, y);
        pos += _origin;
        return pos;
    }

    #endregion   

} // class Grid 
// namespace
