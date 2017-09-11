using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

    public Color _color = Color.red;
    public int range = 50;
    public float _startWidth = 0.02f, _endWidth = 0.1f;
    public LineRenderer _lineRender;
    private GameObject lightCollision;
    private Light _light;
    private Vector3 lightpos;
    public ControlaPortaAnimacao cpa;
    public GameObject _porta;


    // Use this for initialization
    void Start () {

        cpa = _porta.GetComponent<ControlaPortaAnimacao>();

    }


    void procuraHit()
    {
        Vector3 lazerEnd = transform.position + ( - 1 * transform.forward * range);
        RaycastHit hit;
        if(Physics.Raycast(transform.position, - transform.forward, out hit, range))
        {
            HitBoxMapa hbm = hit.transform.GetComponent<HitBoxMapa>();
            if (hbm != null)
            {
                cpa.ativar = true;
            }
        }

    }

    // Update is called once per frame
    void Update () {
        procuraHit();

    }


    /*void AntigoLaser()
    {
            public Color _color = Color.red;
    public int range = 50;
    public float _startWidth = 0.02f, _endWidth = 0.1f;
    private GameObject lightCollision;
    private Light _light;
    private Vector3 lightpos;

    // Use this for initialization
    void Start()
    {
        lightCollision = new GameObject();
        _light = lightCollision.AddComponent<Light>();
        _light.intensity = 8;
        _light.bounceIntensity = 8;
        _light.range = range;
        _light.color = _color;
        lightpos = new Vector3(0, 0, _endWidth);

        LineRenderer _lineRender = gameObject.AddComponent<LineRenderer>();
        _lineRender.material = new Material(Shader.Find("Particles/Additive"));
        _lineRender.startColor = _color;
        _lineRender.endColor = _color;
        _lineRender.startWidth = _startWidth;
        _lineRender.endWidth = _endWidth;
        _lineRender.positionCount = 2;

    }


    void drawLazer()
    {
        Vector3 lazerEnd = transform.position + transform.forward * range;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, hit.point);
            lightCollision.transform.position = (hit.point - lightpos);
        }
        else
        {
            GetComponent<LineRenderer>().SetPosition(0, transform.position);
            GetComponent<LineRenderer>().SetPosition(1, lazerEnd);
            lightCollision.transform.position = lazerEnd;
        }
    }

    // Update is called once per frame
    void Update()
    {
        drawLazer();
    }
}*/
}
