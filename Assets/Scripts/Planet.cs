using UnityEngine;

public class Planet : MonoBehaviour
{
    public double mass = 1.0f;
    public Vector3 velocity = Vector3.zero;
    public Vector3 g = Vector3.zero;
    public Vector3[] kg, kv;
    public Vector3 v, p;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        kg = new Vector3[4]; 
        kv = new Vector3[4];
        v = Vector3.zero;
        p = Vector3.zero;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (PlanetManager.isPause) return;

        float dT = Time.deltaTime;
        //previous calculation
        //v[n + 1] = v[n] + dT * a(p[n-1]);
        //p[n + 1] = p[n] + dT * v[n+1];
#if !RK4
        velocity += dT * g;
        transform.position += dT * velocity;
#else

        //RK4
        //g = fg(t, v, p);
        //v = fv(t, v, p) = g*dT;

        //v[i+1] = v[i] + dT*g[i];
        //p[i+1] = p[i] + dT*v[i];

        //kg[0] = fg(ti, vi, pi);
        //kg[1] = fg(t+dT/2, vi+dT/2*kg[0], pi+dT/2*kv[0]);
        //kg[2] = fg(t+dT/2, vi+dT/2*kg[1], pi+dT/2*kv[1]);
        //kg[3] = fg(t+dT,   vi+ dT *kg[2], pi+ dT *kv[2]);

        //kv[0] = fv(ti, vi, pi);
        //kv[1] = fv(t+dT/2, vi+dT/2*kg[0], pi+dT/2*kv[0]);
        //kv[2] = fv(t+dT/2, vi+dT/2*kg[1], pi+dT/2*kv[1]);
        //kv[3] = fv(t+dT,   vi+ dT *kg[2], pi+ dT *kv[2]);

        //v[i+1] = v[i] + dT * sum(kg) / 6;
        //p[i+1] = p[i] + dT * sum(kv) / 6;

        //kv[0] = v;
        //kv[1] = v + kg[0] * dT / 2;
        //kv[2] = v + kg[1] * dT / 2;
        //kv[3] = v + kg[2] * dT;

        //print($"name = {name}, v = {v}, p = {p}");
        velocity += dT * (kg[0] + kg[1] * 2 + kg[2] * 2 + kg[3]) / 6;
        transform.position += dT * (kv[0] + kv[1] * 2 + kv[2] * 2 + kv[3]) / 6;

        v = velocity;
        p = transform.position;

        //velocity = v;
        //transform.position = p;
#endif
    }
}
