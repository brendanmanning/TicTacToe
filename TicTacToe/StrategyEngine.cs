using System;

namespace TicTacToe
{
	public class StrategyEngine
	{

		internal int[,] board = new int[3,3];

		internal Strategy[] strats = new Strategy[2];
		internal Strategy currentStrat;
		private Random r = new Random ();
		public StrategyEngine ()
		{
			setupStrategies ();
		}

		public Solution makeMove()
		{
			/* The solution class represents the tile that needs to be filled 
			 * for the opponent to win
			 * 
			 * When the AI finds out that it needs to play defensively, it should play the 
			 * solution it is given because that will block the player's win
			 * 
			 * Conversely, when the AI is about to win, it should play the solution to fill
			 * the block that will make it win
			 * 
			 * If the winningMove variable of a Solution is false, that means that no winning moves
			 * were found, so always check before using a solution. It will crash otherwise */

			// See if AI is one move away from wining
			Solution s = needsToPlayDefensive(1);
			if (s.winningMove) {
				return s;
			}

			// See if the player is one move away from winning
			Solution ds = needsToPlayDefensive(0);
			if (ds.winningMove) {
				return ds;
			}

			// See if we have a strategy chosen yet
			if(currentStrat == null)
			{
				currentStrat = strats [r.Next (0, strats.Length)];
			}

			// Make moves for the current strategy if they exist
			Solution sol = makeMoveForStrategy();
			if(sol != null)
			{
				return sol;
			}

			// If the above didn't work, try switching strategies
			for(int i = 0; i < strats.Length; i++)
			{
				if(!strats[i].title.Equals(currentStrat.title))
				{
					currentStrat = strats[i];
				}
			}

			// Make a move for our new strategy
			Solution newsolution = makeMoveForStrategy ();
			if(newsolution != null)
			{
				return newsolution;
			}

			// If all the above fail, just pick a random spot
			int spotsAvailable = 0;
			for(int x = 0; x < 3; x++)
			{
				for(int y = 0; y < 3; y++)
				{
					if(board[x,y] == -1)
					{
						spotsAvailable++;
					}
				}
			}
			string[] availableSpots = new string[spotsAvailable];
			int loopIndex = 0;
			for(int x = 0; x < 3; x++)
			{
				for(int y = 0; y < 3; y++)
				{
					if(board[x,y] == -1)
					{
						availableSpots[loopIndex] = "(" + x + "," + y + ")";
						loopIndex++;
					}
				}
			}
			int index = r.Next (0, availableSpots.Length);
			int spotX = -1;
			int spotY = -1;
			try {
				spotX = Int32.Parse(availableSpots [index].Replace (")", "").Replace("(","").Split(",".ToCharArray()[0]) [0]);
				spotY = Int32.Parse(availableSpots [index].Replace (")", "").Replace("(","").Split(",".ToCharArray()[0]) [1]);
			} catch (Exception e) {
				Console.WriteLine ("Error");
			}
			return new Solution (spotX, spotY);
		}

		private Solution makeMoveForStrategy()
		{
			// See if we've got any moves to make in for our strategy
			for(int i = 0; i < currentStrat.combinations.Length; i++)
			{
				Solution move = currentStrat.combinations[i].makeMovesIfExist(board);
				if(move != null)
				{
					return move;
				}
			}

			return null;
		}

		// Pass 1 to see if player is about to win and AI Must be defensive
		// Pass 0 to see if AI is about to win and player should be defensive
		public Solution needsToPlayDefensive(int player)
		{
			int p = player;
			for (int x = 0; x < 3; x++) {
				for (int y = 0; y < 3; y++) {
					if (board [x, y] == 0) {

						// Check for horizontal

						// If this tile has one to the left
						//..and can win to the right
						//...This requires being in a middle tile
						if (x == 1) {
							if (board [x - 1, y] == p) {
								if (board [x + 1, y] == -1) {
									Console.WriteLine ("C1");
									return new Solution(x+1,y);
								}
							}
						}

						// If this tile has one to the right
						//..and can win to the left
						//...This requires being in a middle tile
						if (x == 1) {
							if ((board [x + 1, y] == p) && (board [x - 1, y] == -1)) {
								Console.WriteLine ("C2");
								return new Solution(x-1,y);
							}
						}

						// If this tile has one to the left and can win to the left
						//..This will only work on the right side of the board
						if (x == 2) {
							if ((board [x - 1, y] == p) && (board [x - 2, y] == -1)) {
								Console.WriteLine ("C3");
								return new Solution(x-2,y);
							}
						}

						// If this tile has one to the right and can win to the right
						//..This only works on the left side of the board
						if (x == 0) {
							if ((board [x + 1, y] == p) && (board [x + 2, y] == -1)) {
								Console.WriteLine ("C4");
								return new Solution(x+2,y);
							}
						}

						// Check if there's one on each side (i.e X _ X)
						//..We should only check this when dealing with a 
						//...Tile on the left side. We loop through all the tiles
						//....So we'll get to it eventually
						if (x == 0) {
							if ((board [x + 2, y] == p) && (board [x + 1, y] == -1)) {
								return new Solution(x+1,y);
							}
						}

						// Check for winning vertically

						// If this tile has one above and can win below
						//..This requires being in a middle (of y)
						if (y == 1) {
							if ((board [x, y - 1] == p) && (board [x, y + 1] == -1)) {
								Console.WriteLine ("C5");
								return new Solution(x,y+1);
							}
						}

						// If the tile has one below and can win above
						//..This requires being the a middle tile (y=1)
						if (y == 1) {
							if ((board [x, y + 1] == p) && (board [x, y - 1] == -1)) {
								return new Solution (x, y - 1);
							}
						}

						// If the tile has one below and can win below
						//..Requires being at the top
						if (y == 0) {
							if ((board [x, y + 1] == p) && (board [x, y + 2] == -1)) {
								return new Solution(x,y+2);
							}
						}

						// If the tile has one above and can win above
						//..Requires being on the bottom
						if (y == 2) {
							if ((board [x, y - 1] == p) && (board [x, y - 2] == -1)) {
								return new Solution(x,y-2);
							}
						}

						// Check for one on each size
						//..Check only when y=0, because
						//...We loop through all the tiles so if we don't get to it now we will later
						if (y == 0) {
							if ((board [x, y + 2] == p) && (board [x, y + 1] == -1)) {
								return new Solution(x,y+1);
							}
						}

						// Check for winning diagonally
						// Top left corner
						if ((x == 0) && (y == 0)) {
							if ((board [x + 1, y + 1] == p) && (board [x + 2,y + 2] == -1)) {
								return new Solution(x+2,y+2);
							}
						}
						// Top right corner
						if ((x == 2) && (y == 0)) {
							if ((board [x - 1, y + 1] == p) && (board [x - 2, y + 2] == -1)) {
								return new Solution(x-2,y+2);
							}
						}
						// Bottom right corner
						if ((x == 2) && (y == 2)) {
							if ((board [x - 1, y - 1] == p) && (board [x - 2, y - 2] == -1)) {
								return new Solution(x-2,y-2);
							}
						}
						// Bottom left corner
						if ((x == 0) && (y == 2)) {
							if ((board [x + 1, y - 1] == p) && (board [x + 2, y - 2] == -1)) {
								return new Solution(x+2,y-2);
							}
						}

						// Pairs of corners
						// Top left && bottom right
						if ((x == 0) && (y == 0)) {
							if ((board [x + 2, y + 2] == p) && (board [x + 1, y + 1] == -1)) {
								return new Solution(x+1,y+1);
							}
						}
						// Top right && bottom left
						if ((x == 2) && (y == 0)) {
							if ((board [x - 2, y + 2] == p) && (board [x - 1, y + 1] == -1)) {
								return new Solution(x-1,y+1);
							}
						}
					}
				}
			}
			Console.WriteLine ("C0");

			Solution noSolution = new Solution(-1,-1);
			noSolution.winningMove = false;
			return noSolution;
		}

		private void setupStrategies()
		{
			/* 
			 * A strategy is a gameplan consisting of a pattern to make
			 * For example a pattern like so
			 * X  _  _
			 * _  X  _
			 * X  _  _
			 */

			TileCombination top_left = new TileCombination (3);
			// Top left
			top_left.addTile (0, 0);
			top_left.addTile (2, 0);
			top_left.addTile (0, 2);
			// Top right
			TileCombination top_right = new TileCombination (3);
			top_right.addTile (0, 0);
			top_right.addTile (2, 0);
			top_right.addTile (2, 2);
			// Bottom right
			TileCombination bottom_right = new TileCombination (3);
			bottom_right.addTile (2, 0);
			bottom_right.addTile (2, 2);
			bottom_right.addTile (0, 2);
			// Bottom Left
			TileCombination bottom_left = new TileCombination (3);
			bottom_left.addTile (0, 0);
			bottom_left.addTile (0, 2);
			bottom_left.addTile (2, 2);
			Strategy corners = new Strategy (new TileCombination[4]{ top_left, top_right, bottom_right, bottom_left });
			corners.title = "Corners";

			TileCombination arrow_pointing_right = new TileCombination (3);
			arrow_pointing_right.addTile (0, 0);
			arrow_pointing_right.addTile (1, 1);
			arrow_pointing_right.addTile (0, 2);
			TileCombination arrow_pointing_left = new TileCombination (3);
			arrow_pointing_left.addTile (2, 0);
			arrow_pointing_left.addTile (1, 1);
			arrow_pointing_left.addTile (2, 2);

			Strategy arrow = new Strategy (new TileCombination[2]{ arrow_pointing_right, arrow_pointing_left });
			arrow.title = "Arrow";

			strats [0] = corners;
			strats [1] = arrow;
		}
	}
}