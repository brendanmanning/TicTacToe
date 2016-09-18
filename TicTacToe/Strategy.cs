using System;

namespace TicTacToe
{
	public class Strategy
	{

		internal string title = "";
		internal TileCombination[] combinations;

		public Strategy (TileCombination[] validCombinations)
		{
			combinations = validCombinations;
		}
	}
}