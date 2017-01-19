using UnityEngine;
using System.Collections;

using UnityEngine;
using System;

public class JetPack : MonoBehaviour {

	private JetPackPath path;
	private double speed = 0.1, height = 5, decayStart = .75, decayAmount = .25;
	private bool jetPackOn;

	private void FixedUpdate() {
		MoveWithJetPack();
	}

	private void Update() {
		GetJetPackOn();
	}

	private void MoveWithJetPack() {
		if (jetPackOn) {
			transform.position = path.GetPosition();
			if(path.IsFinished()) {
				transform.position = path.GetEndPosition();
				jetPackOn = false;
			}
		}
	}

	private void GetJetPackOn() {
		if(!jetPackOn && Input.GetMouseButtonDown(0)) {
			InitializeJetPackSequence();
		}
	}

	private void InitializeJetPackSequence() {
		Vector3 startPosition = transform.position;
		Vector3 endPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		endPosition.z = 0;
		path = new JetPackPath(startPosition, endPosition, speed, height, decayStart, decayAmount);
		jetPackOn = true;
	}
}

public class JetPackPath {
	private Vector3 startPosition, endPosition, distance;
	private double height, width;
	private double position, speed;
	private double decayAmount, decayStart, decayWindow;

	public JetPackPath(Vector3 startPosition, Vector3 endPosition, double speed, double height, double decayStart, double decayAmount) {
		this.startPosition = startPosition;
		this.endPosition = endPosition;
		this.speed = speed;
		this.height = height;
		this.decayStart = decayStart;
		this.decayAmount = decayAmount;
		distance = endPosition - startPosition;
		width = .5;
		decayWindow = 1 - decayStart;
	}

	public Vector3 GetPosition() {
		position += GetSpeed() * Time.fixedDeltaTime;
		return startPosition + new Vector3((float)GetXPosition(), (float)GetYPosition(), (float)GetZPosition());
	}

	public double GetSpeed() {
		if(position >= decayStart) {
			double decay = position - decayWindow;
			double proportion = decay / decayWindow;
			return speed - (proportion * speed * decayAmount);
		}
		return speed;
	}

	public Vector3 GetEndPosition() {
		return endPosition;
	}

	public bool IsFinished() {
		return position >= 1.0;
	}

	private double GetXPosition() {
		return position * distance.x;
	}

	private double GetYPosition() {
		double yOffset = distance.y * position;
		double yPosition = GetSquareRoot();
		if (double.IsNaN(yPosition)) yPosition = 0;
		return yPosition + yOffset;
	}

	private double GetZPosition() {
		return position * distance.z;
	}

	private double GetSquareRoot() {
		double fraction = Squared(position - width) / Squared(width);
		double toBeSquareRooted = (1 - fraction) * Squared(height);
		return Math.Sqrt(toBeSquareRooted);
	}

	private double Squared(double value) {
		return Math.Pow(value, 2);
	}
}
