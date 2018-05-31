[System.Serializable]
public class CellAddress {
	public int row, col;
	public CellAddress() {
		row = 0;
		col = 0;
	}
	public CellAddress(int row, int col) {
		this.row = row;
		this.col = col;
	}
	public CellAddress(CellAddress p) {
		row = p.row;
		col = p.col;
	}

	public string ToFormatedString() {
		return string.Format("R: {0}, C: {1}", row, col);
	}

	public override string ToString() {
		return string.Format("[{0}, {1}]", row, col);
	}
	public override bool Equals(object o) {
		return this == (CellAddress)o;
	}
	public static bool operator ==(CellAddress a, CellAddress b) {
		// If both are null, or both are same instance, return true.
		if(System.Object.ReferenceEquals(a, b)) {
			return true;
		}

		// If one is null, but not both, return false.
		if(((object)a == null) || ((object)b == null)) {
			return false;
		}

		// Return true if the fields match:
		return a.row == b.row && a.col == b.col;
	}

	public static bool operator !=(CellAddress a, CellAddress b) {
		return !(a == b);
	}
	public override int GetHashCode() {
		return base.GetHashCode();
	}
}
