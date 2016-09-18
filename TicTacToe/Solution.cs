using System;

namespace TicTacToe
{
	public class Solution
	{
		internal bool winningMove = true;
		internal int x = -1;
		internal int y = -1;
		public Solution (int xToBuy,int yToBuy)
		{
			this.x = xToBuy;
			this.y = yToBuy;
		}
	}
}

