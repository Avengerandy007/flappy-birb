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

		static IntPtr surfaceUp = IMG_Load("data/Sprites/PipeUp.png");
		static IntPtr surfaceDown = IMG_Load("data/Sprites/PipeDown.png");
		static IntPtr textureUp =  SDL_CreateTextureFromSurface(Window.Display.renderer, surfaceUp);
		static IntPtr textureDown = SDL_CreateTextureFromSurface(Window.Display.renderer, surfaceDown);

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
				if (pipe is UpPipe) SDL_RenderCopy(Window.Display.renderer, textureUp, IntPtr.Zero, ref pipe.rect);
				else SDL_RenderCopy(Window.Display.renderer, textureDown, IntPtr.Zero, ref pipe.rect);
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
			if (pipeStopwatch.Elapsed.TotalSeconds <= timeBetweenPipes || totalPipes.Count >= 8) return;
			new UpPipe();
			new DownPipe();
			pipeStopwatch.Restart();
		}

		public static void FreeSurfaces(){
			SDL_FreeSurface(surfaceDown);
			SDL_FreeSurface(surfaceUp);
		}

		public static void CleanUp(){
			SDL_DestroyTexture(textureUp);
			SDL_DestroyTexture(textureDown);
		}

		static void DestroyIfOutOfBounds(){
			foreach(var pipe in totalPipes){
				if (pipe.pos.X <= -80){
					pipe.exists = false;
				} 
			}
		}

		public static void ClearList(){
			for(int i = 0; i < totalPipes.Count; i++){
				if (!totalPipes[i].exists){
					totalPipes.RemoveAt(i);
				}
			}
		}
	}

	class UpPipe : Generic{
		public UpPipe(){
			height = rHeight.Next(250, 400);
			rect.h = height;
			rect.y = Window.Display.windowMin;
			pos = new IntVector2(rect.x, rect.y);
		}
	}

	class DownPipe : Generic{
		public DownPipe(){
			height = rHeight.Next(250, 400);
			rect.h = height;
			rect.y = Window.Display.windowMax - height;
			pos = new IntVector2(rect.x, rect.y);
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
