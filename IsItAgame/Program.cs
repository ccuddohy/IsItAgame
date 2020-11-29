using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace IsItAgame
{
	class Program
	{
		static readonly Stopwatch StopwatchCompass = new Stopwatch();
		static readonly TimeSpan StopWatchTimeSpan = TimeSpan.FromSeconds(1);
		static readonly TimeSpan ThreadSleepTimeSpan = TimeSpan.FromMilliseconds(10);
		static readonly int Height = Console.WindowHeight;
		static readonly int Width = Console.WindowWidth;
		static readonly Dictionary<int, string> CompassHeading = new Dictionary<int, string>();

		/// <summary>
		/// The strings below make up the characters that are used to show the compass heading
		/// </summary>
		#region compass heading strings
		static readonly string North = "" +
		"      N      " + "\r\n" +
		"             " + "\r\n" +
		"W           E" + "\r\n" +
		"             " + "\r\n" +
		"      S      " + "\r\n";

		static readonly string NorthEast = "" +
		"             " + "\r\n" +
		"  N      E   " + "\r\n" +
		"             " + "\r\n" +
		"             " + "\r\n" +
		"  W      S   " + "\r\n";

		static readonly string East = "" +
		"      E      " + "\r\n" +
		"             " + "\r\n" +
		"N           S" + "\r\n" +
		"             " + "\r\n" +
		"      W      " + "\r\n";
		static readonly string SouthEast = "" +
		"             " + "\r\n" +
		" E       S   " + "\r\n" +
		"             " + "\r\n" +
		"             " + "\r\n" +
		" N       W   " + "\r\n" +
		"             " + "\r\n";
		static readonly string South = "" +
		"      S      " + "\r\n" +
		"             " + "\r\n" +
		"E           W" + "\r\n" +
		"             " + "\r\n" +
		"      N      " + "\r\n";
		static readonly string SouthWest = "" +
		"             " + "\r\n" +
		" S       W   " + "\r\n" +
		"             " + "\r\n" +
		"             " + "\r\n" +
		" E       N   " + "\r\n" +
		"             " + "\r\n";
		static readonly string West = "" +
		"      W      " + "\r\n" +
		"             " + "\r\n" +
		"S           N" + "\r\n" +
		"             " + "\r\n" +
		"      E      " + "\r\n";
		static readonly string NorthWest = "" +
		"             " + "\r\n" +
		" W       N  " + "\r\n" +
		"             " + "\r\n" +
		"             " + "\r\n" +
		" S       E   " + "\r\n" +
		"             " + "\r\n";
		#endregion

		/// <summary>
		/// This is used to hold the current index of the CompassHeading dictionary which 
		/// determines the compass heading.
		/// It is set to 0, indicating north to start
		/// </summary>
		static int CompassHeadingIndex = 0;

		/// <summary>
		/// Writes the compass heading to the console window. This function is a slight modification
		/// from git hub console game examples page: 
		/// </summary>
		/// <param name="str"></param>
		/// <param name="displayTurning"></param>
		static void RenderCompass(string str)
		{
			Console.SetCursorPosition((Width / 4), 2);
			int x = Console.CursorLeft;
			int y = Console.CursorTop;
			foreach (char c in @str)
			{
				if (c is '\n')
				{
					Console.SetCursorPosition(x, ++y);
				}
				else if (Console.CursorLeft < Width - 1 && (!(c is ' ') || true))
				{
					if (c.Equals('N'))
					{
						Console.ForegroundColor = ConsoleColor.Red;
					}
					else
					{
						Console.ForegroundColor = ConsoleColor.White;
					}
					Console.Write(c);
				}
				else if (Console.CursorLeft < Width - 1 && Console.CursorTop < Height - 1)
				{
					Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
				}
			}
		}

		static void Render(string @string, bool renderSpace = false)
		{
			int x = Console.CursorLeft;
			int y = Console.CursorTop;
			foreach (char c in @string)
				if (c is '\n')
					Console.SetCursorPosition(x, ++y);
				else if (Console.CursorLeft < Width - 1 && (!(c is ' ') || renderSpace))
					Console.Write(c);
				else if (Console.CursorLeft < Width - 1 && Console.CursorTop < Height - 1)
					Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
		}

		

		static void RotateCompassForTurn(int step)
		{
			if (CompassHeadingIndex + step > 7 ) 
			{
				CompassHeadingIndex = 0;
			}
			else if(CompassHeadingIndex + step < 0)
			{
				CompassHeadingIndex = 7;
			}
			else
			{
				CompassHeadingIndex = CompassHeadingIndex + step;
			}
			RenderCompass(CompassHeading[CompassHeadingIndex]);
		}

		static void Main(string[] args)
		{
			# region  populate the compass heading stringing into the Dictionary
			CompassHeading.Add(0, North);
			CompassHeading.Add(1, NorthEast);
			CompassHeading.Add(2, East);
			CompassHeading.Add(3, SouthEast);
			CompassHeading.Add(4, South);
			CompassHeading.Add(5, SouthWest);
			CompassHeading.Add(6, West);
			CompassHeading.Add(7, NorthWest);
			#endregion

			RenderCompass(CompassHeading[0]);//set compass to North heading to start
			int turning = 0;
			bool turnNeedsUpdate = false;
			while (true)
			{
				#region Window Resize is not allowed. The game will end
				if (Height != Console.WindowHeight || Width != Console.WindowWidth)
				{
					Console.Clear();
					Console.WriteLine("Sorry you are not allowed to resize the console window. Hit any key to exit");
					Console.ReadKey();
					return;
				}
				#endregion

				#region Key Available
				if (Console.KeyAvailable)
				{
					switch (Console.ReadKey(true).Key)
					{
						case ConsoleKey.Escape:
							Console.Clear();
							Console.WriteLine("Thank you for playing. Hit any key to exit");
							Console.ReadKey();
							return;
						case ConsoleKey.UpArrow:
							
							break;
						case ConsoleKey.LeftArrow:
							if(turning != 0)//already turning 
							{
								break;
							}
							turning = -1;
							Console.SetCursorPosition((Width / 4) + 3, 1);
							Console.Write("Turning Left");
							StopwatchCompass.Restart();
							break;
						case ConsoleKey.RightArrow:
							if (turning != 0)//already turning 
							{
								break;
							}
							turning = 1;
							Console.SetCursorPosition((Width / 4) + 3, 1);
							Console.Write("Turning Right");
							StopwatchCompass.Restart();
							break;
						case ConsoleKey.DownArrow:
							//stop if can
							break;
					}
				}
				#endregion

				//complete turn if necessary
				if (StopwatchCompass.Elapsed > StopWatchTimeSpan)
				{
					if(turning > 0)
					{
						RotateCompassForTurn(1);
						turning = turning + 1;
						if (turning > 2)
						{
							turning = 0;
							turnNeedsUpdate = true;
						}
					}
					else if(turning < 0)
					{
						RotateCompassForTurn(-1);
						turning = turning - 1;
						if (turning < -2)
						{
							turning = 0;
							turnNeedsUpdate = true;
						}
					}
					if(0 == turning && turnNeedsUpdate)
					{
						Console.SetCursorPosition((Width / 4) + 3, 1);
						Console.Write("                    ");
						turnNeedsUpdate = false;
					}
					StopwatchCompass.Restart();
					
				}

					Thread.Sleep(ThreadSleepTimeSpan);
			}

			

		}
	}
}
