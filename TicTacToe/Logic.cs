using System;

namespace TicTacToe
{
	public class Logic
	{
		private string playerString;
		private string aiString;
		private int[,] board;

		public Logic (string playerChar, string aiChar) {}

		public void upateBoard(int[,] b)
		{
			board = b;
		}

		public bool didPlayerWin()
		{
			return didPlayerNumberWin (board, 0);
		}

		public bool didAIWin()
		{
			return didPlayerNumberWin(board, 1);
		}

		public bool didPlayerAndAITie()
		{
			for(int x = 0; x < 3; x++)
			{
				for(int y = 0; y < 3; y++)
				{
					if(board[x,y] == -1)
					{
						return false;
					}
				}
			}

			return true;
		}

		private bool didPlayerNumberWin(int[,] board, int num)
		{
			// Check for across
			for (int i = 0; i < 3; i++) {
				if ((board [0,i] == num) && (board [1,i] == num) && (board [2,i] == num)) {
					return true;
				}
			}

			// Check for vertically
			for (int i = 0; i < 3; i++) {
				if ((board [i,0] == num) && (board [i,1] == num) && (board [i,2] == num)) {
					return true;
				}
			}

			// Check for diagonally
			if ((board [0, 0] == num) && (board [1, 1] == num) && (board [2, 2] == num)) {
				return true;
			} else if ((board [0, 2] == num) && (board [1, 1] == num) && (board [2, 0] == num)) {
				return true;
			}

			// Otherwise, a victory hasn't been achieved yet
			return false;
		}
	}
}

