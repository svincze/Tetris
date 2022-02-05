namespace TetrisLibrary;
/// <summary>
/// The array where the game is being played, aka GameGrid. The array is specified by the Rows and Columns.
/// </summary>
public class GameGrid{
    private readonly int[,] grid;
    public int Rows { get; }
    public int Cols { get; }

    //Q: Does this break the single responsibility principle? NO. Object only iteracts with it's own self.

    /// <summary>
    /// Indexer for the array game grid
    /// </summary>
    public int this[int r, int c]{
        get => grid[r, c];
        //This is a hidden element. The method returns a "value" when called even though no variable is defined here.
        //This is a built-in feature of C#. 
        set => grid[r, c] = value;
    }

    /// <summary>
    /// This is the constructor of our GameGrid class
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="columns"></param>
    public GameGrid(int rows, int columns) {
        Rows = rows;
        Cols = columns;
        grid = new int[rows, columns];
    }

    /// <summary>
    /// Checks if the given r(ow) and c(olumn) is within the initialized GameGrid
    /// </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    /// <returns>Boolean</returns>
    public bool IsInside(int r, int c) {
        return r >= 0 && r < Rows && c >= 0 && c < Cols;
    }

    /// <summary>
    /// Checks if the given r(ow) and c(olumn) coordinate has any value
    /// </summary>
    /// <param name="r"></param>
    /// <param name="c"></param>
    /// <returns>Boolean</returns>
    public bool IsEmpty(int r, int c) {
        return IsInside(r, c) && grid[r, c] == 0;
    }

    /// <summary>
    /// Check if the given r(ow) is full
    /// </summary>
    /// <param name="r"></param>
    /// <returns>Boolean</returns>
    public bool IsRowFull(int r) {
        for (int c = 0; c < Cols; c++) {
            if (grid[r, c] == 0) return false;
        }
        return true;
    }

    /// <summary>
    /// Check if the given r(ow) is empty
    /// </summary>
    /// <param name="r"></param>
    /// <returns>Boolean</returns>
    public bool IsRowEmpty(int r) {
        for (int c = 0; c < Cols; c++) {
            if (grid[r, c] != 0) return false;
        }
        return true;
    }

    /// <summary>
    /// Clears the given r(ow)
    /// </summary>
    /// <param name="r"></param>
    private void ClearRow(int r) {
        for (int c = 0; c < Cols; c++) {
            grid[r, c] = 0;
        }
    }

    /// <summary>
    /// Move the given r(ow) by the required amount
    /// </summary>
    /// <param name="r"></param>
    /// <param name="movedBy"></param>
    private void MoveRow(int r, int movedBy) {
        for (int c = 0; c < Cols; c++) {
            //copy row data to specified position param movedBy
            grid[r + movedBy, c] = grid[r, c];
            //empty the moved row
            grid[r, c] = 0;
        }
    }

    /// <summary>
    /// Clear all rows in the Gamegrid
    /// </summary>
    /// <returns>The number of cleared rows</returns>
    public int ClearFullRows() {
        int cleared = 0;
        //going from bottom to the top of the array
        for (int r = Rows - 1; r >= 0; r--) {
            if (IsRowFull(r)) {
                ClearRow(r);
                cleared++;
            }
            else if (cleared > 0) {
                MoveRow(r, cleared);
            }
        }
        return cleared;
    }
}
