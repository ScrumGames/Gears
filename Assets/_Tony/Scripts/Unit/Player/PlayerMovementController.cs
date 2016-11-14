using UnityEngine;
using System.Collections;

namespace Unit.Player
{
	public class PlayerMovementController : MonoBehaviour 
	{
		private float _verticalInput;
		private float _horizontalInput;
		private Rigidbody _rigidBody;
		private NavMeshAgent _navMeshAgent;
		private Vector3 _movement;
		private Quaternion _mouseRotation = Quaternion.identity;

		[SerializeField] private LayerMask groundLayer;
		[SerializeField] private float _speed;
		[SerializeField] Camera _mainCamera;


		private void Awake()
		{
			_rigidBody = GetComponent<Rigidbody> ();
			_navMeshAgent = GetComponent<NavMeshAgent> ();
		}

		private void FixedUpdate()
		{
			CheckInput ();
			Move ();
			Turn ();
			CheckJump ();
		}

		private void CheckJump()
		{
			if (Input.GetButtonDown ("Jump")) {
				StartCoroutine (JumpCoroutine ());
			} 
			else
			{
				_navMeshAgent.enabled = true;
			}

		}

		private IEnumerator JumpCoroutine()
		{
			_navMeshAgent.enabled = false;
			_rigidBody.AddForce (0, 300, 0);

			yield return new WaitForSeconds (3.0f);

		}

		private void CheckInput()
		{
			_verticalInput = Input.GetAxis ("Vertical");
			_horizontalInput = Input.GetAxis ("Horizontal");


			Ray mouseRay = _mainCamera.ScreenPointToRay(Input.mousePosition);

			RaycastHit hit;

			if (Physics.Raycast(mouseRay, out hit, 100,groundLayer.value))
			{
				Vector3 diff = hit.point - transform.position;
				diff.y = 0;

				_mouseRotation = Quaternion.LookRotation(diff);
			}
		}

		private void Turn()
		{
			_rigidBody.MoveRotation(_mouseRotation);
		}

		private void Move()
		{
			_movement = new Vector3 (_horizontalInput, 0, _verticalInput);
			_movement = _movement.normalized * _speed * Time.deltaTime;

			if(_navMeshAgent.enabled)
				_navMeshAgent.Move (_movement);
		}

	}
}


