[System.Serializable]
public class Point {
	public int X, Y;
	public Point() {
		X = 0;
		Y = 0;
	}
	public Point(int x, int y) {
		X = x;
		Y = y;
	}
	public Point(Point p) {
		X = p.X;
		Y = p.Y;
	}

	public string ToFormatedString() {
		return string.Format("X: {0}, Y: {1}", X, Y);
	}

	public override string ToString() {
		return string.Format("{0}, {1}", X, Y);
	}
	public override bool Equals(object o) {
		return this == (Point)o;
	}
	public static bool operator ==(Point a, Point b) {
		// If both are null, or both are same instance, return true.
		if(System.Object.ReferenceEquals(a, b)) {
			return true;
		}

		// If one is null, but not both, return false.
		if(((object)a == null) || ((object)b == null)) {
			return false;
		}

		// Return true if the fields match:
		return a.X == b.X && a.Y == b.Y;
	}

	public static bool operator !=(Point a, Point b) {
		return !(a == b);
	}
	public override int GetHashCode() {
		return base.GetHashCode();
	}
}
