using Snake.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer _gameTimer = new DispatcherTimer();
        private Random rnd = new Random();

        const int SnakeSquareSize = 20;
        const int SnakeStartLength = 3;
        const int SnakeStartSpeed = 400;
        const int SnakeSpeedThreshold = 100;

        private readonly List<SnakePart> SnakeParts = new List<SnakePart>();
        private SnakeDirection SnakeDirection = SnakeDirection.Right;
        private UIElement snakeFood = null;
        private int SnakeLength;
        private int currentScore = 0;

        public MainWindow()
        {
            InitializeComponent();
            _gameTimer.Tick += GameTickTimer_Tick;
        }

        private void GameTickTimer_Tick(object sender, EventArgs e)
        {
            MoveSnake();
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            DrawGameArea();
            StartNewGame();
        }

        private void DrawGameArea()
        {
            bool doneDrawingBackground = false;
            int nextX = 0, nextY = 0;
            int rowCounter = 0;
            bool nextIsOdd = false;

            while (doneDrawingBackground == false)
            {
                var rect = new Rectangle
                {
                    Width = SnakeSquareSize,
                    Height = SnakeSquareSize,
                    Fill = nextIsOdd ? Brushes.White : Brushes.Black
                };

                GameArea.Children.Add(rect);
                Canvas.SetTop(rect, nextY);
                Canvas.SetLeft(rect, nextX);

                nextIsOdd = !nextIsOdd;
                nextX += SnakeSquareSize;

                if (nextX >= GameArea.ActualWidth)
                {
                    nextX = 0;
                    nextY += SnakeSquareSize;
                    rowCounter++;
                    nextIsOdd = rowCounter % 2 != 0;
                }

                if (nextY >= GameArea.ActualHeight)
                    doneDrawingBackground = true;
            }
        }

        private void DrawSnake()
        {
            foreach (var snakePart in SnakeParts)
            {
                if (snakePart.UiElement == null)
                {
                    snakePart.UiElement = new Rectangle()
                    {
                        Width = SnakeSquareSize,
                        Height = SnakeSquareSize,
                        Fill = (snakePart.IsHead ? Brushes.YellowGreen : Brushes.Green)
                    };

                    GameArea.Children.Add(snakePart.UiElement);
                    Canvas.SetTop(snakePart.UiElement, snakePart.Position.Y);
                    Canvas.SetLeft(snakePart.UiElement, snakePart.Position.X);
                }
            }
        }

        private void MoveSnake()
        {
            while (SnakeParts.Count >= SnakeLength)
            {
                GameArea.Children.Remove(SnakeParts[0].UiElement);
                SnakeParts.RemoveAt(0);
            }

            foreach (var snakePart in SnakeParts)
            {
                (snakePart.UiElement as Rectangle).Fill = Brushes.Green;
                snakePart.IsHead = false;
            }

            var snakeHead = SnakeParts[^1];
            double nextX = snakeHead.Position.X;
            double nextY = snakeHead.Position.Y;

            switch (SnakeDirection)
            {
                case SnakeDirection.Left:
                    nextX -= SnakeSquareSize;
                    break;
                case SnakeDirection.Right:
                    nextX += SnakeSquareSize;
                    break;
                case SnakeDirection.Up:
                    nextY -= SnakeSquareSize;
                    break;
                case SnakeDirection.Down:
                    nextY += SnakeSquareSize;
                    break;
                default:
                    break;
            }

            SnakeParts.Add(new SnakePart()
            {
                Position = new Point(nextX, nextY),
                IsHead = true
            });

            DrawSnake();
            DoCollisionCheck();
        }

        private void StartNewGame()
        {
            foreach (var snakeBodyPart in SnakeParts)
            {
                if (snakeBodyPart.UiElement != null)
                    GameArea.Children.Remove(snakeBodyPart.UiElement);
            }

            SnakeParts.Clear();
            
            if (snakeFood != null)
                GameArea.Children.Remove(snakeFood);

            currentScore = 0;
            SnakeLength = SnakeStartLength;
            SnakeDirection = SnakeDirection.Right;
            SnakeParts.Add(new SnakePart() { Position = new Point(SnakeSquareSize * 5, SnakeSquareSize * 5) });
            _gameTimer.Interval = TimeSpan.FromMilliseconds(SnakeStartSpeed);

            DrawSnake();
            DrawSnakeFood();

            UpdateGameStatus();
            _gameTimer.IsEnabled = true;
        }

        private Point GetNextFoodPosition()
        {
            int maxX = (int)(GameArea.ActualWidth / SnakeSquareSize);
            int maxY = (int)(GameArea.ActualHeight / SnakeSquareSize);

            int foodX = rnd.Next(0, maxX) * SnakeSquareSize;
            int foodY = rnd.Next(0, maxY) * SnakeSquareSize;

            foreach (var snakePart in SnakeParts)
            {
                if ((snakePart.Position.X == foodX) && (snakePart.Position.Y == foodY))
                    return GetNextFoodPosition();
            }

            return new Point(foodX, foodY);
        }

        private void DrawSnakeFood()
        {
            var foodPosition = GetNextFoodPosition();
            snakeFood = new Ellipse()
            {
                Width = SnakeSquareSize,
                Height = SnakeSquareSize,
                Fill = Brushes.Red
            };

            GameArea.Children.Add(snakeFood);
            Canvas.SetTop(snakeFood, foodPosition.Y);
            Canvas.SetLeft(snakeFood, foodPosition.X);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            var originalSnakeDirection = SnakeDirection;
            switch (e.Key)
            {
                case Key.Up:
                    if (SnakeDirection != SnakeDirection.Down)
                        SnakeDirection = SnakeDirection.Up;
                    break;
                case Key.Down:
                    if (SnakeDirection != SnakeDirection.Up)
                        SnakeDirection = SnakeDirection.Down;
                    break;
                case Key.Left:
                    if (SnakeDirection != SnakeDirection.Right)
                        SnakeDirection = SnakeDirection.Left;
                    break;
                case Key.Right:
                    if (SnakeDirection != SnakeDirection.Left)
                        SnakeDirection = SnakeDirection.Right;
                    break;
                case Key.Space:
                case Key.Enter:
                    StartNewGame();
                    break;
            }
            
            if (SnakeDirection != originalSnakeDirection)
                MoveSnake();
        }

        private void DoCollisionCheck()
        {
            SnakePart snakeHead = SnakeParts[^1];

            if ((snakeHead.Position.X == Canvas.GetLeft(snakeFood)) && (snakeHead.Position.Y == Canvas.GetTop(snakeFood)))
            {
                EatSnakeFood();
                return;
            }

            if ((snakeHead.Position.Y < 0) || (snakeHead.Position.Y >= GameArea.ActualHeight) ||
            (snakeHead.Position.X < 0) || (snakeHead.Position.X >= GameArea.ActualWidth))
            {
                EndGame();
            }

            foreach (SnakePart snakeBodyPart in SnakeParts.Take(SnakeParts.Count - 1))
            {
                if ((snakeHead.Position.X == snakeBodyPart.Position.X) && (snakeHead.Position.Y == snakeBodyPart.Position.Y))
                    EndGame();
            }
        }

        private void EatSnakeFood()
        {
            SnakeLength++;
            currentScore++;
            int timerInterval = Math.Max(SnakeSpeedThreshold, (int)_gameTimer.Interval.TotalMilliseconds - (currentScore * 2));
            _gameTimer.Interval = TimeSpan.FromMilliseconds(timerInterval);
            GameArea.Children.Remove(snakeFood);
            DrawSnakeFood();
            UpdateGameStatus();
        }

        private void EndGame()
        {
            _gameTimer.IsEnabled = false;
            MessageBox.Show(this, "Oooops, you died!\n\nTo start a new game, just press the Space bar...", "SnakeWPF");
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void UpdateGameStatus()
        {
            this.tbStatusScore.Text = currentScore.ToString();
            this.tbStatusSpeed.Text = _gameTimer.Interval.TotalMilliseconds.ToString();
        }
    }
}
