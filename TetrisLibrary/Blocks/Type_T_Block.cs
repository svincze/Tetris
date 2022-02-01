namespace TetrisLibrary;
public class Type_T_Block :Block{
    public override int Id => 6;
    protected override Position Spawn => new Position(0, 3);
    protected override Position[][] Tiles { get; } = new Position[][] {
        new Position[] {new(0,1), new(1,0),new(1,1),new(1,2)},
        new Position[] {new(0,1), new(1,1),new(1,2),new(2,1)},
        new Position[] {new(1,0), new(1,1),new(1,2),new(2,1)},
        new Position[] {new(0,1), new(1,0),new(1,1),new(2,1)}
    };
}

