using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlanetManager : MonoBehaviour
{
    public Transform target;
    public Camera cam;
    public Button playPauseButton;
    private UniversalGravitation uG;
    private GameObject prefab;
    static public bool isPause = true;

    void Start()
    {
        prefab = Resources.Load<GameObject>("Planet");
        uG = GameObject.Find("UniversalGravitation").GetComponent<UniversalGravitation>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.N))
        {
            AddPlanet();
        }
        if (Input.GetKeyDown(KeyCode.P)) PlayPause();
    }

    Vector3 CalcVelForOrbit(Planet parentPlanet, Planet newPlanet)
    {
        Vector3 vel = Vector3.forward * Mathf.Sqrt((float)(uG.G * parentPlanet.mass / (parentPlanet.transform.position - newPlanet.transform.position).magnitude));
        vel += parentPlanet.velocity;
        return vel;
    }

    public void PlayPause()
    {
        isPause = !isPause;
    }

    public void AddPlanet()
    {
        playPauseButton.GetComponent<ToggleIcon>().SwitchIcon();
        isPause = true;
        GameObject newPlanetObject = Instantiate(prefab, cam.transform.parent);
        // Planet parentPlanet = cam.transform.parent.parent.gameObject.GetComponent<Planet>();
        Planet parentPlanet = uG.planetObjects[0].GetComponent<Planet>();
        cam.transform.parent.SetParent(parentPlanet.transform, false);
        newPlanetObject.transform.position += Vector3.right * Random.Range(10f, 20f);
        newPlanetObject.transform.SetParent(target);
        Color newCol = new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        Material mat = newPlanetObject.GetComponent<MeshRenderer>().material;
        mat.color = newCol;
        TrailRenderer trailRenderer = newPlanetObject.transform.GetComponentInChildren<TrailRenderer>();
        trailRenderer.startColor = newCol;
        trailRenderer.endColor = newCol;
        Planet newPlanet = newPlanetObject.GetComponent<Planet>();
        newPlanet.mass = Random.Range(1, 51);
        newPlanet.velocity = CalcVelForOrbit(parentPlanet, newPlanet);
        // cam.transform.parent.SetParent(newPlanet.transform, false);
        uG.addPlanet(newPlanet);
    }

    public void Reset()
    {
        isPause = true;
        SceneManager.LoadScene(0);
    }
}
