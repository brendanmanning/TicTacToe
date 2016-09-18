using System;

namespace TicTacToe
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			bool started = false;
			new Menu();

			Board b = new Board();
			Logic gameLogic = new Logic ("X","O");
			StrategyEngine engine = new StrategyEngine ();

			int scrollX = 0;
			int scrollY = 0;
			bool didPlayLastTile = false;

			while (true) {
				ConsoleKeyInfo keyInfo = Console.ReadKey ();

			
				if (keyInfo.Key == ConsoleKey.Spacebar) {
					if (started) {
						//if (b.openSquares () != 1) {
							if (scrollX != -1 && scrollY != -1) {
								int[] scrolledToLocation = b.right (scrollX, scrollY);
								scrollX	= scrolledToLocation [0];
								scrollY = scrolledToLocation [1];
							}
						//} else {

						//}
					}
				} else if (keyInfo.Key == ConsoleKey.Enter) {
					if (started) {
						if (scrollX != -1 && scrollY != -1) {
							doPlay (engine, b, scrollX, scrollY);
						} else if (scrollX == -1 && scrollY == -1 && didPlayLastTile == false) {
							doPlay (engine, b, scrollX, scrollY);
							didPlayLastTile = true;
						}

						// Check if the the player won
						gameLogic.upateBoard (b.getBoard ());
						if (b.openSquares () == 1) {
							doPlay (engine, b, b.lastX (), b.lastY ());
							Console.WriteLine ("The last square was filled in automatically");
						}
						gameLogic.upateBoard (b.getBoard ());
						if (gameLogic.didPlayerWin ()) {
							Console.WriteLine (Environment.NewLine + "YOU WIN!!!");
							break;
						} else if (gameLogic.didAIWin ()) {
							Console.WriteLine (Environment.NewLine + "YOU LOSE!!!");
							break;
						} else if (gameLogic.didPlayerAndAITie ()) {
							Console.WriteLine (Environment.NewLine + "IT'S A TIE!!!");
							break;
						}
					}
				} else if (keyInfo.Key == ConsoleKey.S) {
					if (!started) {
						started = true;
						b.updateForBoard ();
					}
				}
			}
		}

		public static void doPlay(StrategyEngine e,Board brd,int x, int y)
		{
			if (brd.canPlay (x, y)) {
				brd.play (0, x, y);
				e.board = brd.getBoard ();
				Solution move = e.makeMove ();
				brd.play (1, move.x, move.y);
			} else {
				Console.Clear ();
				Console.WriteLine ("Press Space to select a square first");
			}
		}
	}
}
