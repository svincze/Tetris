namespace TetrisLibrary;
/// <summary>
/// The Block is an abstract class, meaning the class has a missing or incomplete implementation.
// An abstract class cannot be instantiated.
// An abstract class may contain abstract methods and accessors.
// The class that inherits from an abstract class needs to override it
/// </summary>
public abstract class Block {
    /// <summary>
    /// Id for a block
    /// </summary>
    public abstract int Id { get; }
    /// <summary>
    /// This variable defines the spwan position of each block
    /// </summary>
    //Protected means in this that the property here can only be accessed by the classes that inherits this class
    protected abstract Position Spawn { get; }
    /// <summary>
    /// Each block will have a multidimensional Position type (which has int row, and int column)
    /// The tiles variable is proctected, which means it's only accessible within its class and by derived class instances.
    /// </summary>
    protected abstract Position[][] Tiles { get; }
    /// <summary>
    /// The rotation state of a block
    /// </summary>
    private int rotationState;
    /// <summary>
    /// Offset of a block, meaning it's true position in the Game Grid
    /// </summary>
    private Position offset;

    public Block() {
        offset = new Position(Spawn.Row, Spawn.Column);
    }

    /// <summary>
    /// This method gets all the currently occupied Position coordinates based on the rotation of the block stored in Tiles
    /// </summary>
    /// <returns>Returns positions into an IEnumerable of type Position, basically a list of coordinates for the block</returns>
    public IEnumerable<Position> TilePosition() {
        return Tiles[rotationState].Select(item => new Position(item.Row + offset.Row, item.Column + offset.Column));
    }

    /// <summary>
    /// Rotates the block clockwise, the values are reseted on 4
    /// </summary>
    public void RotateStateCW() => rotationState = (rotationState + 1) % Tiles.Length;
     
    /// <summary>
    /// Rotates the block counter-clockwise, the values are reseted on 0
    /// </summary>
    public void RotateStateCCW() {
        if (rotationState == 0) {
            rotationState = Tiles.Length - 1;
        }
        else {
           rotationState--;
        }
    }

    /// <summary>
    /// Moves the block by the given rows and col
    /// </summary>
    /// <param name="rows"></param>
    /// <param name="col"></param>
    public void MoveBlock(int rows, int col) {
        offset = new Position(offset.Row + rows, offset.Column + col);
    }

    /// <summary>
    /// Resets the block state to the default, also sets the block to the spawn point
    /// </summary>
    public void ResetRotation() {
        rotationState = 0;
        offset = new Position(Spawn.Row, Spawn.Column);
    }

}