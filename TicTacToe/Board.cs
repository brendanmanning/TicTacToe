using System;

namespace TicTacToe
{
	public class Board
	{
		private string board;
		private string currentboard;

		private int[,] tiles;

		private string playerChar = "X";
		private string AIChar = "O";

		private string helpText = "Use space to move the X. Then press Enter to submit";

		/* 
		 * 
		 * Player = 0;
		 * AI = 1;
		 * 
		 */

		public Board ()
		{
			board = generateTemplate ();

			tiles = new int[3,3];
			for (int index = 0; index < 3; index++) 
			{
				for (int yindex = 0; yindex < 3; yindex++) {
					tiles[index,yindex] = -1;
				}
			}
		}


		//MARK: Gameplay methods
		public void play(int player,int x, int y)
		{
			// If -1 is passed in because the previous scroll found just one tile left
			//.. recurse with the last tile value
			if (x == -1) {
				for (int lx = 0; lx < 3; lx++) {
					for (int ly = 0; ly < 3; ly++) {
						if (tiles [lx, ly] == -1) {
							play (player, lx, ly);
							return;
						}
					}
				}
			}

			if (x == -1) {
				return;
			}

			// Update the tiles array
			tiles[x,y] = player;

			// Update the UI
			updateForBoard();
		}

		//MARK: UI methods
		public void updateForBoard()
		{
			// Here we create a string with 9 carriage returns, to clear the previous board
			//.. and we tack on the board template, and modify it according to the current game state
			string newBoard = newLines(20) + helpText + Environment.NewLine + board;

			for(int x = 0; x < 3; x++)
			{
				for(int y = 0; y < 3; y++)
				{
					string stringAtPosition = " ";
					if(tiles[x,y] == 0)
					{
						stringAtPosition = playerChar;
					}
					if(tiles[x,y] == 1)
					{
						stringAtPosition = AIChar;
					}
					newBoard = newBoard.Replace("(" + x + "," + y + ")", stringAtPosition);
				}
			}

			currentboard = newBoard;
			Console.Write(newBoard);
		}
	
		//MARK: Selection methods
		public int[] right(int cx, int cy)
		{
			int opentiles = 0;
			for (int tile = 0; tile < 3; tile++) {
				for (int tiley = 0; tiley < 3; tiley++) {
					if (tiles [tile, tiley] == -1) {
						opentiles++;
					}
				}
			}

			string[] available = new string[opentiles];
			int tileIndex = 0;
			for (int x = 0; x < 3; x++) {
				for (int y = 0; y < 3; y++) {
					if (tiles [x, y] == -1) {
						available [tileIndex] = "(" + x + "," + y + ")";
						tileIndex++;
					}
				}
			}

			// Cursor is at last position
			if (available [available.Length - 1].Equals ("(" + cx + "," + cy + ")")) {
				return showCursorAt (Int32.Parse (available [0].Split (",".ToCharArray () [0]) [0].Substring (1)), Int32.Parse (available [0].Split (",".ToCharArray () [0]) [1].Remove (1)));
			} else if(available.Length == 1) {
				showCursorAt (getX (available, 0), getY (available, 0));
				return new int[2]{ -1, -1 };
			} else {
				int indexOfCursor = -1;
				for(int indx = 0; indx < available.Length; indx++)
				{
					if (available [indx].Equals ("(" + cx + "," + cy + ")")) {
						indexOfCursor = indx;
					}
				}

				if (indexOfCursor != -1) {
					return showCursorAt (Int32.Parse (available [indexOfCursor + 1].Split (",".ToCharArray () [0]) [0].Substring (1)), Int32.Parse (available [indexOfCursor + 1].Split (",".ToCharArray () [0]) [1].Remove (1)));
				} else {
					return right (Int32.Parse (available [indexOfCursor + 2].Split (",".ToCharArray () [0]) [0].Substring (1)),
						Int32.Parse (available [indexOfCursor + 2].Split (",".ToCharArray () [0]) [1].Remove (1)));
					//return new int[2] {
						//Int32.Parse (available [indexOfCursor + 2].Split (",".ToCharArray () [0]) [0].Substring (1)),
						//Int32.Parse (available [indexOfCursor + 2].Split (",".ToCharArray () [0]) [1].Remove (1))
					//};
				}
			}
		}

		public int[] showCursorAt(int cursorx, int cursory)
		{
			string temporaryBoard = newLines (20) + helpText + Environment.NewLine + board;
			temporaryBoard = temporaryBoard.Replace ("(" + cursorx + "," + cursory + ")", "+");
			for(int x = 0; x < 3; x++)
			{
				for(int y = 0; y < 3; y++)
				{
					string stringAtPlace = " ";
					if(tiles[x,y] == 0)
					{
						stringAtPlace = playerChar;
					} 
					if(tiles[x,y] == 1)					
					{
						stringAtPlace = AIChar;
					}

					temporaryBoard = temporaryBoard.Replace ("(" + x + "," + y + ")", stringAtPlace);
				}
			}
			Console.Write (temporaryBoard);

			int[] toreturn =  new int[2];
			toreturn [0] = cursorx;
			toreturn [1] = cursory;
			return toreturn;
		}

		//MARK: Utility
		public bool canPlay(int atX, int atY)
		{
			return (tiles[atX,atY] == -1);
		}

		public string newLines(int count)
		{
			string s = "";
			for (int c = 0; c < count; c++) {
				s += Environment.NewLine;
			}
			return s;
		}

		private int getX(string[] inString, int atIndex)
		{
			return Int32.Parse (inString [atIndex].Split (",".ToCharArray () [0]) [0].Substring (1));			
		}

		private int getY(string[] inString, int atIndex)
		{
			return Int32.Parse (inString [atIndex].Split (",".ToCharArray () [0]) [1].Remove (1));			
		}

		private string generateTemplate()
		{
			// Wow this is ugly
			string t = "";
			t+= "    :     :    " + Environment.NewLine;
			t+=" (0,0)  :  (1,0)  :  (2,0) " + Environment.NewLine;
			t+="....:.....:...." + Environment.NewLine;
			t+="    :     :    " + Environment.NewLine;
			t+=" (0,1)  :  (1,1)  :  (2,1) " + Environment.NewLine;
			t+="....:.....:...." + Environment.NewLine;
			t+="    :     :    " + Environment.NewLine;
			t+=" (0,2)  :  (1,2)  :  (2,2) " + Environment.NewLine;
			t+="    :     :    ";

			return t;
		}

		public int[,] getBoard()
		{
			return tiles;
		}
	}
}

