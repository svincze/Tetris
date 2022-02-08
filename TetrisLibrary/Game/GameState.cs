namespace TetrisLibrary;
public class GameState {
    /// <summary>
    /// This is a backing field
    /// </summary>
    //P&W: What's...a backing field? What is the purpose of this?
    //A backing field is just a field that is used by properties when you want to modify or use that private field data.Since the property has more logic (see property of Currentblock!)
    //In other words, a property is just a reference to another private variable.  Example:
    private Block currentBlock;

    /// <summary>
    /// This property resets the rotation when calling the current block
    /// </summary>
    public Block CurrentBlock {
        get => currentBlock;
        private set {
            currentBlock = value;
            currentBlock.ResetRotation();
            for (int i = 0; i < 2; i++) {
                currentBlock.MoveBlock(1, 0);
                if (!BlockFits()) currentBlock.MoveBlock(-1, 0);
            }
        }
    }

    public GameGrid Grid { get; }
    public BlockQueue Queue { get; }
    public bool GameOver { get; private set; }
    public int Score { get; private set; }
    public Block HeldBlock { get; private set; }
    public bool CanHold { get; private set; }
    public bool IsPaused { get; private set; }

    /// <summary>
    /// Ctor making a game grid upon creating the Gamestate object. We define here the size of the Tetris game
    /// </summary>
    public GameState() {
        Queue = new BlockQueue();
        Grid = new GameGrid(22, 10);
        CurrentBlock = Queue.GetAndUpdate();
        CanHold = true;
        IsPaused = false;
    }

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
        if (!BlockFits()) CurrentBlock.RotateStateCCW();
    }

    /// <summary>
    /// Game state tries to rotate the block ccw
    /// </summary>
    public void RotateBlockCCW() {
        CurrentBlock.RotateStateCCW();
        if (!BlockFits()) CurrentBlock.RotateStateCW();
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
        CurrentBlock.MoveBlock(0, 1);
        if (!BlockFits()) CurrentBlock.MoveBlock(0, -1);
    }

    /// <summary>
    /// Game state check if the game ended
    /// </summary>
    /// <returns>False if the top two rows are not empty</returns>
    private bool IsGameOver() {
        return !(Grid.IsRowEmpty(0) && Grid.IsRowEmpty(1));
    }

    public void Paused() => IsPaused = !IsPaused;

    /// <summary>
    /// Place the block and update the game if we cleared anything or if we lost the game
    /// </summary>
    private void PlaceBlock() {
        foreach (Position item in CurrentBlock.TilePosition()) {
            Grid[item.Row, item.Column] = CurrentBlock.Id;
        }

        Score += Grid.ClearFullRows();

        if (IsGameOver()) {
            GameOver = true;
        }
        else {
            CurrentBlock = Queue.GetAndUpdate();
            CanHold = true;
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

    public void HoldMyBlock() {
        if (!CanHold) return;
        if (HeldBlock == null) {
            HeldBlock = CurrentBlock;
            CurrentBlock = Queue.GetAndUpdate();
        }
        else {
            Block temp = CurrentBlock;
            CurrentBlock = HeldBlock;
            HeldBlock = temp;
        }

        CanHold = false;
    }

    //TASK: Rethink this
    /// <summary>
    /// Calculate the distance to the last occupied row in the column
    /// </summary>
    /// <param name="position"></param>
    /// <returns>The distance between the current position and the last empty one</returns>
    private int TileDropDistance(Position position) {
        int drop = 0;
        while (Grid.IsEmpty(position.Row + drop + 1, position.Column)) {
            drop++;
        }
        return drop;
    }

    public int BlockDropDistance() {
        int drop = Grid.Rows;
        foreach (Position position in CurrentBlock.TilePosition()) {
            drop = System.Math.Min(drop, TileDropDistance(position));
        }
        return drop;
    }

    public void DropBlock() {
        CurrentBlock.MoveBlock(BlockDropDistance(), 0);
        PlaceBlock();
    }
}
