using static SDL2.SDL_mixer;

namespace Audio{

	class Controller{
		public static void OpenSound(){
			Mix_OpenAudio(44100, MIX_DEFAULT_FORMAT, 2, 1024);
		}
	}

	class Sound{
		IntPtr file;
		public Sound(string name){
			file = Mix_LoadWAV($"data/Sound/{name}.wav");
			if (file == IntPtr.Zero) throw new Exception($"Provided file path for {name} sound is invalid. {SDL2.SDL.SDL_GetError()}");
		}

		public void Play(){
			Mix_PlayChannel(-1, file, 0);
		}

		public void Free(){
			Mix_FreeChunk(file);
		}
	}
}
