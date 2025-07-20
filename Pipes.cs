using Vectors;
using static SDL2.SDL;
using static SDL2.SDL_image;

namespace Pipes{
	class Generic{
		protected IntVector2 pos;
		protected int height;
		public SDL_Rect rect;
		public bool exists;

		Pipes.Subcomponents.ScoreAdder scoreAdder;

		protected IntPtr surface;
		protected IntPtr texture;

		const int width = 80;

		const int timeBetweenPipes = 3;

		protected static Random rHeight = new Random();

		public static List<Generic> totalPipes = new List<Generic>();
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
			scoreAdder = new Pipes.Subcomponents.ScoreAdder();
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
			IncreaseScore();
		}

		static void IncreaseScore(){
			foreach(var pipe in totalPipes){
				pipe.scoreAdder.AddScore();
			}
		}

		static void Move(){
			if (moveStopwatch.Elapsed.TotalMilliseconds <= 5) return;
			foreach(var pipe in totalPipes){
				pipe.pos.X -= 1;
				pipe.rect.x = pipe.pos.X;
				pipe.scoreAdder.pos.X -=1;
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
				if (pipe.pos.X <= -80){
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
			height = rHeight.Next(250, 400);
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
			height = rHeight.Next(250, 400);
			rect.h = height;
			rect.y = Window.Display.windowMax - height;
			pos = new IntVector2(rect.x, rect.y);

			SetTexture();
		}

		void SetTexture(){
			surface = IMG_Load("data/Sprites/PipeDown.png");
			
			if (surface == IntPtr.Zero) throw new ArgumentException($"Could not load DOWN PIPE png in surface. {SDL_GetError()}");
			texture = SDL_CreateTextureFromSurface(Window.Display.renderer, surface);
			if (texture == IntPtr.Zero) throw new Exception($"Could not create DOWN PIPE texture. {SDL_GetError()}");

			SDL_FreeSurface(surface);

		}
	}
}

namespace Pipes.Subcomponents{
	class ScoreAdder{
		public bool ticked;

		public IntVector2 pos;

		public ScoreAdder(){
			ticked = false;
			pos = new IntVector2(Window.Display.windowMax + 80, 0);
		}

		public bool CheckIfPlayerPos(){
			if (Program.player.position.X >= pos.X) return true;
			else return false;
		}

		public void AddScore(){
			if (CheckIfPlayerPos() && !ticked){
				Program.player.score += 0.5f;
				ticked = true;
				Console.WriteLine($"Score: {Program.player.score}");
			}
		}
	}
}
