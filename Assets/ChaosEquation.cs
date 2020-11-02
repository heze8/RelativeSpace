using System;
using System.Collections;
using System.Drawing;
using System.Runtime.CompilerServices;
using UnityEngine;
using Color = UnityEngine.Color;
using Random = UnityEngine.Random;

///summary
///summary
public class ChaosEquation : MonoBehaviour
{
 
    #region Public Fields
	//9 step equation
	[Range(-3,3)]
    public double t;
    public double x;
    public double y;
    // x + y + t + xy + xt  + yt + x2 + y2 + t2
    [Serializable]
    public struct ChaosEquationParams
    {
	    [SerializeField] [Range(-1, 1)] public int xParam;
	    [SerializeField] [Range(-1, 1)] public int yParam;
	    [SerializeField] [Range(-1, 1)] public int tParam;
	    [SerializeField] [Range(-1, 1)] public int xy;
	    [SerializeField] [Range(-1,1)]
	    public int xt;
	    [SerializeField] [Range(-1,1)]
	    public int yt;
	    [SerializeField] [Range(-1,1)]
	    public int x2;
	    [SerializeField] [Range(-1,1)]
	    public int y2;
	    [SerializeField] [Range(-1,1)]
	    public int t2;
    }

    public ChaosEquationParams xParams;
    public ChaosEquationParams yParams;
    public float tStep = 0.01f;
    public int iters = 3;
    private ParticleSystem emitter;

    #endregion

    void CreateChaosEquation(int xy, int xt, int yt, int x2, int y2, int t2)
    {
	    // this.xy = xy;
	    // this.xt = xt;
	    // this.yt = yt;
	    // this.x2 = x2;
	    // this.y2 = y2;
	    // this.t2 = t2;
	    StartCoroutine(GraphEquation());
    }

    public void Start()
    {
	    emitter = GetComponent<ParticleSystem>();
	    StartCoroutine(GraphEquation());
	    
    }

    public IEnumerator GraphEquation()
    {
	    t = -3f;
	    emitter.Clear();
	    emitter.Emit(iters);
	    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[iters];
		emitter.GetParticles(particles);
		for (int i = 0; i < iters; i++)
		{
			particles[i].startColor =  Random.ColorHSV();
		}
	
	    while (t < 3)
	    {
		    x = t;
		    y = t;
		    for (int i = 0; i < iters; i++)
		    {
			    // x + y + t + xy + xt  + yt + x2 + y2 + t2
			    double xx = x * x;
			    double yy = y * y;
			    double tt = t * t;
			    double xy = x * y;
			    double xt = x * t;
			    double yt = y * t;
				
			    var xValue = xParams.xParam * x + xParams.yParam * y + xParams.tParam * t + xParams.xy * xy + xParams.xt * xt +
			        xParams.yt * yt + xParams.x2 * xx + xParams.y2 * yy + xParams.t2 * tt;
			    y = yParams.xParam * x + yParams.yParam * y + yParams.tParam * t + yParams.xy * xy + yParams.xt * xt +
			        yParams.yt * yt + yParams.x2 * xx + yParams.y2 * yy + yParams.t2 * tt;
			    x = xValue;
			    if (double.IsNaN(x) || double.IsNaN(y))
			    {
				    particles[i].position = new Vector3(100f, 100f, 0);
			    }
			    else
			    {
				    particles[i].position = new Vector3((float) x, (float) y, 0);
			    }

		    }
		    emitter.SetParticles(particles);

		    // y = yParams.xParam + yParams.yParam + yParams.tParam + yParams.xy + yParams.xt + yParams.yt + yParams.x2 + yParams.y2 + yParams.t2;
		    t += tStep;
		    yield return new WaitForSeconds(tStep);
	    }
    }
}