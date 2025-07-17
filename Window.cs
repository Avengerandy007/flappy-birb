using static SDL2.SDL;
using static SDL2.SDL_ttf;
using static SDL2.SDL_image;


namespace Window;
static class Display{
	public static IntPtr window;
	public static IntPtr renderer;

	public static void Setup(){

		if (SDL_Init(SDL_INIT_VIDEO) < 0){
			throw new Exception($"There was a problem intialising SDL video services: {SDL_GetError()}");
		}

		window = SDL_CreateWindow("Shitty version of Flappy Bird", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, 640, 480, SDL_WindowFlags.SDL_WINDOW_SHOWN | SDL_WindowFlags.SDL_WINDOW_RESIZABLE);
		
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
	}
	
	public static void Render(){
		SDL_SetRenderDrawColor(renderer, 135, 206, 250, 255);
		SDL_RenderClear(renderer);
		//TODO: Render objects methods
		SDL_RenderPresent(renderer);
	}

	public static void Clean(){
		SDL_DestroyWindow(window);
		SDL_DestroyRenderer(renderer);
		IMG_Quit();
		SDL_Quit();
	}
}
