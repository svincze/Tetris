using System;
namespace TetrisLibrary;
public class BlockQueue {
    /// <summary>
    /// Block array containing the different kind of blocks we have
    /// </summary>
    private readonly Block[] blocks = new Block[] {
        new Type_I_Block(),
        new Type_J_Block(),
        new Type_L_Block(),
        new Type_O_Block(),
        new Type_S_Block(),
        new Type_T_Block(),
        new Type_Z_Block(),
    };

    public BlockQueue() {
        NextBlock = ReturnRandomBlock();
    }

    private readonly Random random = new Random();
    /// <summary>
    /// This gets the block in the queue
    /// </summary>
    public Block NextBlock { get; private set; }

    private Block ReturnRandomBlock() {
        return blocks[random.Next(blocks.Length)];
    }
    /// <summary>
    /// This method selects a block from the array that differs from our current one
    /// </summary>
    /// <returns>The next randomly selected block</returns>
    public Block GetAndUpdate() {
        return blocks.First(x => x.Id != NextBlock.Id);
    }
}