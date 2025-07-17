using System;
using SDL2;
using Window;

class Program{
	
	static bool running;
	static void Main(){
		running = true;
		Console.WriteLine("Hello from shitty flappy bird!");
		Display.Setup();
		Update();
	}

	static void Update(){
		while(running){
			Display.Render();
			PollEvents();
		}
		Display.Clean();
	}

	static void PollEvents(){
		while(SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1){
			switch(e.type){
				case SDL.SDL_EventType.SDL_QUIT:
					running = false;
					break;
			}
		}
	}
}
