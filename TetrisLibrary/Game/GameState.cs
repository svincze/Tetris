namespace TetrisLibrary;
public class GameState {
    /// <summary>
    /// This is a backing field
    /// </summary>
    //P&W: What's...a backing field? What is the purpose of this?
    private Block currentBlock;
    /// <summary>
    /// This ctor resets the rotation when calling the current block
    /// </summary>
    public Block CurrentBlock {
        get => currentBlock;
        private set {
            currentBlock = value;
            currentBlock.ResetRotation();
        }
    }

    public GameGrid Grid { get; }
    public BlockQueue Queue { get; }
    public bool GameOver { get; private set; }

    /// <summary>
    /// Ctor making a game grid. We define here the size of the Tetris game
    /// </summary>
    public GameState() {
        Queue = new BlockQueue();
        Grid = new GameGrid(22, 10);
        CurrentBlock = Queue.GetAndUpdate();
    }


    //PIOTR\WIKTOR: Wouldn't it be better to precheck everything the user tries to do? This always operates with a true game object

    /// <summary>
    /// This method checks if the current block has a valid position in the grid
    /// </summary>
    /// <returns>Returns false even if one block element is not empty in the grid</returns>
    private bool BlockFits() {
        foreach (Position item in CurrentBlock.TilePosition()) {
            if (!Grid.IsEmpty(item.Row, item.Column)) return false;
        }
        return true;
    }


    /// <summary>
    /// Game state tries to rotate the block cw
    /// </summary>
    public void RotateBlockCW() {
        CurrentBlock.RotateStateCW();
        if(!BlockFits()) CurrentBlock.RotateStateCCW();
    }

    /// <summary>
    /// Game state tries to rotate the block ccw
    /// </summary>
    public void RotateBlockCCW() {
        CurrentBlock.RotateStateCCW();
        if(!BlockFits()) CurrentBlock.RotateStateCW();
    }

    /// <summary>
    /// Game state tries to move the block by 1 to the left
    /// </summary>
    public void MoveBlockLeft() {
        CurrentBlock.MoveBlock(0, -1);
        if (!BlockFits()) CurrentBlock.MoveBlock(0, 1);
    }

    /// <summary>
    /// Game state tries to move the block by 1 to the right
    /// </summary>
    public void MoveBlockRight() {
        CurrentBlock.MoveBlock(0, -1);
        if (!BlockFits()) CurrentBlock.MoveBlock(0, -1);
    }

    /// <summary>
    /// Game state check if the game ended
    /// </summary>
    /// <returns>False if the top two rows are not empty</returns>
    private bool IsGameOver() {
        return !(Grid.IsRowEmpty(0) && Grid.IsRowEmpty(1));
    }

    /// <summary>
    /// Place the block and update the game if we cleared anything or if we lost the game
    /// </summary>
    private void PlaceBlock() {
        foreach (Position item in CurrentBlock.TilePosition()) {
            Grid[item.Row, item.Column] = CurrentBlock.Id;
        }

        Grid.ClearFullRows();

        if (IsGameOver()) {
            GameOver = true;
        }
        else {
            CurrentBlock = Queue.GetAndUpdate();
        }
    }

    /// <summary>
    /// Game state tries to move the block by 1 down
    /// </summary>
    public void MoveBlockDown() {
        CurrentBlock.MoveBlock(1, 0);
        if (!BlockFits()) {
            CurrentBlock.MoveBlock(-1, 0);
            PlaceBlock();
        }
    }
}
