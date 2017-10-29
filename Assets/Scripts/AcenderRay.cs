using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class AcenderRay : MonoBehaviour {

    public float range = 30f;
    public Camera _camera;
    public AcenderQuadrado aq;
    public AcenderQuadrado[] arrayAq;
    public int cont;
    private bool portaAberta;
    private bool vez; //Se falso Player1, se Verdadeiro Player2
    private int contAlteracaoPainel; //Conta as chamadas do Update, apenas na primeira chamada deve acender as luzes que foram acionadas em outra sala
    private bool jogou; //permite uma jogada de cada jogador

    private void Start()
    {
        arrayAq = FindObjectsOfType<AcenderQuadrado>();

    }
    

    void ChecarMatrizCompleta()
    {
        cont = 0;
        foreach(AcenderQuadrado item in arrayAq)
        {
            if (item.ativo)
            {
                cont++;
            }
        }
        if (cont == 16)
        {
            portaAberta = true;
            PassaValor.numPorta = 3; //AbrirPorta
            Debug.Log(PassaValor.numPorta);
           
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PassaValor.vezMatriz)
        {
            if (!jogou)
            {
                if (vez && !string.IsNullOrEmpty(PassaValor.posicaoSelecionadaMatriz)) //se for o turno do jogador deve-se incrementar a qtd de chamadas
                {
                    contAlteracaoPainel++; //incrementa as chamadas
                }

                if (!portaAberta)
                {
                    ChecarMatrizCompleta();

                    if (contAlteracaoPainel == 1) //Primeira chamada, então acender luzes acionadas pelo outro jogador
                        acenderPosicaoRecebida();

                    vez = PassaValor.vezMatriz;

                    if (Input.GetButtonDown("Fire1"))
                    {

                        Aperta();
                        if (aq != null)
                        {
                            PassaValor.alterarJogadaMatriz(aq.posicao); //informa ao server a jogada realizada
                            if (vez && PassaValor.players == 1) //se for a vez do playter 1, acender colunas
                            {
                                aq._col1.Controla();
                                aq._col2.Controla();
                                aq._col3.Controla();


                            }
                            else if (vez && PassaValor.players == 2)// se for a vez do player 2, acender linhas
                            {
                                aq._lin1.Controla();
                                aq._lin2.Controla();
                                aq._lin3.Controla();


                            }

                            contAlteracaoPainel = 0; //zerar contador após apertar algum botão da matriz
                            vez = false; //alterar vez do jogador
                            jogou = true; //marca o turno do jogador
                        }
                    }

                }
            }
        }
        else
        {
            jogou = false;
        }
        
        
    }

    void acenderPosicaoRecebida()
    {
        if (vez)
        {

            if (!string.IsNullOrEmpty(PassaValor.posicaoSelecionadaMatriz))
            {

                GameObject go = GameObject.Find("Cube" + PassaValor.posicaoSelecionadaMatriz); //busca o botão que o outro jogador acionou pelo nome (jogador especifica posição do botao)
                AcenderQuadrado ac = go.GetComponent<AcenderQuadrado>(); //pega o script do botao

                /*acende ou apaga as luzes necessárias*/
                
                ac.Controla();
                
                if (vez && PassaValor.players == 2)
                {
       
                    ac._col1.Controla();
                    ac._col2.Controla();
                    ac._col3.Controla();
                    Debug.LogWarning("Acendeu: " + PassaValor.posicaoSelecionadaMatriz);
                }
                else if (vez && PassaValor.players == 1)
                {
          
                    ac._lin1.Controla();
                    ac._lin2.Controla();
                    ac._lin3.Controla();

                }
                
            }

        }
    }

    void Aperta()
    {
        if (PassaValor.vezMatriz) //só acende ou apaga se for a vez do jogador
        {
            RaycastHit hit;
            if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, range))
            {

                AcenderQuadrado ac = hit.transform.GetComponent<AcenderQuadrado>();
                aq = ac;
                if (ac != null)
                {

                    ac.Controla();
                }
            }
        }
    }

}
