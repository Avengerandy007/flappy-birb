using Vectors;
using Window;
using static SDL2.SDL;
using static SDL2.SDL_image;

class Player{
	IntVector2 position;
	bool jumping;
	int speed = 10;
	int g = 10;
	IntVector2 jumpInitPos;

	SDL_Rect rect;

	IntPtr surface;
	IntPtr texture;
	

	public Player(){
		position = new IntVector2(200, 240);
		rect = new SDL_Rect{
			x = position.X,
			y = position.Y,
			w = 40,
			h = 28,
		};
	}

	public void SetupTexture(){
		surface = IMG_Load("data/Sprites/Player.png");
		if (surface == IntPtr.Zero) throw new ArgumentException($"Could not load player png in surface. {SDL_GetError()}");

		texture = SDL_CreateTextureFromSurface(Display.renderer, surface);
		if (texture == IntPtr.Zero) throw new Exception($"Could not create player texture. {SDL_GetError()}");

		SDL_FreeSurface(surface);
	}

	public void Update(){
		if (!jumping){
			Gravity(ref position);
		}else Jump();
		rect.y = position.Y;
	}

	void Gravity(ref IntVector2 currPos){
		currPos.Y += g;
	}
	
	public void JumpEvent(){
		if (jumping) return;
		jumping = true;
		jumpInitPos = position;
	}

	void Jump(){
		if ((jumpInitPos.Y - position.Y) > 100) jumping = false;
		position.Y -= speed;
	}

	public void Render(){
		SDL_RenderCopy(Display.renderer, texture, IntPtr.Zero, ref rect);
	}

	public void ClearTexture(){
		SDL_DestroyTexture(texture);
	}
}
