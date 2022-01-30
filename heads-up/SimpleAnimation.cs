using UnityEngine;
using System.Collections;

public class SimpleAnimation : MonoBehaviour {
	public Vector3 moveOffset;
	public float moveSpeed;
    public float stopTimeAfterMove = 0;
	public Vector3 rotationAxis;
	public float rotationSpeed;

    public bool move = false, rotate = false, scale=false;

    public AnimationCurve scaleCurve;

    public enum rotationModes { continuous, pingPong };
    public enum rotationAxisModes { left, right, up, down, forward, backward, global};

    public rotationAxisModes rotationAxisType = rotationAxisModes.global;

    
    public rotationModes RotationMode = rotationModes.continuous;
    public float aperture = 10;

    public bool startDelay = false;
    public float delay = 0.5f;

    

   // protected float delay=0;
    protected Vector3 moveDir;

    protected Vector3 initialScale;
    protected bool scaleSet = false;

    public bool overrideInitialScale = false;
    public Vector3 initialScaleOverride = Vector3.one;

    public bool pingPongScale = false;
    public float pingPongScaleDuration = 1;

    public bool timescaleIndependent = false;

    public bool reset = true;



    protected void launch()
    {
        StopAllCoroutines();

        if (startDelay)
        {
            //delay = Random.Range(0, delay);

        }


        if (move)
        {
            StartCoroutine(movementAlt());
        }

        if (rotate)
        {
            switch (RotationMode)
            {
                case rotationModes.continuous:
                    StartCoroutine(rotation());
                    break;
                case rotationModes.pingPong:
                    StartCoroutine(rotationPingPong());
                    break;
            }
        }

        if (scale)
        {
            if (!scaleSet)
            {

                initialScale = overrideInitialScale ? initialScaleOverride : transform.localScale;
                scaleSet = true;
            }


            if (pingPongScale)
            {
                StartCoroutine(scaleFromCurvePingPong());
            }
            else
            {
                StartCoroutine(scaleFromCurve());
            }

        }
    }

    

    void OnEnable()
	{
		launch ();
	}

    protected IEnumerator scaleFromCurve()
    {
        yield return new WaitForSeconds(delay);
        var accumTime = 0.0f;
        var maxTime = scaleCurve.keys[scaleCurve.keys.Length - 1].time;

        

        while (accumTime <= maxTime)
        {
            accumTime += GetElapsedTime();
            transform.localScale = initialScale * scaleCurve.Evaluate(accumTime);
            yield return null;
        }
    }


    protected IEnumerator scaleFromCurvePingPong()
    {
        yield return new WaitForSeconds(delay);
        var accumTime = 0.0f;
        var maxTime = scaleCurve.keys[scaleCurve.keys.Length - 1].time * pingPongScaleDuration;



        while (true)
        {
            accumTime += GetElapsedTime();
            transform.localScale = initialScale * scaleCurve.Evaluate(Mathf.Repeat(accumTime,maxTime));
            yield return null;
        }
    }

    private IEnumerator rotationPingPong() 
    {
        yield return new WaitForSeconds(delay);


        var nextRotationSign = 1.0f;
        var nextRotationAmount = aperture;
        while (true)
        {
            var rotationAmountDone=0.0f;
            while (Mathf.Abs(rotationAmountDone) < Mathf.Abs(nextRotationAmount)) 
            {
                var rotate = rotationSpeed * nextRotationSign * GetElapsedTime();
                
                rotationAmountDone += rotate;

                var euler = transform.localRotation.eulerAngles;
                euler.x += rotate * rotationAxis.x;
                euler.y += rotate * rotationAxis.y;
                euler.z += rotate * rotationAxis.z;

                transform.localRotation = Quaternion.Euler(euler);

               // transform.RotateAround(transform.position, rotationAxis, rotate);
                yield return null;
            }
           
            nextRotationSign *= -1;
            nextRotationAmount = aperture * nextRotationSign * 2;
        }
    }

	private IEnumerator rotation()
	{
        if (timescaleIndependent)
        {
            yield return new WaitForSecondsRealtime(delay);
        }
        else
        {
            yield return new WaitForSeconds(delay);
        }
        
		while(true)
		{
			transform.RotateAround(transform.position,getRotationAxis(),rotationSpeed*GetElapsedTime());
			yield return null;
		}
	}

    protected Vector3 getRotationAxis()
    {
       
        switch (rotationAxisType)
        {
            case rotationAxisModes.left:
                return transform.right * -1;

            case rotationAxisModes.right:
                return transform.right;
    
            case rotationAxisModes.up:
                return transform.up;
               
            case rotationAxisModes.down:
                return transform.up * -1;
             
            case rotationAxisModes.forward:
                return transform.forward;
            case rotationAxisModes.backward:
                return transform.forward * -1;
            case rotationAxisModes.global:
            default:
                return rotationAxis;
        }
    }

    public bool smoothMove = false;
    public AnimationCurve speedCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    private IEnumerator movementAlt()
    {
        yield return null;

        yield return new WaitForSeconds(delay);

      
        moveDir = moveOffset.normalized;

        float moveLength = 0.0f;
        float sign = 1;
        while (true)
        {

            var speed = moveSpeed;
            if (smoothMove)
            {
                
                    speed = moveSpeed * speedCurve.Evaluate(moveLength/moveOffset.magnitude);
                
                    
            }
           
            var move = moveDir * (sign * speed * GetElapsedTime());

            transform.position += move;


            moveLength += move.magnitude;
            
            if (moveLength >= moveOffset.magnitude)
            {
                moveLength = 0;

                sign *= -1;
                yield return new WaitForSeconds(stopTimeAfterMove);
            }

            yield return null;
        }
    }


	private IEnumerator movement()
	{
        yield return new WaitForSeconds(delay);

        var origin = transform.localPosition;
		var target = transform.localPosition + moveOffset;

        

		while(true){
			var distance = Vector3.Distance(transform.localPosition, target);
			var seconds = distance/moveSpeed;
			var accumTime = 0.0f;
			while(accumTime<=1)
			{
				accumTime+=GetElapsedTime()/seconds;
				transform.localPosition = Vector3.Lerp(origin,target,Mathf.SmoothStep(0,1,accumTime));
				yield return null;
			}
			var temp = target;
			target = origin;
			origin = temp;
		}
	}

	// Update is called once per frame
	void Update () 
	{

	}

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, transform.position + moveOffset);
        Gizmos.DrawWireSphere(transform.position, 0.3f);
        Gizmos.DrawWireSphere(transform.position+moveOffset, 0.3f);
    }

    private float GetElapsedTime()
    {
        return timescaleIndependent ? Time.unscaledDeltaTime : Time.deltaTime;
    }
}
