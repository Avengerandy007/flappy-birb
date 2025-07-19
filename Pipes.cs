using System;
using Vectors;
using static SDL2.SDL;

namespace Pipes;
class Generic{
	protected IntVector2 pos;
	protected int height;
	protected SDL_Rect rect;

	const int width = 60;

	const int timeBetweenPipes = 2;

	static List<Generic> totalPipes = new List<Generic>();
	static System.Diagnostics.Stopwatch pipeStopwatch = new System.Diagnostics.Stopwatch();

	public static void SetupStopWatch(){
		pipeStopwatch.Start();
	}

	public Generic(){
		rect = new SDL_Rect{
			x = 600,
			//x = Window.Display.windowMax,
			w = width,
		};
		totalPipes.Add(this);
	}

	public static void RenderPipes(){
		foreach (var pipe in totalPipes){
			SDL_SetRenderDrawColor(Window.Display.renderer, 0,  128, 0, 255);
			SDL_RenderDrawRect(Window.Display.renderer, ref pipe.rect);
			SDL_RenderFillRect(Window.Display.renderer, ref pipe.rect);
			SDL_SetRenderDrawColor(Window.Display.renderer, 135, 206, 250, 255);
		}
	}

	public static void CreatePipe(){
		if (pipeStopwatch.Elapsed.TotalSeconds <= timeBetweenPipes) return;
		new UpPipe();
		new DownPipe();
		pipeStopwatch.Restart();
	}
}

class UpPipe : Generic{
	public UpPipe(){
		Random rGapYmin = new Random();
		height = rGapYmin.Next(250, 450);
		rect.h = height;
		rect.y = Window.Display.windowMin;
		pos = new IntVector2(rect.x, rect.y);
		Console.WriteLine("new UpPipe");
	}
}

class DownPipe : Generic{
	public DownPipe(){
		Random rGapYmax = new Random();
		height = rGapYmax.Next(250, 450);
		rect.h = height;
		rect.y = Window.Display.windowMax - height;
		pos = new IntVector2(rect.x, rect.y);
		Console.WriteLine("new DownPipe");
	}
}
