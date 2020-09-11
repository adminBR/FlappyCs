using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace FlappyBug
{
	class Pixel
	{
		public char Symbol;
		public int X = 0;
		public int Y = 0;
		public Pixel(char c, int x, int y)
		{
			Symbol = c;
			X = x;
			Y = y;
		}
	}
	class VectorInt
	{
		public int X;
		public int Y;
		public VectorInt(int x, int y)
		{
			X = x;
			Y = y;
		}
	}


internal class Program
	{
		public static int points = 0;
		public static bool gameOver = false;
		public static bool keypress = false;
		public static bool frameChange = false;

		public static List<Pixel> frameBuffer = new List<Pixel>();
		private static List<VectorInt> pipes = new List<VectorInt>();
		private static VectorInt bird = new VectorInt(14, 8);

		private static void Main(string[] args)
		{
			Console.SetWindowSize(49, 18);
			Console.SetBufferSize(49, 18);
			Task.Factory.StartNew(delegate
			{
				while (true)
				{
					if (Console.ReadKey().Key == ConsoleKey.UpArrow && !keypress)
					{
						keypress = true;
					}
				}
			});
			Console.SetCursorPosition(15, 8);
			Console.Write("[ Flappy Bug ]");
			Console.SetCursorPosition(4, 10);
			Console.Write("Pressione enter 2 vezes para continuar");
			Console.ReadKey();
			while (true)
			{
				loop();
				Console.SetCursorPosition(17, 8);
				Console.Write("[ GameOver ]");
				Console.SetCursorPosition(4, 10);
				Console.Write("Pressione enter 2 vezes para continuar");
				Console.ReadKey();
				points = 0;
				gameOver = false;
				keypress = false;
				frameChange = false;
				frameBuffer = new List<Pixel>();
				pipes = new List<VectorInt>();
				bird = new VectorInt(14, 8);
				Console.Clear();
			}
		}

		public static void loop()
		{
			pipes.Add(new VectorInt(48, 8));
			Stopwatch swPipes = new Stopwatch();
			swPipes.Start();
			Stopwatch swBird = new Stopwatch();
			swBird.Start();
			while (!gameOver)
			{
				if (swPipes.ElapsedMilliseconds >= 150)
				{
					if (keypress)
					{
						bird.Y -= 2;
						keypress = false;
					}
					else
					{
						bird.Y++;
					}
					if (bird.Y < 0)
					{
						bird.Y = 0;
					}
					if (bird.Y > 16)
					{
						gameOver = true;
					}
					genPipes();
					movePipes();
					drawPipes();
					frameChange = true;
					swPipes.Restart();
				}
				if (frameChange)
				{
					Console.Clear();
					drawFrame();
					frameChange = false;
				}
			}
		}

		public static void movePipes()
		{
			for (int i = 0; i < pipes.Count; i++)
			{
				pipes[i].X = pipes[i].X - 1;
				if (pipes[i].X == 13)
				{
					points++;
				}
				if (pipes[i].X < 0)
				{
					Random r = new Random();
					pipes[i].X = 48;
					pipes[i].Y = r.Next(5, 12);
				}
			}
		}

		public static void genPipes()
		{
			if (pipes.Count < 5 && pipes[pipes.Count - 1].X <= 38)
			{
				Random r = new Random();
				pipes.Add(new VectorInt(48, r.Next(5, 12)));
			}
		}

		public static void drawPipes()
		{
			frameBuffer = new List<Pixel>();
			for (int i = 0; i < pipes.Count; i++)
			{
				int above = pipes[i].Y - 2;
				int under = pipes[i].Y + 1;
				for (int k = 0; k < above; k++)
				{
					Pixel p = new Pixel('H', pipes[i].X, k);
					frameBuffer.Add(p);
				}
				for (int j = 16; j > under; j--)
				{
					Pixel p2 = new Pixel('H', pipes[i].X, j);
					frameBuffer.Add(p2);
				}
			}
		}

		public static void drawFrame()
		{
			for (int i = 0; i < frameBuffer.Count; i++)
			{
				Pixel p = frameBuffer[i];
				if (p.X == bird.X && p.Y == bird.Y)
				{
					gameOver = true;
				}
				Console.SetCursorPosition(p.X, p.Y);
				Console.BackgroundColor = ConsoleColor.Green;
				Console.Write(p.Symbol);
			}
			Console.BackgroundColor = ConsoleColor.Black;
			Console.SetCursorPosition(1, 17);
			Console.Write("points: " + points);
			Console.BackgroundColor = ConsoleColor.Yellow;
			Console.SetCursorPosition(bird.X, bird.Y);
			Console.Write('O');
			Console.BackgroundColor = ConsoleColor.DarkBlue;
		}
	}

}
