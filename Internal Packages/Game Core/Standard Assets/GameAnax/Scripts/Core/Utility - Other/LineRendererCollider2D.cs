using System.Collections.Generic;

using UnityEngine;

using GameAnax.Core;
using GameAnax.Core.Attributes;
using GameAnax.Core.Extension;
using GameAnax.Core.Utility;


public class LineRendererCollider2D : MonoBehaviour {
	//http://answers.unity3d.com/questions/470943/collider-for-line-renderer.html

	[SerializeField]
	private Sprite sprite;
	[SerializeField]
	private Color lineColor;

	private LineRenderer _line;
	private List<GameObject> _capsules = new List<GameObject>();
	private CapsuleCollider2D _capsule;
	private Vector3 _p1, _p2;
	private Vector2 _size;
	private Rigidbody2D _r2d;
	private SpriteRenderer _spriteRenderer;

	void Awake() { _line = GetComponent<LineRenderer>(); }
	void Start() {
		_size.y = _line.endWidth + 0.1f;
	}


	GameObject _go;
	Vector2 _x1, _x2, _diference;
	float _sign, _eluZ;
	Vector3 _el;

	[ButtonInspector("Created 2D Collider")]
	public void CreateCollider2D() {
		CreateCollider2D("Line");
	}
	public void CreateCollider2D(string thisTag) {

		_capsules.ForEach(o => Destroy(o));
		_capsules.Clear();

		for(int i = 0; i < _line.positionCount - 1; i++) {
			if(i + 1 >= _line.positionCount) continue;

			_p1 = _line.GetPosition(i);
			_p2 = _line.GetPosition(i + 1);
			_x1 = _p1.Cast();
			_x2 = _p2.Cast();

			_go = new GameObject("Capsual " + i);
			_go.transform.SetParent(_line.transform);
			if(!thisTag.IsNulOrEmpty()) _go.tag = thisTag;
			_go.layer = 8;
			_go.transform.localPosition = _p1 + (_p2 - _p1) / 2f;
			_go.transform.localScale = Vector3.one;

			_size.x = (Vector2.Distance(_p1, _p2));

			if(_x1.x < _x2.x)
				_eluZ = _x1.GetAngleTo(_x2);
			else
				_eluZ = _x2.GetAngleTo(_x1);
			_eluZ += 180f;

			_el = Vector3.zero;
			_el.z = _eluZ;
			_go.transform.localEulerAngles = _el;
			_capsules.Add(_go);

			_capsule = _go.AddComponent<CapsuleCollider2D>();
			_capsule.isTrigger = true;
			_capsule.size = _size;

			//_capsule.offset = Vector3.zero;
			Vector2 offset = _capsule.offset;
			offset.y = 0.05f;
			_capsule.offset = offset;

			_capsule.direction = CapsuleDirection2D.Horizontal;

			_spriteRenderer = _go.AddComponent<SpriteRenderer>();
			_spriteRenderer.sprite = sprite;
			_spriteRenderer.drawMode = SpriteDrawMode.Sliced;
			_spriteRenderer.color = lineColor;

			_size.y = 0.18f;
			_size.x = Mathf.Max(_size.x, 0.22f);
			_spriteRenderer.size = _size;

			_go.transform.localScale = Vector3.one;

			_r2d = _go.AddComponent<Rigidbody2D>();
			_r2d.gravityScale = 0;
			_r2d.constraints = RigidbodyConstraints2D.FreezeAll;
			_r2d.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
			_r2d.sleepMode = RigidbodySleepMode2D.NeverSleep;

			_r2d.bodyType = RigidbodyType2D.Dynamic;
			_r2d.isKinematic = true;
		}
	}
}
