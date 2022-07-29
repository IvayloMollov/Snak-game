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

namespace SnakeGame
{
   
    public partial class MainWindow : Window
    {
        Brush rectCol = Brushes.LightSlateGray;
        Brush lineCol = Brushes.Bisque;
        private Brush snakeColor = Brushes.DarkMagenta;
        private bool gameStarted = false;
        private bool move = false;
        private int score = 0;
        private int headSize = 20;
        private int lenght = 35;
        private Point startingPoint = new Point(75, 75);
        private Random rnd = new Random();
        private Point mousePos = new Point(75, 75);
        private Polyline polyline = new Polyline();
        private double stepSize = 5;
        private double stepSize2;
        private Ellipse snakeHead = new Ellipse();
        PointCollection food = new PointCollection();

        //creating a snake
        private void createSnake()
        {
            snakeHead.Fill = snakeColor;
            snakeHead.Width = headSize;
            snakeHead.Height = headSize;

            Canvas.SetTop(snakeHead, 75);
            Canvas.SetLeft(snakeHead, 75);

            polyline.Stroke = snakeColor;
            polyline.StrokeThickness = 10;
            polyline.StrokeLineJoin = PenLineJoin.Round;
            polyline.StrokeStartLineCap = PenLineCap.Round;
            polyline.StrokeEndLineCap = PenLineCap.Round;
            polyline.Points.Add(startingPoint); 
            paintCanvas.Children.Add(polyline);
            paintCanvas.Children.Add(snakeHead);

            stepSize2 = stepSize * stepSize;
        }
        // generates all food 
        private void paintFood(int index)
        {
            //check for difficulty and free space to generate food
            Point bonusPoint = new Point();
            if ((bool)mediumRadioButton.IsChecked)
            {
                do
                {
                    bonusPoint = new Point(rnd.Next(5, 630), rnd.Next(5, 390));
                } while (bonusPoint.X > paintCanvas.ActualWidth * 0.3 - 10 && bonusPoint.X < paintCanvas.ActualWidth * 0.7 + 10 &&
                   bonusPoint.Y > paintCanvas.ActualHeight * 0.3 - 10 && bonusPoint.Y < paintCanvas.ActualHeight * 0.7 + 10);
            }
            if ((bool)hardRadioButton.IsChecked)
            {
                do
                {
                    bonusPoint = new Point(rnd.Next(5, 630), rnd.Next(5, 390));
                } while (mousePos.X > paintCanvas.ActualWidth * 0.2 - 10 && mousePos.X < paintCanvas.ActualWidth * 0.4 + 10 &&
                           mousePos.Y > paintCanvas.ActualHeight * 0.2 - 10 && mousePos.Y < paintCanvas.ActualHeight * 0.4 + 10 &&
                           mousePos.X > paintCanvas.ActualWidth * 0.2 - 10 && mousePos.X < paintCanvas.ActualWidth * 0.4 + 10 &&
                           mousePos.Y > paintCanvas.ActualHeight * 0.6 - 10 && mousePos.Y < paintCanvas.ActualHeight * 0.8 + 10 &&
                           mousePos.X > paintCanvas.ActualWidth * 0.6 - 10 && mousePos.X < paintCanvas.ActualWidth * 0.8 + 10 &&
                           mousePos.Y > paintCanvas.ActualHeight * 0.2 - 10 && mousePos.Y < paintCanvas.ActualHeight * 0.4 + 10 &&
                           mousePos.X > paintCanvas.ActualWidth * 0.6 - 10 && mousePos.X < paintCanvas.ActualWidth * 0.8 + 10 &&
                           mousePos.Y > paintCanvas.ActualHeight * 0.6 - 10 && mousePos.Y < paintCanvas.ActualHeight * 0.8 + 10 &&
                           mousePos.X > paintCanvas.ActualWidth * 0.3 - 10 && mousePos.X < paintCanvas.ActualWidth * 0.7 + 10 &&
                           mousePos.Y > paintCanvas.ActualHeight * 0.4 - 10 && mousePos.Y < paintCanvas.ActualHeight * 0.6 + 10);
            }
            else { bonusPoint = new Point(rnd.Next(5, 630), rnd.Next(5, 390)); }

            //creating food
            Ellipse newEllipse = new Ellipse();
            newEllipse.Fill = Brushes.Red;
            newEllipse.Width = headSize;
            newEllipse.Height = headSize;

            Canvas.SetTop(newEllipse, bonusPoint.Y);
            Canvas.SetLeft(newEllipse, bonusPoint.X);
            paintCanvas.Children.Insert(index, newEllipse);
            food.Insert(index, bonusPoint);

        }

        private double Dist2(Point p1, Point p2) // The square of the distance between two points 
        {
            var dx = p1.X - p2.X;
            var dy = p1.Y - p2.Y;
            return dx * dx + dy * dy;
        }
        // Shows a game over message
        private void GameOver()
        {
            MessageBox.Show("You Lose! Your score is " + score.ToString(), "Game Over", MessageBoxButton.OK, MessageBoxImage.Hand);
            Reset();
        }
        //Method, which on call resets all variables to their initial state
        private void Reset()
        {
            paintCanvas.Children.Clear();
            score = 0;
            gameStarted = false;
            move = false;
            startingPoint = new Point(75, 75);
            mousePos = new Point(75, 75);
            polyline = new Polyline();
            easyRadioButton.IsChecked = false;
            mediumRadioButton.IsChecked = false;
            hardRadioButton.IsChecked = false;
            easyRadioButton.IsEnabled = true;
            mediumRadioButton.IsEnabled = true;
            hardRadioButton.IsEnabled = true;
        }
        public MainWindow()
        {
            InitializeComponent();
        }


        #region Boundaries and obstacles

        private void PaintRectangle(double setLeft, double setTop, double setWidth, double setHeoght, Brush myCol)
        {
            Rectangle rect = new Rectangle();
            rect.Width = setWidth;
            rect.Height = setHeoght;
            rect.Fill = myCol;
            rect.Stroke = myCol;
            rect.StrokeThickness = 2;
            Canvas.SetLeft(rect, setLeft);
            Canvas.SetTop(rect, setTop);

            paintCanvas.Children.Add(rect);
        }
        private void PaintBorders(Brush myCol)
        {
            Line topLine = new Line();
            topLine.StrokeStartLineCap = PenLineCap.Round;
            topLine.StrokeEndLineCap = PenLineCap.Round;
            topLine.Fill = myCol;
            topLine.StrokeThickness = 10;
            topLine.Stroke = myCol;
            topLine.X1 = 0;
            topLine.Y1 = 0;
            topLine.X2 = paintCanvas.ActualWidth;
            topLine.Y2 = 0;
            Line bottomLine = new Line();
            bottomLine.StrokeStartLineCap = PenLineCap.Round;
            bottomLine.StrokeEndLineCap = PenLineCap.Round;
            bottomLine.Fill = myCol;
            bottomLine.StrokeThickness = 10;
            bottomLine.Stroke = myCol;
            bottomLine.X1 = 0;
            bottomLine.Y1 = paintCanvas.ActualHeight;
            bottomLine.X2 = paintCanvas.ActualWidth;
            bottomLine.Y2 = paintCanvas.ActualHeight;
            Line leftLine = new Line();
            leftLine.Fill = myCol;
            leftLine.StrokeThickness = 10;
            leftLine.Stroke = myCol;
            leftLine.StrokeStartLineCap = PenLineCap.Round;
            leftLine.StrokeEndLineCap = PenLineCap.Round;
            leftLine.X1 = 0;
            leftLine.Y1 = 0;
            leftLine.X2 = 0;
            leftLine.Y2 = paintCanvas.ActualHeight;
            Line rightLine = new Line();
            rightLine.StrokeStartLineCap = PenLineCap.Round;
            rightLine.StrokeEndLineCap = PenLineCap.Round;
            rightLine.Fill = myCol;
            rightLine.StrokeThickness = 10;
            rightLine.Stroke = myCol;
            rightLine.X1 = paintCanvas.ActualWidth;
            rightLine.Y1 = 0;
            rightLine.X2 = paintCanvas.ActualWidth;
            rightLine.Y2 = paintCanvas.ActualHeight;
            paintCanvas.Children.Add(topLine);
            paintCanvas.Children.Add(bottomLine);
            paintCanvas.Children.Add(leftLine);
            paintCanvas.Children.Add(rightLine);

        }
        #endregion
        //Button to start a game on a chosen difficulty
        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            createSnake();
            int maxFood = 12;
            if ((bool)(mediumRadioButton.IsChecked))
                maxFood = 10;
            else if ((bool)(hardRadioButton.IsChecked))
                maxFood = 7;
            for (int i = 0; i < maxFood; i++)
            {
                paintFood(i);
            }
         


            if (gameStarted)
            {
                gameStarted = false;
            }
            else
            {
                gameStarted = true;
            }
            if (gameStarted)
            {
                easyRadioButton.IsEnabled = false;
                mediumRadioButton.IsEnabled = false;
                hardRadioButton.IsEnabled = false;
            }
            else
            {
                easyRadioButton.IsEnabled = true;
                mediumRadioButton.IsEnabled = true;
                hardRadioButton.IsEnabled = true;
            }
        }

        private void paintCanvas_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        #region Buttons_to_set_difficulty
        //clearing the canvas and adding obstacles for easy difficulty
        private void easyRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            paintCanvas.Children.Clear();
            PaintBorders(lineCol);
        }
        //clearing the canvas and adding obstacles for medium difficulty
        private void mediumRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            paintCanvas.Children.Clear();
            PaintRectangle(paintCanvas.ActualWidth * 0.3, paintCanvas.ActualHeight * 0.3,
                paintCanvas.ActualWidth * 0.4, paintCanvas.ActualHeight * 0.4, rectCol);
            PaintBorders(lineCol);
        }
        //clearing the canvas and adding obstacles for hard difficulty
        private void hardRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            paintCanvas.Children.Clear();
            PaintBorders(lineCol);
            PaintRectangle(paintCanvas.ActualWidth * 0.2, paintCanvas.ActualHeight * 0.2,
            paintCanvas.ActualWidth * 0.2, paintCanvas.ActualHeight * 0.2, rectCol);
            PaintRectangle(paintCanvas.ActualWidth * 0.2, paintCanvas.ActualHeight * 0.6,
            paintCanvas.ActualWidth * 0.2, paintCanvas.ActualHeight * 0.2, rectCol);
            PaintRectangle(paintCanvas.ActualWidth * 0.6, paintCanvas.ActualHeight * 0.2,
            paintCanvas.ActualWidth * 0.2, paintCanvas.ActualHeight * 0.2, rectCol);
            PaintRectangle(paintCanvas.ActualWidth * 0.6, paintCanvas.ActualHeight * 0.6,
            paintCanvas.ActualWidth * 0.2, paintCanvas.ActualHeight * 0.2, rectCol);
            PaintRectangle(paintCanvas.ActualWidth * 0.30, paintCanvas.ActualHeight * 0.4,
            paintCanvas.ActualWidth * 0.4, paintCanvas.ActualHeight * 0.2, rectCol);
        }
        //Moving the snake and testing for collisions
        #endregion
        private void paintCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            base.OnMouseMove(e);
            Point newMousePos = e.GetPosition(paintCanvas); // Store the position to test
            bool near = newMousePos.X > 45 && newMousePos.X < 105 && newMousePos.Y > 45 && newMousePos.Y <= 105;
            if (near) { move = true; }
            if (move && gameStarted)
            {
                if (Dist2(newMousePos, mousePos) > stepSize2) // Check if the distance is far enough
                {
                    double dx = newMousePos.X - mousePos.X;
                    double dy = newMousePos.Y - mousePos.Y;

                    if (Math.Abs(dx) > Math.Abs(dy)) // Test in which direction the snake is going
                        mousePos.X += Math.Sign(dx) * stepSize;
                    else { mousePos.Y += Math.Sign(dy) * stepSize; }

                    Canvas.SetTop(snakeHead, mousePos.Y - (headSize / 2));
                    Canvas.SetLeft(snakeHead, mousePos.X - (headSize / 2));
                    polyline.Points.Add(mousePos);

                    if (polyline.Points.Count > lenght) // Keep the snake lenght under maximum lenght allowed
                        polyline.Points.RemoveAt(0);

                    //Eating food
                    int n = 0;
                    foreach (Point point in food)
                    {

                        if ((Math.Abs(point.X - mousePos.X) < headSize) &&
                            (Math.Abs(point.Y - mousePos.Y) < headSize))
                        {

                            lenght += 2;
                            score += 1;
                            lblScore.Content = "Score: " + score.ToString();
                            // In the case of food consumption, erase the food object
                            // from the list of food as well as from the canvas
                            food.RemoveAt(n);
                            paintCanvas.Children.RemoveAt(n);
                            paintFood(n);
                            break;
                        }
                        n++;
                    }



                    #region Detecting_contact_with_surfaces_or_snake_tail
                    // Contact with the outer border of the canvas
                    if ((mousePos.X < 10) || (mousePos.X > paintCanvas.ActualWidth - 10) ||
                        (mousePos.Y < 10) || (mousePos.Y > paintCanvas.ActualHeight - 10))
                        GameOver();
                    // Obstacles for medium difficulty
                    if ((bool)mediumRadioButton.IsChecked)
                    {
                        if (mousePos.X > paintCanvas.ActualWidth * 0.3 - 10 && mousePos.X < paintCanvas.ActualWidth * 0.7 + 10 &&
                           mousePos.Y > paintCanvas.ActualHeight * 0.3 - 10 && mousePos.Y < paintCanvas.ActualHeight * 0.7 + 10)
                            GameOver();
                    }



                    // Obstacles for hard difficulty
                    if ((bool)hardRadioButton.IsChecked)
                    {
                        if (mousePos.X > paintCanvas.ActualWidth * 0.2 - 10 && mousePos.X < paintCanvas.ActualWidth * 0.4 + 10 &&
                           mousePos.Y > paintCanvas.ActualHeight * 0.2 - 10 && mousePos.Y < paintCanvas.ActualHeight * 0.4 + 10)
                            GameOver();
                        if (mousePos.X > paintCanvas.ActualWidth * 0.2 - 10 && mousePos.X < paintCanvas.ActualWidth * 0.4 + 10 &&
                           mousePos.Y > paintCanvas.ActualHeight * 0.6 - 10 && mousePos.Y < paintCanvas.ActualHeight * 0.8 + 10)
                            GameOver();
                        if (mousePos.X > paintCanvas.ActualWidth * 0.6 - 10 && mousePos.X < paintCanvas.ActualWidth * 0.8 + 10 &&
                           mousePos.Y > paintCanvas.ActualHeight * 0.2 - 10 && mousePos.Y < paintCanvas.ActualHeight * 0.4 + 10)
                            GameOver();
                        if (mousePos.X > paintCanvas.ActualWidth * 0.6 - 10 && mousePos.X < paintCanvas.ActualWidth * 0.8 + 10 &&
                           mousePos.Y > paintCanvas.ActualHeight * 0.6 - 10 && mousePos.Y < paintCanvas.ActualHeight * 0.8 + 10)
                            GameOver();
                        if (mousePos.X > paintCanvas.ActualWidth * 0.3 - 10 && mousePos.X < paintCanvas.ActualWidth * 0.7 + 10 &&
                           mousePos.Y > paintCanvas.ActualHeight * 0.4 - 10 && mousePos.Y < paintCanvas.ActualHeight * 0.6 + 10)
                            GameOver();
                    }
                    // crossing the snake tail
                    for (int i = 0; i < (polyline.Points.Count - headSize); i++)
                    {
                        Point point = new Point(polyline.Points[i].X, polyline.Points[i].Y);
                        if ((Math.Abs(point.X - mousePos.X) < (headSize)) &&
                             (Math.Abs(point.Y - mousePos.Y) < (headSize)))
                        {
                            GameOver();
                            break;
                        }

                    }
                    #endregion

                }
            }
        }
        //Help button
        private void btnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("1. Guide the snake to reach the food. \n2. Try to steer clear of any obstacle or your own tail \n" +
                "3. Hitting an obstacle or crossing your tail ends the game", "Game rules", MessageBoxButton.OK, MessageBoxImage.Information);

        }
    }
}
