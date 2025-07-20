using Vectors;
using static SDL2.SDL_ttf;
using static SDL2.SDL;

class UI{
	IntVector2 pos;
	SDL_Rect rect;
	static IntPtr font;
	static SDL_Color white;

	public float variableToDisplay;
	string textToDisplay;

	IntPtr texture;
	IntPtr surface;

	static List<UI> totalUIElements = new List<UI>();

	public UI(string text, int PosX, int PosY, int RectW, int RectH){
		textToDisplay = text;
		pos = new IntVector2(PosX, PosY);
		rect = new SDL_Rect{
			x = pos.X,
			y = pos.Y,
			w = RectW,
			h = RectH
		};
		
		totalUIElements.Add(this);
	}

	public static void OpenFont(){
		font = TTF_OpenFont("data/Font/Font.ttf", 32);
		if (font == IntPtr.Zero) throw new Exception($"There was a problem openning the ui font. {SDL_GetError()}");
		white = new SDL_Color{r = 255, g = 255, b = 255, a = 255};
	}

	public void Render(){
		surface = TTF_RenderText_Solid(font, $"{textToDisplay} {variableToDisplay}", white);
		if (surface == IntPtr.Zero) throw new Exception($"Couldn't load UI surface. {SDL_GetError()}");
		texture = SDL_CreateTextureFromSurface(Window.Display.renderer, surface);
		if (texture == IntPtr.Zero) throw new Exception($"Couldn't create UI texture. {SDL_GetError()}");
		SDL_FreeSurface(surface);
		SDL_RenderCopy(Window.Display.renderer, texture, IntPtr.Zero, ref rect);
		SDL_DestroyTexture(texture);
		texture = IntPtr.Zero;
	}

	public static void CleanUp(){
		foreach(var element in totalUIElements){
			SDL_FreeSurface(element.surface);
			SDL_DestroyTexture(element.texture);
			element.texture = IntPtr.Zero;
			element.surface = IntPtr.Zero;
		} 
		TTF_CloseFont(font);
		font = IntPtr.Zero;
	}
}
