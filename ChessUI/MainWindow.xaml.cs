using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChessUI
{
    public partial class MainWindow : Window
    {
        private bool isDragging;
        private UIElement currentElement;
        private Point originalPosition;

        public MainWindow()
        {
            InitializeComponent();

            // Attach mouse event handlers to the grid
            Grid.MouseLeftButtonDown += Grid_MouseLeftButtonDown;
            Grid.MouseMove += Grid_MouseMove;
            Grid.MouseLeftButtonUp += Grid_MouseLeftButtonUp;
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Check if the clicked element is an image
            if (e.OriginalSource is Image)
            {
                // Start dragging the piece
                isDragging = true;
                currentElement = (UIElement)e.OriginalSource;
                originalPosition = e.GetPosition(Grid);

                // Capture the mouse to handle mouse events outside the element
                currentElement.CaptureMouse();
            }
        }

        private void Grid_MouseMove(object sender, MouseEventArgs e)
        {
            if (isDragging && currentElement != null)
            {
                // Calculate the new position of the element
                Point currentPosition = e.GetPosition(Grid);
                double deltaX = currentPosition.X - originalPosition.X;
                double deltaY = currentPosition.Y - originalPosition.Y;

                // Update the position of the element within the Grid
                int newRow = (int)Math.Floor((currentPosition.Y + currentElement.RenderSize.Height / 2) / 50);
                int newColumn = (int)Math.Floor((currentPosition.X + currentElement.RenderSize.Width / 2) / 50);

                // Ensure the new position is within the Grid boundaries
                newRow = Math.Max(0, Math.Min(newRow, Grid.RowDefinitions.Count - 1));
                newColumn = Math.Max(0, Math.Min(newColumn, Grid.ColumnDefinitions.Count - 1));

                // Update the Grid.Row and Grid.Column properties of the element
                Grid.SetRow(currentElement, newRow);
                Grid.SetColumn(currentElement, newColumn);
            }
        }

        private void Grid_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Stop dragging the piece
            isDragging = false;

            // Release the mouse capture
            currentElement?.ReleaseMouseCapture();
            currentElement = null;
        }
    }
}
