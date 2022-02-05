using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using TetrisLibrary;

namespace Tetris
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //Should create MVVM
        //The order of the tile images matches the block ids
        private readonly ImageSource[] tileImages = new ImageSource[] {
            new BitmapImage(new Uri("Assets/TileEmpty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileCyan.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileBlue.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileOrange.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileYellow.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileGreen.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TilePurple.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/TileRed.png", UriKind.Relative)),
        };

        //The order of the tile images matches the block ids
        private readonly ImageSource[] blockImages = new ImageSource[] {
            new BitmapImage(new Uri("Assets/Block-Empty.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-I.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-J.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-L.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-O.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-S.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-T.png", UriKind.Relative)),
            new BitmapImage(new Uri("Assets/Block-Z.png", UriKind.Relative)),
        };

        private readonly Image[,] imageControls;
        private GameState gameState = new();
        private readonly int maxDelay = 1000;
        private readonly int minDelay = 75;
        private readonly int delayDecrease = 25;
       
        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.Grid);
        }

        /// <summary>
        /// This method draws a tile for every block in the two-dimensional array
        /// </summary>
        /// <param name="grid"></param>
        /// <returns>This method returns the game grid array</returns>
        private Image[,] SetupGameCanvas(GameGrid grid) {
            Image[,] imageControls = new Image[grid.Rows, grid.Cols];
            int cellsize = 25;
            for (int r = 0; r < grid.Rows; r++) {
                for (int c = 0; c < grid.Cols; c++) {
                    Image imageControl = new Image {
                        Width = cellsize,
                        Height = cellsize,
                    };


                    Canvas.SetTop(imageControl, (r - 2) * cellsize + 10);
                    Canvas.SetLeft(imageControl, c * cellsize);
                    GameCanvas.Children.Add(imageControl);
                    imageControls[r, c] = imageControl;
                }
            }

            return imageControls;
        }

        private void DrawGrid(GameGrid grid) {
            for (int r = 0; r < grid.Rows; r++) {
                for (int c = 0; c < grid.Cols; c++) {
                    int id = grid[r, c];
                    imageControls[r, c].Opacity = 1;
                    imageControls[r,c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(TetrisLibrary.Block block) {
            foreach (Position position in block.TilePosition()) {
                imageControls[position.Row, position.Column].Opacity = 1;
                imageControls[position.Row, position.Column].Source = tileImages[block.Id];
            }
        }

        private void DrawNextBlock(BlockQueue blockQueue) {
            TetrisLibrary.Block next = blockQueue.NextBlock;
            NextImage.Source = blockImages[next.Id];
        }

        private void DrawHoldBlock(TetrisLibrary.Block heldBlock) {
            if (heldBlock == null) {
                HoldImage.Source = blockImages[0];
            }
            else {
                HoldImage.Source = blockImages[heldBlock.Id];
            }
        }

        private void DrawGhostBlock(TetrisLibrary.Block block) {
            int dropDistance = gameState.BlockDropDistance();
            foreach (Position position in block.TilePosition()) {
                imageControls[position.Row + dropDistance, position.Column].Opacity = 0.25;
                imageControls[position.Row + dropDistance, position.Column].Source = tileImages[block.Id];
            }
        }

        private void Draw(GameState gameState) {
            DrawGrid(gameState.Grid);
            DrawGhostBlock(gameState.CurrentBlock);
            DrawBlock(gameState.CurrentBlock);
            DrawNextBlock(gameState.Queue);
            DrawHoldBlock(gameState.HeldBlock);
            ScoreText.Text = $"Score : {gameState.Score}";
        }

        private async void GameCanvas_Loaded(object sender, RoutedEventArgs e) {
            await GameLoop();
        }

        private async void Retry_Button(object sender, RoutedEventArgs e) {
            gameState = new GameState();
            GameOverMenu.Visibility = Visibility.Hidden;
            await GameLoop();
        }


        /// <summary>
        /// This has to be aysnc because want to wait for the UI
        /// </summary>
        /// <returns></returns>
        private async Task GameLoop() {
        Draw(gameState);
           //This moves our block automatically downwards
            while (!gameState.GameOver) {
                if (!gameState.IsPaused) {
                    PauseMenu.Visibility = Visibility.Hidden;
                    int delay = Math.Max(minDelay, maxDelay - gameState.Score * delayDecrease);
                    await Task.Delay(delay);
                    gameState.MoveBlockDown();

                }
                else {
                    await Task.Delay(50);
                    PauseMenu.Visibility = Visibility.Visible;
                }
                Draw(gameState);
            }
            //We exit the loop, meaning it's game over!
            GameOverMenu.Visibility = Visibility.Visible;
            FinalScoreText.Text = $"Score : {gameState.Score}";
        }


        private void Window_KeyDown(object sender, KeyEventArgs e) {
            if (gameState.GameOver) return;
            if (e.Key == Key.P) gameState.Paused();
            if (!gameState.IsPaused) {
                switch (e.Key) {
                    case Key.Left:
                        gameState.MoveBlockLeft();
                        break;
                    case Key.Right:
                        gameState.MoveBlockRight();
                        break;
                    case Key.Down:
                        gameState.MoveBlockDown();
                        break;
                    case Key.Up:
                        gameState.RotateBlockCW();
                        break;
                    case Key.Z:
                        gameState.RotateBlockCCW();
                        break;
                    case Key.C:
                        gameState.HoldMyBlock();
                        break;
                    case Key.Space:
                        gameState.DropBlock();
                        break;
                    case Key.Q:
                        App.Current.Shutdown();
                        break;
                    default:
                        return;
                }
            }
            Draw(gameState);
        }
    }
}
