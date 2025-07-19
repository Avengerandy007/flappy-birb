using System;
using SDL2;
using Window;

class Program{
	
	static bool running;
	public static Player player = new Player();
	static void Main(){
		running = true;
		Console.WriteLine("Hello from shitty flappy bird!");
		Display.Setup();
		player.SetupTexture();
		Pipes.Generic.SetupStopWatch();
		Update();
	}

	static void Update(){
		while(running){
			Display.Render();
			PollEvents();
			player.Update();
			Pipes.Generic.CreatePipe();	

			SDL.SDL_Delay(50);
		}
		Display.Clean();
	}

	static void PollEvents(){
		while(SDL.SDL_PollEvent(out SDL.SDL_Event e) == 1){
			switch(e.type){
				case SDL.SDL_EventType.SDL_QUIT:
					running = false;
					break;

				case SDL.SDL_EventType.SDL_KEYDOWN:
					if (e.key.keysym.sym == SDL.SDL_Keycode.SDLK_SPACE){
						player.JumpEvent();
					}
					break;
			}
		}
	}
}
