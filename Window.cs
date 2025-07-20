using static SDL2.SDL;
using static SDL2.SDL_ttf;
using static SDL2.SDL_image;


namespace Window;
static class Display{
	public static IntPtr window;
	public static IntPtr renderer;

	public const int windowMax = 1000;
	public const int windowMin = 0;

	static IntPtr bgTexture;

	static SDL_Rect bgRect = new SDL_Rect{
		x = 0,
		y = 0,
		w = 1000,
		h = 1000
	};

	public static void Setup(){

		if (SDL_Init(SDL_INIT_VIDEO) < 0){
			throw new Exception($"There was a problem intialising SDL video services: {SDL_GetError()}");
		}

		window = SDL_CreateWindow("Shitty version of Flappy Bird", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 1000, 1000, SDL_WindowFlags.SDL_WINDOW_SHOWN);
		
		if (window == IntPtr.Zero){
			throw new Exception($"There was a problem definning the SDL window: {SDL_GetError()}");
		}

		renderer =  SDL_CreateRenderer(window, -1, SDL_RendererFlags.SDL_RENDERER_ACCELERATED);

		if (renderer == IntPtr.Zero){
			throw new Exception($"There was a problem definning the SDL renderer: {SDL_GetError()}");
		}

		if (IMG_Init(IMG_InitFlags.IMG_INIT_JPG) < 0){
			throw new Exception($"There was a problem initialising SDL image services: {SDL_GetError()}");
		}

		IntPtr bgSurface = IMG_Load("data/Sprites/Background.png");

		bgTexture = SDL_CreateTextureFromSurface(renderer, bgSurface);

		SDL_FreeSurface(bgSurface);
	}
	
	public static void Render(){
		SDL_SetRenderDrawColor(renderer, 135, 206, 250, 255);
		SDL_RenderClear(renderer);
		SDL_RenderCopy(renderer, bgTexture, IntPtr.Zero, ref bgRect);
		Program.player.Render();
		Pipes.Generic.RenderPipes();
		SDL_RenderPresent(renderer);
	}

	public static void Clean(){
		SDL_DestroyWindow(window);
		SDL_DestroyRenderer(renderer);
		Program.player.ClearTexture();
		SDL_DestroyTexture(bgTexture);
		IMG_Quit();
		SDL_Quit();
	}
}
