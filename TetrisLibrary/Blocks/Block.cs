namespace TetrisLibrary;
/// <summary>
/// The Block is an abstract class, meaning the class has a missing or incomplete implementation.
/// An abstract class cannot be instantiated.
/// An abstract class may contain abstract methods and accessors.
/// </summary>
public abstract class Block {
    /// <summary>
    /// Each block will have a multidimensional Position type (which has int row, and int column)
    /// The tiles variable is proctected, which means it's only accessible within its class and by derived class instances.
    /// </summary>
    protected abstract Position[][] Tiles { get; }
    /// <summary>
    /// This variable defines the spwan position of each block
    /// </summary>
    protected abstract Position Spawn { get; }

    /// <summary>
    /// Id for a block
    /// </summary>
    public abstract int Id { get; }
    /// <summary>
    /// The rotation state of a block
    /// </summary>
    private int rotationSate;
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
    /// <returns>Yield returns positions into an IEnumerable of type Position, basically a list of coordinates for the block</returns>
    public IEnumerable<Position> TilePosition() {
        foreach (Position item in Tiles[rotationSate]) {
            //AskWiktor\Piotr: Why a yield here? I'd created a new List (which inherits from IEnum) of type Position
            //and on each iteration I'd have added a new position to that the list.
            //At the end return the newly made list. 
            //Q: Does the yield return adds to the IEnum<Position> on each iteration? Why should I use yield return here?
            yield return new Position(item.Row + offset.Row, item.Column + offset.Column);
        }
    }
}