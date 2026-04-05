//#define RK4
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem;

public class UniversalGravitation : MonoBehaviour
{
    public GameObject[] planetObjects;
    private int num;
    private List<Planet> planets = new List<Planet>();
    public double G = 6.674; // (m3 kg−1 s−2) // 6.674×10^−11 m3⋅kg−1⋅s−2
    static public Planet heaviestPlanet;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Planet heaviestPlanet = new Planet();
        num = planetObjects.Length;
        heaviestPlanet.mass = 0;
        for (int i = 0; i < num; i++)
        {
            planets.Add(getComp<Planet>(i));
            if (planets[i].mass > heaviestPlanet.mass)
            {
                heaviestPlanet = planets[i];
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (PlanetManager.isPause) return;
#if (RK4)
        RK4_kg();
#else
        for (int i = 0; i < num; i++)
        {
            planets[i].g = Vector3.zero;
        }
        for (int i = 0; i < num - 1; i++)
        {
            for (int j = i + 1; j < num; j++)
            {
                setGrav(planets[i], planets[j]);
            }
        }
#endif

    }

    private T getComp<T>(int i) {
        return planetObjects[i].GetComponent<T>();
    }

    private double distant(Planet p1, Planet p2)
    {
        Vector3 d = p1.transform.position - p2.transform.position;
        return d.magnitude;
    }

    private double calcGrav(Planet p1, Planet p2)
    {
        double d = distant(p1, p2);
        double g = G * p1.mass * p2.mass / d / d;
        return g;
    }

    private void setGrav(Planet p1, Planet p2)
    {
        double gr = calcGrav(p1, p2);
        Vector3 g1 = p1.transform.position - p2.transform.position;
        Vector3 g2 = p2.transform.position - p1.transform.position;
        p1.g += -g1.normalized * (float)(gr / p1.mass);
        p2.g += -g2.normalized * (float)(gr / p2.mass);
    }

    public void addPlanet(Planet newPlanet)
    {
        planets.Add(newPlanet);
        num = planets.Count;
    }

    private void RK4_kg()
    {
        List<Planet> pl = new List<Planet>();
        for(int i = 0; i<planets.Count; i++)
        {
            pl.Add(planets[i]);
            pl[i].v = pl[i].velocity;
            pl[i].p = pl[i].transform.position;
        }
        float dT = Time.deltaTime;
        for (int i = 0; i < 4 ; i++) {
            RK4_fg(i, ref pl);
            for(int j = 0; j < planets.Count; j++)
            {
                //pl[j].p = pl[j].transform.position;
                planets[j].kg[i] = pl[j].g;
                switch (i)
                {
                    case 0: 
                        planets[j].kv[i] = planets[j].v;
                        pl[j].v = planets[j].v + dT*planets[j].kg[i]/2;
                        pl[j].p = planets[j].p + dT*planets[j].kv[i]/2; break;
                    case 1:
                        planets[j].kv[i] = planets[j].v + dT * planets[j].kg[i-1] / 2;
                        pl[j].v = planets[j].v + dT*planets[j].kg[i]/2;
                        pl[j].p = planets[j].p + dT*planets[j].kv[i]/2; break;
                    case 2:
                        planets[j].kv[i] = planets[j].v + dT * planets[j].kg[i-1] / 2; 
                        pl[j].v = planets[j].v + dT*planets[j].kg[i];
                        pl[j].p = planets[j].p + dT*planets[j].kv[i]; break;
                    case 3:
                        planets[j].kv[i] = planets[j].v + dT * planets[j].kg[i-1]; break;
                }
                //pl[j].transform.position = pl[j].p;
            }
        }
    }

    private void RK4_fg(int ii, ref List<Planet> pl)
    {
        for (int i = 0; i < num; i++)
        {
            pl[i].g = Vector3.zero;
        }
        for (int i = 0; i < num - 1; i++)
        {
            for (int j = i + 1; j < num; j++)
            {
                RK4_setGrav(pl[i], pl[j]);
            }
        }
    }
    private void RK4_setGrav(Planet p1, Planet p2)
    {
        double gr = RK4_calcGrav(p1, p2);
        Vector3 g1 = p1.p - p2.p;
        Vector3 g2 = p2.p - p1.p;
        p1.g += -g1.normalized * (float)(gr / p1.mass);
        p2.g += -g2.normalized * (float)(gr / p2.mass);
    }
    private double RK4_calcGrav(Planet p1, Planet p2)
    {
        double d = (p1.p - p2.p).magnitude;
        double g = G * p1.mass * p2.mass / d / d;
        return g;
    }
}
