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
        public MainWindow()
        {
            InitializeComponent();
            imageControls = SetupGameCanvas(gameState.Grid);
        }

        private Image[,] SetupGameCanvas(GameGrid grid) {
            Image[,] imageControls = new Image[grid.Rows, grid.Cols];
            int cellsize = 25;
            for (int r = 0; r < grid.Rows; r++) {
                for (int c = 0; c < grid.Cols; c++) {
                    Image imageControl = new Image {
                        Width = cellsize,
                        Height = cellsize,
                    };


                    Canvas.SetTop(imageControl, (r - 2) * cellsize);
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
                    imageControls[r,c].Source = tileImages[id];
                }
            }
        }

        private void DrawBlock(TetrisLibrary.Block block) {
            foreach (Position position in block.TilePosition()) {
                imageControls[position.Row, position.Column].Source = tileImages[block.Id];
            }
        }

        private void GameCanvas_Loaded(object sender, RoutedEventArgs e) {

        }

        private void Button_Click(object sender, RoutedEventArgs e) {

        }
    }
}
