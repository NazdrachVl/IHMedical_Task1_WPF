using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace IHMedicalTask1WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        Regex regex;
        CheckersBoard board;

        public MainWindow()
        {
            regex = new Regex(@"^btn(\d)_(\d)$");
            board = new CheckersBoard();

            InitializeComponent();
            UpdateAll();
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            CellPosition cell = null;
            if (board.CellPosition1 != null)
            {
                cell = board.CellPosition1.Copy();
            }
            var match = regex.Match((sender as Button).Name);

            if (board.CheckAttackChekers())
            {
                board.Attack(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));
				UpdateAll();
			}
            else
            {
                board.Step(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value));

                if (cell != null)
                {
                    UpdateCell(cell.X, cell.Y, board[cell.X, cell.Y]);
                    UpdateCell(int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value), board[int.Parse(match.Groups[1].Value), int.Parse(match.Groups[2].Value)]);
                }
            }        
        }

        private void UpdateCell(Button btn, Cell cell)
        {
            var viewbox = new Viewbox();
            var grid = new Grid();
            if (cell.Type != CellType.Empty)
            {
                grid.Children.Add(new Ellipse()
                {
                    Fill = cell.Type == CellType.Black ? Brushes.Black : Brushes.White,
                    Stroke = cell.Type == CellType.Black ? Brushes.White : Brushes.Red,
                    StrokeThickness = 2,
                    Height = 30,
                    Width = 30,
                    Margin = new Thickness(5)
                });
                if (cell.IsQueen)
                {
                    grid.Children.Add(new Ellipse()
                    {
                        Fill = Brushes.Red,
                        Stroke = Brushes.Red,
                        StrokeThickness = 2,
                        Height = 10,
                        Width = 10,
                        Margin = new Thickness(5)
                    });
                }
            }
            viewbox.Child = grid;
            btn.Content = viewbox;
        }

        private void UpdateCell(int x, int y, Cell cell)
        {
            Button btn = (Button)FindName($"btn{x}_{y}");
            UpdateCell(btn, cell);
        }

        private void UpdateAll()
        {
            for (int i = 0; i < CheckersBoard.BoardSize; i++)
            {
                for (int j = 0; j < CheckersBoard.BoardSize; j++)
                {
                    UpdateCell(i, j, board[i, j]);
                }
            }
        }


    }
}
