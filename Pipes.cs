using System;
using Vectors;
using static SDL2.SDL;

namespace Pipes;
class Generic{
	protected IntVector2 pos;
	int height;
	protected SDL_Rect rect;

	const int width = 60;

	static List<UpPipe> totalPipes = new List<UpPipe>();//Idk maybe use tuples for this?

	public Generic(){
		Random rHeight = new Random();
		height = rHeight.Next(5, 10) * 10;
		rect = new SDL_Rect{
			x = Window.Display.windowMax,
			w = width,
			h = height,
		};
	}
}

class UpPipe : Generic{
	public UpPipe(){
		rect.y = Window.Display.windowMin + rect.h;
		pos = new IntVector2(rect.x, rect.y);
	}
}

class DownPipe : Generic{
	public DownPipe(){
		rect.y = Window.Display.windowMax - rect.h;
		pos = new IntVector2(rect.x, rect.y);
	}
}
