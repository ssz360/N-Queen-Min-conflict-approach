using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace N_Queen_Min_Conflict_approach_
{
    public class MinConflictAlgorithm
    {
        private int numberOfQueens;
        private int[] board;
        private bool[] conflicts;
        private bool shouldStop;
        public event EventHandler Finish;

        public int NumberOfQueens
        {
            get { return numberOfQueens; }
        }

        public int[] Board
        {
            get { return board; }
            set { board = value; }
        }

        public bool[] Conflicts
        {
            get { return conflicts; }
            set { conflicts = value; }
        }

        public bool ShouldStop
        {
            get { return shouldStop; }
            set { shouldStop = value; }
        }

        public MinConflictAlgorithm(int numberOfQueens)
        {
            this.numberOfQueens = numberOfQueens;
            this.board = new int[numberOfQueens];
            this.conflicts = new bool[numberOfQueens];

            InitBoard();
        }

        private void InitBoard()
        {
            Random rnd = new Random();
            for (int i = 0; i < this.numberOfQueens; i++)
            {
                // row = i 
                // column = rnd.Next(0, size)
                board[i] = rnd.Next(0, this.numberOfQueens);
            }
        }

        public int AllConflicts()
        {
            int result = 0;
            conflicts = new bool[this.numberOfQueens];
            for (int i = 0; i < this.numberOfQueens; i++)
            {
                result += AQueenConflicts(this.board, i);
            }
            return result;
        }

        private int AQueenConflicts(int[] board, int queenRow)
        {
            int result = 0;

            for (int i = 0; i < this.numberOfQueens; i++)
            {
                if (queenRow != i && HasConflict(board, queenRow, i))
                {
                    conflicts[i] = true;
                    result++;
                }
            }

            return result;
        }

        private bool HasConflict(int[] board, int rowOfFirstQueen, int rowOfSecondConflict)
        {

            if (board[rowOfFirstQueen] - board[rowOfSecondConflict] == rowOfSecondConflict - rowOfFirstQueen)
            {
                return true;
            }
            else if (board[rowOfSecondConflict] - board[rowOfFirstQueen] == rowOfSecondConflict - rowOfFirstQueen)
            {
                return true;
            }
            else if (board[rowOfFirstQueen] == board[rowOfSecondConflict])
            {
                return true;
            }

            return false;

        }

        public int[] Solve()
        {
            this.shouldStop = false;

            List<KeyValuePair<int, int>> queensConfilicts = new List<KeyValuePair<int, int>>();

            int numberOfConfilicts = this.AllConflicts();

            // if old board == new board means that The board reached min conflicts
            int oldBoardHash = this.GetBoardHashCode();
            int newBoardHash = int.MinValue;

            while (numberOfConfilicts != 0 && !shouldStop && oldBoardHash != newBoardHash)
            {
                oldBoardHash = newBoardHash;

                queensConfilicts.Clear();
                for (int i = 0; i < this.numberOfQueens; i++)
                {
                    queensConfilicts.Add(new KeyValuePair<int, int>(i, AQueenConflicts(this.board, i)));
                }


                var sortedByConfilicts = queensConfilicts.OrderByDescending(x => x.Value);

                foreach (var item in sortedByConfilicts)
                {
                    if (item.Value == 0)
                    {
                        continue;
                    }

                    this.MoveQueenToFindMinConflictPosition(item.Key);
                    numberOfConfilicts = this.AllConflicts();

                    if (numberOfConfilicts == 0)
                    {
                        break;
                    }
                }
                newBoardHash = this.GetBoardHashCode();

            }

            this.finishFire();
            return board;
        }

        private void MoveQueenToFindMinConflictPosition(int row)
        {
            int currentConflicts = AQueenConflicts(board, row);
            int minConflicts = currentConflicts;

            if (minConflicts == 0)
            {
                return;
            }

            int[] tempBoard = new int[this.numberOfQueens];
            board.CopyTo(tempBoard, 0);


            for (int i = 0; i < this.numberOfQueens; i++)
            {
                tempBoard[row] = i;
                minConflicts = AQueenConflicts(tempBoard, row);

                if (minConflicts < currentConflicts)
                {

                    currentConflicts = minConflicts;

                    tempBoard.CopyTo(board, 0);

                    if (minConflicts == 0)
                    {
                        return;
                    }
                }
            }

        }

        private int GetBoardHashCode()
        {
            var board = this.board;

            string temp = string.Empty;

            for (int i = 0; i < board.Length; i++)
            {
                temp = i.ToString() + board[i].ToString();
            }

            return temp.GetHashCode();
        }

        private void finishFire()
        {
            if (Finish!=null)
            {
                Finish(this, EventArgs.Empty);
            }
        }
    }
}
