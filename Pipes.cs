using System;
using Vectors;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace Pipes;
class Generic{
	protected IntVector2 pos;
	protected int height;
	protected SDL_Rect rect;
	bool exists;

	protected IntPtr surface;
	protected IntPtr texture;

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
			SDL_RenderCopy(Window.Display.renderer, pipe.texture, IntPtr.Zero, ref pipe.rect);
		}
	}

	public static void Update(){
		Move();
		DestroyIfOutOfBounds();
		ClearList();
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

	public static void DestoryAllTextures(){
		foreach(var pipe in totalPipes){
			pipe.CleanThisTexture();
		}
	}

	public void CleanThisTexture(){
		SDL_DestroyTexture(texture);
	}

	static void DestroyIfOutOfBounds(){
		foreach(var pipe in totalPipes){
			if (pipe.pos.X <= 0){
				pipe.exists = false;
				pipe.CleanThisTexture();
			} 
		}
	}

	public static void ClearList(){
		totalPipes.RemoveAll((pipe => !pipe.exists));
	}
}

class UpPipe : Generic{
	public UpPipe(){
		Random rGapYmin = new Random();
		height = rGapYmin.Next(250, 430);
		rect.h = height;
		rect.y = Window.Display.windowMin;
		pos = new IntVector2(rect.x, rect.y);

		SetTexture();
	}

	void SetTexture(){
		surface = IMG_Load("data/Sprites/PipeUp.png");
		
		if (surface == IntPtr.Zero) throw new ArgumentException($"Could not load UP PIPE png in surface. {SDL_GetError()}");

		texture = SDL_CreateTextureFromSurface(Window.Display.renderer, surface);
		if (texture == IntPtr.Zero) throw new Exception($"Could not create UP PIPE texture. {SDL_GetError()}");

		SDL_FreeSurface(surface);

	}
}

class DownPipe : Generic{
	public DownPipe(){
		Random rGapYmax = new Random();
		height = rGapYmax.Next(250, 430);
		rect.h = height;
		rect.y = Window.Display.windowMax - height;
		pos = new IntVector2(rect.x, rect.y);

		SetTexture();
	}

	void SetTexture(){
		surface = IMG_Load("data/Sprites/PipeDown.png");
		
		if (surface == IntPtr.Zero) throw new ArgumentException($"Could not load UP PIPE png in surface. {SDL_GetError()}");

		texture = SDL_CreateTextureFromSurface(Window.Display.renderer, surface);
		if (texture == IntPtr.Zero) throw new Exception($"Could not create UP PIPE texture. {SDL_GetError()}");

		SDL_FreeSurface(surface);

	}
}
