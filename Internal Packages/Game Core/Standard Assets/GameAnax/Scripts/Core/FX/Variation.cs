using UnityEngine;
using rand = UnityEngine.Random;


public class Variation : MonoBehaviour {
	[Header("Move Speed vartions fields")]
	[SerializeField]
	private bool isChangeMovementSpeed;
	[SerializeField]
	private Vector2 changeMovSpeedAfterSec;
	[SerializeField]
	private Vector2 xMoveSpeedVaritions;
	[SerializeField]
	private Vector2 yMoveSpeedVaritions;
	[SerializeField]
	private Vector2 zMoveSpeedVaritions;

	[HideInInspector]
	public Vector3 moveSpeed = Vector3.one;
	private bool _isFirstTimeChangeMoveSpeed;

	private void StopMoveVariations() {
		CancelInvoke("MoveSpeedX");
		CancelInvoke("MoveSpeedY");
		CancelInvoke("MoveSpeedZ");
	}
	private void StartMoveVariations() {
		if(!isChangeMovementSpeed || !_isFirstTimeChangeMoveSpeed)
			return;

		if(_isFirstTimeChangeMoveSpeed) {
			_isFirstTimeChangeMoveSpeed = false;
			MoveSpeedX();
			MoveSpeedY();
			MoveSpeedZ();
		}
	}

	private void MoveSpeedX() {
		float _nextChange;
		_nextChange = changeMovSpeedAfterSec.x.Equals(changeMovSpeedAfterSec.y) ? changeMovSpeedAfterSec.x :
			rand.Range(changeMovSpeedAfterSec.x, changeMovSpeedAfterSec.y);
		moveSpeed.x = rand.Range(xMoveSpeedVaritions.x, xMoveSpeedVaritions.y);
		Invoke("MoveSpeedX", _nextChange);
	}
	private void MoveSpeedY() {
		float _nextChange;
		_nextChange = changeMovSpeedAfterSec.x.Equals(changeMovSpeedAfterSec.y) ? changeMovSpeedAfterSec.x :
			rand.Range(changeMovSpeedAfterSec.x, changeMovSpeedAfterSec.y);
		moveSpeed.y = rand.Range(yMoveSpeedVaritions.x, yMoveSpeedVaritions.y);
		Invoke("MoveSpeedY", _nextChange);
	}
	private void MoveSpeedZ() {
		float _nextChange;
		_nextChange = changeMovSpeedAfterSec.x.Equals(changeMovSpeedAfterSec.y) ? changeMovSpeedAfterSec.x :
			rand.Range(changeMovSpeedAfterSec.x, changeMovSpeedAfterSec.y);
		moveSpeed.z = rand.Range(zMoveSpeedVaritions.x, zMoveSpeedVaritions.y);
		Invoke("MoveSpeedZ", _nextChange);
	}



	[Space(10)]
	[Header("Rotation Speed vartions fields")]
	[SerializeField]
	private bool isChangeRotationSpeed;
	[SerializeField]
	private Vector2 changeRotSpeedAfterSec;
	[SerializeField]
	private Vector2 xRotSpeedVaritions;
	[SerializeField]
	private Vector2 yRotSpeedVaritions;
	[SerializeField]
	private Vector2 zRotSpeedVaritions;

	[HideInInspector]
	public Vector3 rotSpeed = Vector3.one;
	private bool _isFirstTimeChangeRotSpeed;

	private void StopRotVariations() {
		CancelInvoke("RotSpeedX");
		CancelInvoke("RotSpeedY");
		CancelInvoke("RotSpeedZ");
	}
	private void StartRotVariations() {
		if(!isChangeRotationSpeed || !_isFirstTimeChangeRotSpeed)
			return;


		if(_isFirstTimeChangeRotSpeed) {
			_isFirstTimeChangeRotSpeed = false;
			RotSpeedX();
			RotSpeedY();
			RotSpeedZ();
		}
	}

	private void RotSpeedX() {
		float _nextChange = rand.Range(changeRotSpeedAfterSec.x, changeRotSpeedAfterSec.y);
		rotSpeed.x = rand.Range(xRotSpeedVaritions.x, xRotSpeedVaritions.y);
		Invoke("RotSpeedX", _nextChange);
	}
	private void RotSpeedY() {
		float _nextChange = rand.Range(changeRotSpeedAfterSec.x, changeRotSpeedAfterSec.y);
		rotSpeed.y = rand.Range(yRotSpeedVaritions.x, yRotSpeedVaritions.y);
		Invoke("RotSpeedY", _nextChange);
	}
	private void RotSpeedZ() {
		float _nextChange = rand.Range(changeRotSpeedAfterSec.x, changeRotSpeedAfterSec.y);
		rotSpeed.z = rand.Range(zRotSpeedVaritions.x, zRotSpeedVaritions.y);
		Invoke("RotSpeedZ", _nextChange);
	}

	[Space(20)]
	[Header("Movement Pause Resume Field")]
	[SerializeField]
	private bool isMovePauseResume;
	[SerializeField]
	private Vector2 movePauseAfter;
	[SerializeField]
	private Vector2 moveResumeAfter;
	[HideInInspector]
	public bool[] isMovePause = { false, false, false };
	private bool _isFirstTimeMovePauseResumeCall;

	private void StopMovePauseResume() {
		isMovePause = new[] { false, false, false };
		CancelInvoke("MovePauseX");
		CancelInvoke("MovePauseY");
		CancelInvoke("MovePauseZ");
	}
	private void StartMovePauseResume() {
		if(!isMovePauseResume || !_isFirstTimeMovePauseResumeCall) {
			return;
		}
		MovePauseX();
		MovePauseY();
		MovePauseZ();
		_isFirstTimeMovePauseResumeCall = false;

	}

	private void MovePauseX() {
		if(!_isFirstTimeMovePauseResumeCall) isMovePause[0] = !isMovePause[0];
		float _nextChange = isMovePause[0] ? rand.Range(moveResumeAfter.x, moveResumeAfter.y) : rand.Range(movePauseAfter.x, movePauseAfter.y);
		Invoke("MovePauseX", _nextChange);
	}
	private void MovePauseY() {
		if(!_isFirstTimeMovePauseResumeCall) isMovePause[1] = !isMovePause[1];
		float _nextChange = isMovePause[1] ? rand.Range(moveResumeAfter.x, moveResumeAfter.y) : rand.Range(movePauseAfter.x, movePauseAfter.y);
		Invoke("MovePauseY", _nextChange);
	}
	private void MovePauseZ() {
		if(!_isFirstTimeMovePauseResumeCall) isMovePause[2] = !isMovePause[2];
		float _nextChange = isMovePause[2] ? rand.Range(moveResumeAfter.x, moveResumeAfter.y) : rand.Range(movePauseAfter.x, movePauseAfter.y);
		Invoke("MovePauseZ", _nextChange);
	}

	[Space(10)]
	[Header("Rotation Pause Resume Field")]
	[SerializeField]
	private bool isRotPauseResume;
	[SerializeField]
	private Vector2 rotPauseAfter;
	[SerializeField]
	private Vector2 rotResumeAfter;
	[HideInInspector]
	public bool[] isRotPause = { false, false, false };
	private bool _isFirstTimeRotPauseResumeCall;

	private void StopRotPauseResume() {
		isRotPause = new[] { false, false, false };
		CancelInvoke("RotPauseX");
		CancelInvoke("RotPauseY");
		CancelInvoke("RotPauseZ");
	}
	private void StartRotPauseResume() {
		if(!isRotPauseResume || !_isFirstTimeRotPauseResumeCall) {
			return;
		}
		RotPauseX();
		RotPauseY();
		RotPauseZ();
		_isFirstTimeRotPauseResumeCall = false;
	}

	private void RotPauseX() {
		if(!_isFirstTimeRotPauseResumeCall) isRotPause[0] = !isRotPause[0];
		float _nextChange = isRotPause[0] ? rand.Range(rotResumeAfter.x, rotResumeAfter.y) : rand.Range(rotPauseAfter.x, rotPauseAfter.y);
		Invoke("RotPauseX", _nextChange);
	}
	private void RotPauseY() {
		if(!_isFirstTimeRotPauseResumeCall) isRotPause[1] = !isRotPause[1];
		float _nextChange = isRotPause[1] ? rand.Range(rotResumeAfter.x, rotResumeAfter.y) : rand.Range(rotPauseAfter.x, rotPauseAfter.y);
		Invoke("MovePauseY", _nextChange);
	}
	private void RotPauseZ() {
		if(!_isFirstTimeRotPauseResumeCall) isRotPause[2] = !isRotPause[2];
		float _nextChange = isRotPause[2] ? rand.Range(rotResumeAfter.x, rotResumeAfter.y) : rand.Range(rotPauseAfter.x, rotPauseAfter.y);
		Invoke("RotPauseZ", _nextChange);
	}



	[Space(20)]
	[Header("Move Direction Variation Field")]
	[SerializeField]
	private bool isMoveDirection;
	[SerializeField]
	private Vector2 changeMoveReverseAfterSec;
	[SerializeField]
	private Vector2 changeMoveNormalAfterSec;
	[HideInInspector]
	public Vector3 curMoveDir = Vector3.one;
	private Vector3 _oriMoveDir = Vector3.one;
	private bool _isFirstTimeChangeMoveDir;

	private void StopMoveDir() {
		CancelInvoke("MoveDirX");
		CancelInvoke("MoveDirY");
		CancelInvoke("MoveDirZ");
	}
	private void StartChangeMoveDir() {
		if(!isMoveDirection || !_isFirstTimeChangeMoveDir) {
			return;
		}
		MoveDirX();
		MoveDirY();
		MoveDirZ();

		_isFirstTimeChangeMoveDir = false;
	}

	private void MoveDirX() {
		float _nextChange;
		if(curMoveDir.x.Equals(_oriMoveDir.x)) {
			_nextChange = rand.Range(changeMoveNormalAfterSec.x, changeMoveNormalAfterSec.y);
		} else {
			_nextChange = rand.Range(changeMoveReverseAfterSec.x, changeMoveReverseAfterSec.y);
		}

		if(!_isFirstTimeChangeMoveDir) { curMoveDir.x *= -1; }
		Invoke("MoveDirX", _nextChange);
	}
	private void MoveDirY() {
		float _nextChange;
		if(curMoveDir.y.Equals(_oriMoveDir.y)) {
			_nextChange = rand.Range(changeMoveNormalAfterSec.x, changeMoveNormalAfterSec.y);
		} else {
			_nextChange = rand.Range(changeMoveReverseAfterSec.x, changeMoveReverseAfterSec.y);
		}

		if(!_isFirstTimeChangeMoveDir) { curMoveDir.y *= -1; }
		Invoke("MoveDirY", _nextChange);
	}
	private void MoveDirZ() {
		float _nextChange;
		if(curMoveDir.z.Equals(_oriMoveDir.z)) {
			_nextChange = rand.Range(changeMoveNormalAfterSec.x, changeMoveNormalAfterSec.y);
		} else {
			_nextChange = rand.Range(changeMoveReverseAfterSec.x, changeMoveReverseAfterSec.y);
		}

		if(!_isFirstTimeChangeMoveDir) { curMoveDir.z *= -1; }
		Invoke("MoveDirZ", _nextChange);
	}

	[Space(20)]
	[Header("Rotation Direction Variation Field")]
	[SerializeField]
	private bool isRotDirection;
	[SerializeField]
	private Vector2 changeRotReverseAfterSec;
	[SerializeField]
	private Vector2 changeRotNormalAfterSec;
	[HideInInspector]
	public Vector3 curRotDir = Vector3.one;
	private Vector3 _oriRotDir = Vector3.one;
	private bool _isFirstTimeChangeRotDir;

	private void StopRotDir() {
		CancelInvoke("RotDirX");
		CancelInvoke("RotDirY");
		CancelInvoke("RotDirZ");
	}
	private void StartChangeRotDir() {
		if(!isRotDirection || !_isFirstTimeChangeRotDir) {
			return;
		}
		RotDirX();
		RotDirY();
		RotDirZ();

		_isFirstTimeChangeRotDir = false;
	}

	private void RotDirX() {
		float _nextChange;
		if(curRotDir.x.Equals(_oriRotDir.x)) {
			_nextChange = rand.Range(changeRotNormalAfterSec.x, changeRotNormalAfterSec.y);
		} else {
			_nextChange = rand.Range(changeRotReverseAfterSec.x, changeRotReverseAfterSec.y);
		}
		curRotDir.x *= -1;
		if(_isFirstTimeChangeRotDir) { curRotDir.x *= -1; }
		Invoke("RotDirX", _nextChange);
	}
	private void RotDirY() {
		float _nextChange;
		if(curRotDir.x.Equals(_oriRotDir.x)) {
			_nextChange = rand.Range(changeRotNormalAfterSec.x, changeRotNormalAfterSec.y);
		} else {
			_nextChange = rand.Range(changeRotReverseAfterSec.x, changeRotReverseAfterSec.y);
		}
		curRotDir.y *= -1;
		if(_isFirstTimeChangeRotDir) { curRotDir.y *= -1; }
		Invoke("RotDirY", _nextChange);
	}
	private void RotDirZ() {
		float _nextChange;
		if(curRotDir.x.Equals(_oriRotDir.x)) {
			_nextChange = rand.Range(changeRotNormalAfterSec.x, changeRotNormalAfterSec.y);
		} else {
			_nextChange = rand.Range(changeRotReverseAfterSec.x, changeRotReverseAfterSec.y);
		}
		curRotDir.z *= -1;
		if(_isFirstTimeChangeRotDir) { curRotDir.z *= -1; }
		Invoke("RotDirZ", _nextChange);
	}



	public void CancelAllVaritions() {
		StopMoveVariations();
		StopRotVariations();

		StopMovePauseResume();
		StopRotVariations();

		StopMoveDir();
		StopRotDir();

		rotSpeed = Vector3.one;
		moveSpeed = Vector3.one;

		isMovePause = new[] { false, false, false };
		isRotPause = new[] { false, false, false };

		curMoveDir = Vector3.one;
		curRotDir = Vector3.one;
	}
	public void StartVaritions() {
		_isFirstTimeChangeMoveSpeed = true;
		_isFirstTimeChangeRotSpeed = true;

		_isFirstTimeMovePauseResumeCall = true;
		_isFirstTimeRotPauseResumeCall = true;

		_isFirstTimeChangeMoveDir = true;
		_isFirstTimeChangeMoveDir = true;


		rotSpeed = Vector3.one;
		moveSpeed = Vector3.one;

		isMovePause = new[] { false, false, false };
		isRotPause = new[] { false, false, false };

		curMoveDir = Vector3.one;
		curRotDir = Vector3.one;


		StartMoveVariations();
		StartRotVariations();

		StartMovePauseResume();
		StartRotPauseResume();

		StartChangeMoveDir();
		StartChangeRotDir();
	}
}