using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class Ranking : MonoBehaviour {

    public UnityEngine.UI.VerticalLayoutGroup verticalLayoutGroup;
    public GameObject panelRanking;
    private RectTransform rt;
    public GameObject prefab;
    public Button btnVoltar;
    public bool carregado;
    // Use this for initialization
    void Start () {
        rt = panelRanking.GetComponent<RectTransform>();
        panelRanking.SetActive(false);
        
    }
	
	// Update is called once per frame
	void Update () {
        if ( !string.IsNullOrEmpty(PassaValor.ranking))
        {
            if (!carregado)
            {
                panelRanking.SetActive(true);
                string[] split = PassaValor.ranking.Split('#');
                Text t = prefab.GetComponent<Text>();

                for (int i = 0; i < split.Length; i++)
                {
                    t.text = split[i];

                    RectTransform T = ((GameObject)Instantiate(prefab, Vector3.zero, Quaternion.identity)).GetComponent<RectTransform>();
                    T.parent = rt;
             
                }
                PassaValor.ranking = null;
                carregado = true;
            }
            else
            {
                panelRanking.SetActive(true);
                PassaValor.ranking = null;
            }
        }

    }

    public void verRanking()
    {
        if (PassaValor.socket != null)
        {

            PassaValor.getRanking();

        }



    }
    public void voltar()
    {
        
        panelRanking.SetActive(false);
    }
}
