using System;
using UnityEngine;

[AddComponentMenu("Camera/Simple Smooth Mouse Look ")]
public class SmoothMouseLook : MonoBehaviour
{
	private BasicEntityStatsComponent m_EntityStats;
	private void Start()
	{
		this.targetDirection = base.transform.localRotation.eulerAngles;
		if (this.characterBody)
		{
			this.targetCharacterDirection = this.characterBody.transform.localRotation.eulerAngles;
		}
		m_EntityStats = gameObject.transform.parent.parent.GetComponent<BasicEntityStatsComponent>();
	}

	private void LateUpdate()
	{
		if (m_EntityStats.stats.isDead)
		{
			Cursor.lockState = CursorLockMode.None;
			Cursor.visible = true;
            this.enabled = false;
            return;
        }
			

		//this.sensitivity = new Vector2(this.slider.value, this.slider.value);
		this.sensitivity = new Vector2(1, 1);
		if (!FPSWalker.isPaused)
		{
			if (SmoothMouseLook.cursorLocked)
			{
				Cursor.lockState = CursorLockMode.Locked;
				Cursor.visible = false;
			}
			Quaternion quaternion = Quaternion.Euler(this.targetDirection);
			Quaternion rhs = Quaternion.Euler(this.targetCharacterDirection);
			Vector2 a = new Vector2(UnityEngine.Input.GetAxisRaw("Mouse X"), UnityEngine.Input.GetAxisRaw("Mouse Y"));
			a = Vector2.Scale(a, new Vector2(this.sensitivity.x * this.smoothing.x, this.sensitivity.y * this.smoothing.y));
			this._smoothMouse.x = Mathf.Lerp(this._smoothMouse.x, a.x, 1f / this.smoothing.x);
			this._smoothMouse.y = Mathf.Lerp(this._smoothMouse.y, a.y, 1f / this.smoothing.y);
			this._mouseAbsolute += this._smoothMouse;
			if (this.clampInDegrees.x < 360f)
			{
				this._mouseAbsolute.x = Mathf.Clamp(this._mouseAbsolute.x, -this.clampInDegrees.x * 0.5f, this.clampInDegrees.x * 0.5f);
			}
			Quaternion localRotation = Quaternion.AngleAxis(-this._mouseAbsolute.y, quaternion * Vector3.right);
			base.transform.localRotation = localRotation;
			if (this.clampInDegrees.y < 360f)
			{
				this._mouseAbsolute.y = Mathf.Clamp(this._mouseAbsolute.y, -this.clampInDegrees.y * 0.5f, this.clampInDegrees.y * 0.5f);
			}
			base.transform.localRotation *= quaternion;
			if (this.characterBody)
			{
				Quaternion localRotation2 = Quaternion.AngleAxis(this._mouseAbsolute.x, this.characterBody.transform.up);
				this.characterBody.transform.localRotation = localRotation2;
				this.characterBody.transform.localRotation *= rhs;
			}
			else
			{
				Quaternion rhs2 = Quaternion.AngleAxis(this._mouseAbsolute.x, base.transform.InverseTransformDirection(Vector3.up));
				base.transform.localRotation *= rhs2;
			}
		}
	}

	private Vector2 _mouseAbsolute;

	private Vector2 _smoothMouse;

	public Vector2 clampInDegrees = new Vector2(360f, 180f);

	public bool lockCursor;

	public Vector2 sensitivity = new Vector2(2f, 2f);

	public Vector2 smoothing = new Vector2(3f, 3f);

	public Vector2 targetDirection;

	public Vector2 targetCharacterDirection;

	public GameObject characterBody;

	//public Slider slider;

	public static bool cursorLocked = true;
}
