using Vectors;
using Window;
using static SDL2.SDL;
using static SDL2.SDL_image;

class Player{
	public IntVector2 position;
	bool jumping;
	int speed = 10;
	int g = 2;
	IntVector2 jumpInitPos;

	public float score = 0;

	private IntVector2 spawnPos = new IntVector2(200, 240);

	SDL_Rect rect;

	IntPtr surface;
	IntPtr texture;
	

	public Player(){
		position = spawnPos;
		rect = new SDL_Rect{
			x = position.X,
			y = position.Y,
			w = 80,
			h = 56
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
		CheckForPipes();
		CheckForFloor();
		CheckForRoof();
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
	
	void CheckForPipes(){
		foreach(var pipe in Pipes.Generic.totalPipes){
			if (ChekCollisions(pipe.rect)){
				GameOver();
			}
		}
	}

	void CheckForRoof(){
		if (position.Y <= Display.windowMin && jumping) jumping = false;
	}

	void CheckForFloor(){
		int Ymax = position.Y + rect.h;

		if (Ymax >= Display.windowMax) GameOver();
	}

	void GameOver(){
		foreach(var pipe in Pipes.Generic.totalPipes){
			pipe.exists = false;
		}
		Program.player.position = spawnPos;
		score = 0;
	}

	bool ChekCollisions(SDL_Rect objB){
		int Xmax = position.X + rect.w;
		int Ymax = position.Y + rect.h;
		int objBYmax = objB.y + objB.h;
		int objBXmax = objB.x + objB.w;


		if (((position.Y >= objB.y && position.Y <= objBYmax) || (Ymax >= objB.y  && Ymax <= objBYmax)) && (Xmax >= objB.x && position.X <= objBXmax)) return true;
		else return false;
	}
}
