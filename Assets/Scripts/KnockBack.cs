using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class KnockBack : MonoBehaviour
{
    public float knockbackTime = 0.2f;
    public float hitDirectionForce = 0.2f;
    public float constForce = 0.2f;
    public float inputForce = 0.2f;

    private Rigidbody2D playerRb;

    private Coroutine knockbackCoroutine;

    public bool isBeingKnockedBack { get; private set; }

	public AnimationCurve knockbackForceCurve;

	private void Start()
    {
        playerRb= GetComponent<Rigidbody2D>();
    }

    IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        isBeingKnockedBack = true;

        Vector2 _hitForce;
        Vector2 _constantForce;
        Vector2 _knockbackForce;
        Vector2 _combinedForce;
		float _time = 0f;

        _constantForce = constantForceDirection * constForce;

        float _elapsedTime = 0f;
        while(_elapsedTime < knockbackTime)
        {
            //iterate the timer
            _elapsedTime += Time.deltaTime;
            _time += Time.fixedDeltaTime;

            //update hitForce
            _hitForce = hitDirection * hitDirectionForce * knockbackForceCurve.Evaluate(_time);

			//combine _hitForce ans _constantForce
			_knockbackForce = _hitForce + _constantForce;

            //combine _knockBackForce with inputForce
            if(inputDirection != 0)
            {
                _combinedForce = _knockbackForce + new Vector2(inputDirection, 0f);
            }
            else
            {
                _combinedForce = _knockbackForce;
            }

            //applyKnockback
            playerRb.linearVelocity = _combinedForce;

            yield return new WaitForFixedUpdate();
        }

        isBeingKnockedBack = false;
    }

    public void CallKnockback(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
	{
		knockbackCoroutine = StartCoroutine(KnockbackAction(hitDirection, constantForceDirection, inputDirection));

	}
}
