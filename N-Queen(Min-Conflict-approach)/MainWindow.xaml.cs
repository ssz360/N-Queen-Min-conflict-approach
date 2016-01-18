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

namespace N_Queen_Min_Conflict_approach_
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        MinConflictAlgorithm chessBoard;
        public MainWindow()
        {
            InitializeComponent();
        }

        public bool ShouldStop { get; set; }
        public int NumberOfConflicts { get; set; }

        private void btnStop(object sender, RoutedEventArgs e)
        {
            this.ShouldStop = true;
            chessBoard.ShouldStop = true;
        }

        private void btnTry(object sender, RoutedEventArgs e)
        {
            int numberOfQueens;
            if (!int.TryParse(txtSize.Text, out numberOfQueens))
            {
                // todo: to something
            }

            SolveTheBoard(numberOfQueens);
        }

        void chessBoard_Finish(object sender, EventArgs e)
        {
            Dispatcher.Invoke(() => lblConflicts.Content = chessBoard.AllConflicts());
            DrawTheBoard(this.gridBoard, this.chessBoard);
            //this.ShouldStop = false;
        }

        private void DrawTheBoard(Grid uiBoardToDraw, MinConflictAlgorithm chessBoard)
        {
            Dispatcher.Invoke(() =>
            {
                uiBoardToDraw.ColumnDefinitions.Clear();
                uiBoardToDraw.RowDefinitions.Clear();
                uiBoardToDraw.Children.Clear();


                for (int i = 0; i < chessBoard.NumberOfQueens; i++)
                {
                    uiBoardToDraw.RowDefinitions.Add(new RowDefinition());
                    uiBoardToDraw.ColumnDefinitions.Add(new ColumnDefinition());
                }

                for (int i = 0; i < chessBoard.NumberOfQueens; i++)
                {
                    Ellipse queen = new Ellipse();

                    if (chessBoard.Conflicts[i])
                    {
                        queen.Fill = Brushes.Red;
                    }
                    else
                    {
                        queen.Fill = Brushes.Black;
                    }

                    int collumn = chessBoard.Board[i];

                    Grid.SetColumn(queen, collumn);
                    Grid.SetRow(queen, i);

                    uiBoardToDraw.Children.Add(queen);
                }
            });
        }

        private void btnSolve(object sender, RoutedEventArgs e)
        {
            int tries = (int)sldTries.Value;
            int gap = (int)sldGap.Value;
            int conflicts = (int)sldMinCinfolits.Value;
            int numberOfQueens;
            if (!int.TryParse( txtSize.Text,out numberOfQueens))
            {
                // todo: to something
            }

            this.NumberOfConflicts = int.MaxValue;

            SolveTheBoard(tries, conflicts, gap, numberOfQueens);
        }

        private async void SolveTheBoard(int tries, int conflicts, int delay, int numberOfQueens)
        {

            for (int i = 0; i < tries && !this.ShouldStop && this.NumberOfConflicts > conflicts; i++)
            {
                await SolveTheBoard(numberOfQueens);
                this.NumberOfConflicts = chessBoard.AllConflicts();
                await Task.Delay(delay);
            }
            this.ShouldStop = false;

        }

        private async Task SolveTheBoard(int numberOfQueens)
        {
            chessBoard = new MinConflictAlgorithm(numberOfQueens);
            chessBoard.Finish += chessBoard_Finish;

            await Task.Factory.StartNew(() => chessBoard.Solve());

            //return chessBoard;
        }

    }
}
