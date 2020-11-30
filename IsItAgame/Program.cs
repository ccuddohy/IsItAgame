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
		static readonly ConsoleColor OriginalBackgroundColor = Console.BackgroundColor;
		static readonly ConsoleColor OriginalForegroundColor = Console.ForegroundColor;
		static readonly int StartingCompassLeft = (Width / 2) + 5;
		static readonly int StartingCompassTop = 5;
		static readonly int TuringTextLeft = StartingCompassLeft + 1;
		static readonly int StartingTraficLightLeft = 5;
		static readonly int TurningTestTop = StartingCompassTop-1; 
		static readonly int StartingTraficLightTop = 1;
		static readonly int DistanceDisplayStartLeft = Width / 3;
		static readonly int DistanceDisplayStartTop = 7;

		enum TrafficLightState
		{ 
			GREEN = 0,
			YELLOW = 1, 
			RED = 2, 
			LEFTTURNGREEN = 3
		}

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
		"  E       S  " + "\r\n" +
		"             " + "\r\n" +
		"             " + "\r\n" +
		"  N       W  " + "\r\n" +
		"             " + "\r\n";
		static readonly string South = "" +
		"      S      " + "\r\n" +
		"             " + "\r\n" +
		"E           W" + "\r\n" +
		"             " + "\r\n" +
		"      N      " + "\r\n";
		static readonly string SouthWest = "" +
		"             " + "\r\n" +
		"  S       W  " + "\r\n" +
		"             " + "\r\n" +
		"             " + "\r\n" +
		"  E       N  " + "\r\n" +
		"             " + "\r\n";
		static readonly string West = "" +
		"      W      " + "\r\n" +
		"             " + "\r\n" +
		"S           N" + "\r\n" +
		"             " + "\r\n" +
		"      E      " + "\r\n";
		static readonly string NorthWest = "" +
		"             " + "\r\n" +
		"  W       N  " + "\r\n" +
		"             " + "\r\n" +
		"             " + "\r\n" +
		"  S       E  " + "\r\n" +
		"             " + "\r\n";
		#endregion

		/// <summary>
		/// The strings below are used to form the traffic light
		/// </summary>
		#region traffic light strings
		static readonly string TrafficLightFrame =
			" _____________" + "\r\n" +
			"|             |" + "\r\n" +
			"|             |" + "\r\n" +
			"|             |" + "\r\n" +
			"|             |" + "\r\n" +
			"|             |" + "\r\n" +
			"|             |" + "\r\n" +
			"|             |" + "\r\n" +
			"|             |" + "\r\n" +
			"|             |" + "\r\n" +
			"|             |" + "\r\n" +
			"|             |" + "\r\n" +
			"|             |" + "\r\n" +
			"|_____________|";

		static readonly string ActiveLight =
		"        |" + "\r\n" +
		"        |" + "\r\n" +
		"        |" + "\r\n";


		static readonly string NonActiveLight =
		" _______ " + "\r\n" +
		"|       |" + "\r\n" +
		"|_______|" + "\r\n" + 
		"         " + "\r\n";

		static readonly string LeftGreenLight =
		" _______ " + "\r\n" +
		"| /____ |" + "\r\n" +
	   @"|_\_____|" + "\r\n";
		#endregion

		static readonly string DistanceDisplayFrame =
			" _________" + "\r\n" +
			"|         |" + "\r\n" +
			"|         |" + "\r\n" +
			"|_________|" + "\r\n";


		/// <summary>
		/// draws the traffic light in the console 
		/// </summary>
		/// <param name="tls"></param>
		static void RenderTraficLight(TrafficLightState tls)
		{
			Console.SetCursorPosition(StartingTraficLightLeft + 3, StartingTraficLightTop + 2);
			int x = Console.CursorLeft;
			int y = Console.CursorTop;
			for (int bulb = 1; bulb <= 3; bulb++)
			{
				string lightString = "";
				if(bulb == 1 && tls == TrafficLightState.RED)
				{
					Console.ForegroundColor = ConsoleColor.Red;
					Console.BackgroundColor = ConsoleColor.Red;
					lightString = ActiveLight;
				}
				if (bulb == 1 && tls != TrafficLightState.RED)
				{
					lightString = NonActiveLight;
				}
				if(bulb == 2 && tls == TrafficLightState.YELLOW)
				{
					Console.BackgroundColor = ConsoleColor.Yellow;
					lightString = ActiveLight;
				}
				else if(bulb == 2 && tls != TrafficLightState.YELLOW)
				{
					lightString = NonActiveLight;
				}
				else if (bulb == 3 && tls == TrafficLightState.LEFTTURNGREEN)
				{
					Console.ForegroundColor = ConsoleColor.Green;
					lightString = LeftGreenLight;
				}
				else if (bulb == 3 && tls == TrafficLightState.GREEN)
				{
					Console.BackgroundColor = ConsoleColor.Green;
					Console.ForegroundColor = ConsoleColor.Green;
					lightString = ActiveLight;
				}
				else if (bulb == 3 && tls != TrafficLightState.LEFTTURNGREEN && tls != TrafficLightState.GREEN)
				{
					lightString = NonActiveLight;
				}
				foreach (char c in lightString)
				{
					if (c is '\n')
					{
						Console.SetCursorPosition(x, ++y);
					}
					else if (Console.CursorLeft < Width - 1 && (!(c is ' ') || true))
					{
						Console.Write(c);
					}
					else if (Console.CursorLeft < Width - 1 && Console.CursorTop < Height - 1)
					{
						Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
					}
				}
				Console.BackgroundColor = OriginalBackgroundColor;
				Console.ForegroundColor = OriginalForegroundColor;
			}
		}


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
			Console.SetCursorPosition(StartingCompassLeft, StartingCompassTop);
			int x = Console.CursorLeft;
			int y = Console.CursorTop;
			foreach (char c in @str)
			{
				if (c is '\n')
				{
					Console.SetCursorPosition(x, ++y);
				}
				else if (Console.CursorLeft < Width - 1 /*&& (!(c is ' ') || true)*/)
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

			Console.SetCursorPosition(StartingTraficLightLeft, StartingTraficLightTop);
			Render(TrafficLightFrame, true);
			RenderTraficLight(TrafficLightState.GREEN);
			RenderCompass(CompassHeading[0]);//set compass to North heading to start

			Console.SetCursorPosition(DistanceDisplayStartLeft, DistanceDisplayStartTop);
			Render(DistanceDisplayFrame, true);

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
							Console.SetCursorPosition(TuringTextLeft, TurningTestTop);
							Console.BackgroundColor = ConsoleColor.Cyan;
							Console.ForegroundColor = ConsoleColor.Black;
							Console.Write("Turning Left");
							StopwatchCompass.Restart();
							Console.BackgroundColor = OriginalBackgroundColor;
							Console.ForegroundColor = OriginalForegroundColor;

							break;
						case ConsoleKey.RightArrow:
							if (turning != 0)//already turning 
							{
								break;
							}
							turning = 1;
							Console.SetCursorPosition(TuringTextLeft, TurningTestTop);
							Console.BackgroundColor = ConsoleColor.Cyan;
							Console.ForegroundColor = ConsoleColor.Black;
							Console.Write("Turning Right");
							StopwatchCompass.Restart();
							Console.BackgroundColor = OriginalBackgroundColor;
							Console.ForegroundColor = OriginalForegroundColor;
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
						//clear the turning text
						Console.SetCursorPosition(TuringTextLeft, TurningTestTop);
						Console.Write("             ");
						turnNeedsUpdate = false;
					}
					StopwatchCompass.Restart();
					
				}

				Thread.Sleep(ThreadSleepTimeSpan);
			}

			

		}
	}
}
