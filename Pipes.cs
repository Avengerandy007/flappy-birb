using System;
using Vectors;
using static SDL2.SDL;

namespace Pipes;
class Generic{
	protected IntVector2 pos;
	protected int height;
	protected SDL_Rect rect;
	bool exists;

	const int width = 80;

	const int timeBetweenPipes = 3;

	static List<Generic> totalPipes = new List<Generic>();
	static System.Diagnostics.Stopwatch pipeStopwatch = new System.Diagnostics.Stopwatch();
	static System.Diagnostics.Stopwatch moveStopwatch = new System.Diagnostics.Stopwatch();

	public static void SetupStopWatch(){
		pipeStopwatch.Start();
		moveStopwatch.Start();
	}

	public Generic(){
		rect = new SDL_Rect{
			x = Window.Display.windowMax,
			w = width,
		};
		exists = true;
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

	public static void Update(){
		Move();
	}

	static void Move(){
		if (moveStopwatch.Elapsed.TotalMilliseconds <= 5) return;
		foreach(var pipe in totalPipes){
			pipe.pos.X -= 1;
			pipe.rect.x = pipe.pos.X;
		}
		moveStopwatch.Restart();
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
		height = rGapYmin.Next(250, 430);
		rect.h = height;
		rect.y = Window.Display.windowMin;
		pos = new IntVector2(rect.x, rect.y);
	}
}

class DownPipe : Generic{
	public DownPipe(){
		Random rGapYmax = new Random();
		height = rGapYmax.Next(250, 430);
		rect.h = height;
		rect.y = Window.Display.windowMax - height;
		pos = new IntVector2(rect.x, rect.y);
	}
}
