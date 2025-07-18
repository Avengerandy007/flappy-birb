using Vectors;
using Window;
using static SDL2.SDL;

class Player{
	IntVector2 position;
	bool jumping;
	int speed = 10;
	int g = 10;
	IntVector2 jumpInitPos;

	SDL_Rect rect;
	

	public Player(){
		position = new IntVector2(200, 240);
		rect = new SDL_Rect{
			x = position.X,
			y = position.Y,
			w = 50,
			h = 50,
		};
	}

	public void Update(){
		if (!jumping){
			Gravity(ref position);
			Console.WriteLine($"Player X = {position.X} and Y = {position.Y}");
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
		SDL_SetRenderDrawColor(Display.renderer, 0, 128, 0, 255);
		SDL_RenderDrawRect(Display.renderer, ref rect);
		SDL_RenderFillRect(Display.renderer, ref rect);
		SDL_SetRenderDrawColor(Display.renderer, 0, 0, 0, 255);
	}
}
