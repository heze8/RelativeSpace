using System;
using System.Collections;
using System.Collections.Generic;
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
    [Range(-3,3)]
    public double tMax = 1f;
    public double x;
    public double y;
    [Range(0,1000)]
    public float scale;
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
	    this.xParams.xy = xy;
	    this.xParams.xt = xt;
	    this.xParams.yt = yt;
	    this.xParams.x2 = x2;
	    this.xParams.y2 = y2;
	    this.xParams.t2 = t2;
	    this.yParams.xy = xy;
	    this.yParams.xt = xt;
	    this.yParams.yt = yt;
	    this.yParams.x2 = x2;
	    this.yParams.y2 = y2;
	    this.yParams.t2 = t2;
	    StartCoroutine(GraphEquation());
    }

    public void Start()
    {
	    emitter = GetComponent<ParticleSystem>();
	   
	    StartCoroutine(Randomize());
    }

    public IEnumerator Randomize()
    {
	    yield return GraphEquation();
	    List<int> rands= new List<int>();
	    for (int i = 0; i < 6; i++)
	    {
		    rands.Add(Random.Range(-1, 2));
	    }
	    CreateChaosEquation(rands[0], rands[1], rands[2], rands[3], rands[4], rands[5]);
	    StartCoroutine(Randomize());
    }

    public bool OutOfFrame(double x, double y)
    {
	    if (double.IsNaN(x) || double.IsNaN(y) || x > 50 || x < -50 || y > 50 || y < - 50) return true;
	    return false;
	}

    public IEnumerator GraphEquation()
    {
	    emitter.Clear();
	    emitter.Emit(iters);
	    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[iters];
		emitter.GetParticles(particles);
		for (int i = 0; i < iters; i++)
		{
			particles[i].startColor =  Random.ColorHSV();
		}

	    while (t < tMax)
	    {
		    x = t;
		    y = t;
		    int numInView = 1;

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
			    
			    if (OutOfFrame(x, y))
			    {
				    particles[i].position = new Vector3(1000f, 1000f, 0);
			    }
			    else
			    {
				    numInView++;
				    particles[i].position = new Vector3((float) x, (float) y, 0) * scale;
			    }

		    }
			emitter.SetParticles(particles);
		    var inView = (0.1 + 10 / numInView);
		    Time.timeScale = (float) inView;
		    // y = yParams.xParam + yParams.yParam + yParams.tParam + yParams.xy + yParams.xt + yParams.yt + yParams.x2 + yParams.y2 + yParams.t2;
		    t += tStep * inView;
		    yield return new WaitForSeconds(tStep);
	    }
	    t = -1f;
    }
}