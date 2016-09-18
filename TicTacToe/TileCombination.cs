using System;

namespace TicTacToe
{
	public class TileCombination
	{
		string[] tiles;
		public TileCombination (int tilescount)
		{
			tiles = new string[tilescount];
			for (int i = 0; i < tilescount; i++) {
				tiles [i] = null;
			}
		}
		public void addTile(int x, int y)
		{
			for (int i = 0; i < tiles.Length; i++) {
				if (tiles [i] == null) {
					tiles [i] = "(" + x + "," + y + ")";
					break;
				}
			}
		}
		public Solution makeMovesIfExist(int[,] withBoard)
		{
			for(int x = 0; x < 3; x++)
			{
				for(int y = 0; y < 3; y++)
				{
					if(withBoard[x,y] == -1)
					{
						for(int t = 0; t < tiles.Length; t++)
						{
							if(tiles[t].Equals("(" + x + "," + y + ")"))
							{
								return new Solution (x, y);
							}
						}
					}
				}
			}

			return null;
		}
	}
}