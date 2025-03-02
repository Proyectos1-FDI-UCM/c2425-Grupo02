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
public class IntGrid
{

    // ---- ATRIBUTOS PRIVADOS ----
    #region Atributos Privados (private fields)

    int _width;                     //Anchura de la cuadrícula
    int _height;                    //Altura de la cuadrícula
    int _cellSize;                  //Tamaño de cada celda del grid
    Vector2Int[,] _cells;           //Array bidimiensional con la posición central de cada celda en coordenadas locales

    #endregion
    
    // ---- MÉTODOS PÚBLICOS ----
    #region Métodos públicos
    
    //Constructor

    public IntGrid(int width, int height, int cellSize) {
        _width = width;
        _height = height;
        _cellSize = cellSize;
        _cells = new Vector2Int[width, height];

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++) 
            {
                _cells[i, j] = GetPos(i, j);
            }
        }
    }

    /*
    public bool Contains() {

    }
     */

    //Getters

    public int GetWidth() {
        return _width;
    }

    public int GetHeight() {
        return _height;
    }

    public int GetCellSize() {
        return _cellSize;
    }

    public Vector2Int[,] GetCells() {
        return _cells;
    }

    #endregion

    // ---- MÉTODOS PRIVADOS ----
    #region Métodos Privados

    //Cambiarlo a coordenadas locales
    private Vector2Int GetPos(int x, int y) {
        x += _cellSize / 2;
        y += _cellSize / 2;
        return new Vector2Int(x, y) * _cellSize;
    }

    #endregion   

} // class Grid 
// namespace
