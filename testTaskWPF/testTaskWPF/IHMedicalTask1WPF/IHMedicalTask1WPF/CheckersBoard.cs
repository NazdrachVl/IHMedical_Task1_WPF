using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IHMedicalTask1WPF
{
	class CheckersBoard
	{
		private Cell[,] _map;
		private List<CellPosition> _attackCheckers;

		private static Random rand;

		public CellPosition CellPosition1;

		public CellType Turn = CellType.White;

		#region Properties

		public Cell this[int row, int column]
		{
			get { return _map[row, column]; }
		}

		public static int BoardSize
		{
			get { return 8; }
		}

		#endregion

		public CheckersBoard()
		{
			_map = new Cell[BoardSize, BoardSize];
			New();
		}

		public CheckersBoard(Cell[,] map)
		{
			_map = new Cell[BoardSize, BoardSize];
			Array.Copy(map, 0, _map, 0, map.Length);
		}

		public static CheckersBoard Random()
		{
			Cell[,] map = new Cell[BoardSize, BoardSize];

			// init all fields with EMPTY
			for (int i = 0; i < CheckersBoard.BoardSize; i++)
			{
				for (int j = 0; j < CheckersBoard.BoardSize; j++)
				{
					map[i, j] = new Cell(CellType.Empty);
				}
			}
			// generate random count of checkers
			rand = new Random();
			int countWhite = rand.Next(3, 12);
			int countBlack = rand.Next(3, 12);
			// put black chess
			for (int i = 0; i < countBlack;)
			{
				int row = rand.Next(0, CheckersBoard.BoardSize);
				int column = rand.Next(0, CheckersBoard.BoardSize);
				if ((row + column) % 2 == 0 || map[row, column].Type != CellType.Empty)
				{
					continue;
				}
				else
				{
					map[row, column] = new Cell(CellType.Black, row == CheckersBoard.BoardSize - 1 || rand.NextDouble() < 0.1);
					i++;
				}
			}
			// put white chess
			for (int i = 0; i < countWhite;)
			{
				int row = rand.Next(0, CheckersBoard.BoardSize);
				int column = rand.Next(0, CheckersBoard.BoardSize);
				if ((row + column) % 2 == 0 || map[row, column].Type != CellType.Empty)
				{
					continue;
				}
				else
				{
					map[row, column] = new Cell(CellType.White, row == 0 || rand.NextDouble() < 0.1);
					i++;
				}
			}
			return new CheckersBoard(map);
		}

		public void New()
		{
			for (int i = 0; i < BoardSize; i++)
			{
				for (int j = 0; j < BoardSize; j++)
				{
					if (i < 3 && (i + j) % 2 == 1)
					{
						_map[i, j] = new Cell(CellType.Black);
					}
					else if (i > 4 && (i + j) % 2 == 1)
					{
						_map[i, j] = new Cell(CellType.White);
					}
					else
					{
						_map[i, j] = new Cell(CellType.Empty);
					}
				}
			}
		}

		public void Step(int x, int y)
		{
			if (CellPosition1 == null)
			{
				if (Turn == _map[x, y].Type)
				{
					CellPosition1 = new CellPosition(x, y);
				}
				return;
			}

			Cell chosenCell1 = _map[CellPosition1.X, CellPosition1.Y];
			Cell chosenCell2 = _map[x, y];
			// checker is move
			if (chosenCell2.Type == CellType.Empty)
			{
				//checker is queen
				if (chosenCell1.IsQueen)
				{
					if (Math.Abs(CellPosition1.X - x) == Math.Abs(CellPosition1.Y - y))
					{
						for (int i = 1, j = 1; i < Math.Abs(CellPosition1.X - x) && j < Math.Abs(CellPosition1.Y - y); i++, j++)
						{
							int tx = CellPosition1.X + (((CellPosition1.X - x) < 0) ? i : -i);
							int ty = CellPosition1.Y + (((CellPosition1.Y - y) < 0) ? j : -j);
							if (_map[tx, ty].Type != CellType.Empty)
							{
								return;
							}
						}
						var tmp = chosenCell2;
						_map[x, y] = chosenCell1;
						_map[CellPosition1.X, CellPosition1.Y] = tmp;
						// turn moves to oposite side
						if (chosenCell1.Type == CellType.White)
						{
							Turn = CellType.Black;
						}
						else
						{
							Turn = CellType.White;
						}
						CellPosition1 = null;
					}
				}
				else
				{
					//checker is standart
					if (chosenCell1.Type == CellType.White && x + 1 == CellPosition1.X && (y + 1 == CellPosition1.Y || y - 1 == CellPosition1.Y) ||
						chosenCell1.Type == CellType.Black && x - 1 == CellPosition1.X && (y + 1 == CellPosition1.Y || y - 1 == CellPosition1.Y))
					{
						if ((x == 0 && chosenCell1.Type == CellType.White) || (x == 7 && chosenCell1.Type == CellType.Black))
						{
							chosenCell1.IsQueen = true;
						}
						var tmp = chosenCell2;
						_map[x, y] = chosenCell1;
						_map[CellPosition1.X, CellPosition1.Y] = tmp;
						// turn moves to oposite side
						if (chosenCell1.Type == CellType.White)
						{
							Turn = CellType.Black;
						}
						else
						{
							Turn = CellType.White;
						}
						CellPosition1 = null;
					}
				}
			}
		}

		public bool CheckAttackChekers()
		{
			_attackCheckers = new List<CellPosition>();
			for (int i = 0; i < BoardSize; i++)
			{
				for (int j = 0; j < BoardSize; j++)
				{
					//check attak for queen
					if (Turn == _map[i, j].Type)
					{
						if (_map[i, j].IsQueen)
						{
							//up-left diagonal
							if (i > 1 && j > 1) //if it probably can attack at this way
							{
								int k = i, t = j;
								try
								{
									while (_map[k, t].Type == CellType.Empty && k >= 0 && t >= 0)
									{
										k--;
										t--;
									}

									//we check it
									if (((_map[k - 1, t - 1].Type != Turn && _map[k - 1, t - 1].Type != CellType.Empty) && _map[k - 2, t - 2].Type == CellType.Empty))
									{
										_attackCheckers.Add(new CellPosition(i, j));
									}
								}
								catch { }
							}
							//up-right diagonal
							if (i > 1 && j < BoardSize - 2) //if it probably can attack at this way
							{
								int k = i, t = j;
								try
								{
									while (_map[k - 1, t + 1].Type == CellType.Empty && k >= 0 && t < BoardSize - 1)
									{
										k--;
										t++;
									}

									//we check it
									if (((_map[k - 1, t + 1].Type != Turn && _map[k - 1, t + 1].Type != CellType.Empty) && _map[k - 2, t + 2].Type == CellType.Empty))
									{
										_attackCheckers.Add(new CellPosition(i, j));
									}
								}
								catch { }
							}
							//down-left diagonal
							if (i < BoardSize - 2 && j > 1) //if it probably can attack at this way
							{
								int k = i, t = j;
								try
								{
									while (_map[k + 1, t - 1].Type == CellType.Empty && k < BoardSize - 1 && t >= 0)
									{
										k++;
										t--;
									}
									//we check it
									if (((_map[k + 1, t - 1].Type != Turn && _map[k + 1, t - 1].Type != CellType.Empty) && _map[k + 2, t - 2].Type == CellType.Empty))
									{
										_attackCheckers.Add(new CellPosition(i, j));
									}
								}
								catch { }
							}
							//down-right diagonal
							if (i < BoardSize - 2 && j < BoardSize - 2) //if it probably can attack at this way
							{
								int k = i, t = j;
								try
								{
									while (_map[k + 1, t + 1].Type == CellType.Empty && k < BoardSize - 1 && t < BoardSize - 1)
									{
										k++;
										t++;
									}
									//we check it
									if (Turn == _map[k, t].Type && ((_map[k + 1, t + 1].Type != Turn && _map[k + 1, t + 1].Type != CellType.Empty) && _map[k + 2, t + 2].Type == CellType.Empty))
									{
										_attackCheckers.Add(new CellPosition(i, j));
									}
								}
								catch { }
							}
						}
						else
						{
							//checkers
							//up-left diagonal
							if (i > 1 && j > 1) //if it probably can attack at this way
							{
								//we check it
								if (Turn == _map[i, j].Type && ((_map[i - 1, j - 1].Type != Turn && _map[i - 1, j - 1].Type != CellType.Empty) && _map[i - 2, j - 2].Type == CellType.Empty))
								{
									_attackCheckers.Add(new CellPosition(i, j));
								}
							}
							//up-right diagonal
							if (i > 1 && j < BoardSize - 2) //if it probably can attack at this way
							{
								//we check it
								if (Turn == _map[i, j].Type && ((_map[i - 1, j + 1].Type != Turn && _map[i - 1, j + 1].Type != CellType.Empty) && _map[i - 2, j + 2].Type == CellType.Empty))
								{
									_attackCheckers.Add(new CellPosition(i, j));
								}
							}
							//down-left diagonal
							if (i < BoardSize - 2 && j > 1) //if it probably can attack at this way
							{
								//we check it
								if (Turn == _map[i, j].Type && ((_map[i + 1, j - 1].Type != Turn && _map[i + 1, j - 1].Type != CellType.Empty) && _map[i + 2, j - 2].Type == CellType.Empty))
								{
									_attackCheckers.Add(new CellPosition(i, j));
								}
							}
							//down-right diagonal
							if (i < BoardSize - 2 && j < BoardSize - 2) //if it probably can attack at this way
							{
								//we check it
								if (Turn == _map[i, j].Type && ((_map[i + 1, j + 1].Type != Turn && _map[i + 1, j + 1].Type != CellType.Empty) && _map[i + 2, j + 2].Type == CellType.Empty))
								{
									_attackCheckers.Add(new CellPosition(i, j));
								}
							}
						}

					}
				}
			}
			if (_attackCheckers.Count == 0)
			{
				return false;
			}
			return true;
		}

		public void Attack(int x, int y)
		{
			if (_attackCheckers.Exists(cell => cell.X == x && cell.Y == y))
			{
				CellPosition1 = new CellPosition(x, y);
				return;
			}
			if (CellPosition1 != null)
			{
				Cell chosenCell1 = _map[CellPosition1.X, CellPosition1.Y];
				Cell chosenCell2 = _map[x, y];
				var tx = CellPosition1.X - x;
				var ty = CellPosition1.Y - y;
				//up-left diagonal
				if (tx > 1 && ty > 1)
				{
					if (_map[x, y].IsQueen)
					{
						int k = x, t = y;
						while (_map[k, t].Type == CellType.Empty && k >= 0 && t >= 0)
						{
							k--;
							t--;
						}
					}
					if (chosenCell2.Type == CellType.Empty && (_map[x, y].Type != Turn && _map[x, y].Type == CellType.Empty))
					{
						var tmp = chosenCell2;
						_map[x, y] = chosenCell1;
						_map[CellPosition1.X, CellPosition1.Y] = tmp;
						_map[x + 1, y + 1] = tmp;
						this.CheckAttackChekers();
						if (!_attackCheckers.Exists(cell => cell.X == x && cell.Y == y))
						{
							if (chosenCell1.Type == CellType.White)
							{
								Turn = CellType.Black;
							}
							else
							{
								Turn = CellType.White;
							}
						}
						CellPosition1 = null;
						CheckerToQueen(x, y);
						return;
					}
				}
				//up-right diagonal
				if (tx > 1 && ty < -1)
				{
					if (_map[x, y].IsQueen)
					{
						int k = x, t = y;
						while (_map[k + 1, t - 1].Type == CellType.Empty && k < BoardSize - 1 && t >= 0)
						{
							k++;
							t--;
						}
					}
					if (_map[x, y].IsQueen)
					{
						int k = x, t = y;
						while (_map[k - 1, t + 1].Type == CellType.Empty && k >= 0 && t < BoardSize - 1)
						{
							k--;
							t++;
						}
					}
					if (chosenCell2.Type == CellType.Empty && (_map[x, y].Type != Turn && _map[x, y].Type == CellType.Empty))
					{
						var tmp = chosenCell2;
						_map[x, y] = chosenCell1;
						_map[CellPosition1.X, CellPosition1.Y] = tmp;
						_map[x + 1, y - 1] = tmp;
						this.CheckAttackChekers();
						if (!_attackCheckers.Exists(cell => cell.X == x && cell.Y == y))
						{
							if (chosenCell1.Type == CellType.White)
							{
								Turn = CellType.Black;
							}
							else
							{
								Turn = CellType.White;
							}
						}
						CellPosition1 = null;
						CheckerToQueen(x, y);
						return;
					}
				}
				//down-left diagonal
				if (tx < -1 && ty > 1)
				{
					if (chosenCell2.Type == CellType.Empty && (_map[x, y].Type != Turn && _map[x, y].Type == CellType.Empty))
					{
						var tmp = chosenCell2;
						_map[x, y] = chosenCell1;
						_map[CellPosition1.X, CellPosition1.Y] = tmp;
						_map[x - 1, y + 1] = tmp;
						this.CheckAttackChekers();
						if (!_attackCheckers.Exists(cell => cell.X == x && cell.Y == y))
						{
							if (chosenCell1.Type == CellType.White)
							{
								Turn = CellType.Black;
							}
							else
							{
								Turn = CellType.White;
							}
						}
						CellPosition1 = null;
						CheckerToQueen(x, y);
						return;
					}
				}
				//down-right diagonal
				if (tx < -1 && ty < -1)
				{
					if (_map[x, y].IsQueen)
					{
						int k = x, t = y;
						while (_map[k + 1, t + 1].Type == CellType.Empty && k < BoardSize - 1 && t < BoardSize - 1)
						{
							k++;
							t++;
						}
					}
					if (chosenCell2.Type == CellType.Empty && (_map[x, y].Type != Turn && _map[x, y].Type == CellType.Empty))
					{
						var tmp = chosenCell2;
						_map[x, y] = chosenCell1;
						_map[CellPosition1.X, CellPosition1.Y] = tmp;
						_map[x - 1, y - 1] = tmp;
						this.CheckAttackChekers();
						if (!_attackCheckers.Exists(cell => cell.X == x && cell.Y == y))
						{
							if (chosenCell1.Type == CellType.White)
							{
								Turn = CellType.Black;
							}
							else
							{
								Turn = CellType.White;
							}
						}
						CellPosition1 = null;
						CheckerToQueen(x, y);
						return;
					}
				}


			}
		}

		public void CheckerToQueen(int x, int y)
		{
			if (_map[x, y].Type == CellType.White && x == 0 || _map[x, y].Type == CellType.Black && x == BoardSize - 1)
			{
				_map[x, y].IsQueen = true;
			}
		}

	}
}