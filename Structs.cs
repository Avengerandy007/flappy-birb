namespace Vectors;

struct IntVector2{
	public int X = 0;
	public int Y = 0;

	public IntVector2(int x, int y){
		X = x;
		Y = y;
	}

	public override bool Equals(object? o){
		if (o == null || GetType() != o.GetType()) return false;

		IntVector2 vec2 = (IntVector2)o;

		return (X == vec2.X && Y == vec2.Y);
	}

	public static bool operator == (IntVector2 vec1, IntVector2 vec2){
		return vec1.Equals(vec2);
	}

	public static bool operator != (IntVector2 vec1, IntVector2 vec2){
		return !vec1.Equals(vec2);
	}

    	public override int GetHashCode()
    	{
		return X.GetHashCode() ^ Y.GetHashCode();
    	}
}
