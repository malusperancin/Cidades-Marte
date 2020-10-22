using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace apCaminhosMarte
{
    class GrafoBacktracking
    {
        CaminhoEntreCidades[,] matriz;
        int qtasCidades;

        public GrafoBacktracking(string nomeArquivo)
        {
            // instancia-se leitor de arquivos com o nome do arquivo passado por parâmetro
            var arquivo = new StreamReader(nomeArquivo);
            qtasCidades = 23; // nº de cidades do arquivo
            matriz = new CaminhoEntreCidades[qtasCidades, qtasCidades]; // matriz [23,23]
            while(!arquivo.EndOfStream) // enquanto puder ler o arquivo
            {
                string linha = arquivo.ReadLine(); // lê uma linha por vez
                CaminhoEntreCidades caminho = new CaminhoEntreCidades(int.Parse(linha.Substring(0, 3)), int.Parse(linha.Substring(3, 3)), int.Parse(linha.Substring(6, 5)), int.Parse(linha.Substring(11, 4)), int.Parse(linha.Substring(15)));
                matriz[caminho.IdCidadeOrigem, caminho.IdCidadeDestino] = caminho; // add o caminho lido na matriz, com a linha = idCidadeOrigem e coluna = idCidadeDestino
            }
            arquivo.Close();
        }

        public PilhaLista<CaminhoEntreCidades> BuscarCaminho(int origem, int destino)
        {
            int cidadeAtual, saidaAtual;
            bool achouCaminho = false, naoTemSaida = false;

            bool[] passou = new bool[qtasCidades];

            for (int indice = 0; indice < qtasCidades; indice++)    // inicia os valores de “passou” 
                passou[indice] = false;                             // pois ainda não foi em nenhuma cidade

            cidadeAtual = origem;
            saidaAtual = 0;

            var pilha = new PilhaLista<CaminhoEntreCidades>();

            while (!achouCaminho && !naoTemSaida)
            {
                naoTemSaida = (cidadeAtual == origem && saidaAtual == qtasCidades && pilha.EstaVazia);
                if (!naoTemSaida)
                {
                    while ((saidaAtual < qtasCidades) && !achouCaminho)
                    {
                        if (matriz[cidadeAtual, saidaAtual] == null)
                            saidaAtual++;
                        else // há caminho da atual pra saida
                            if (passou[saidaAtual])
                            saidaAtual++;
                            else // há caminho e ainda n passou por ela
                            {
                                pilha.Empilhar(matriz[cidadeAtual, saidaAtual]);

                                if (saidaAtual == destino)
                                    achouCaminho = true;
                                else
                                {
                                    passou[cidadeAtual] = true;
                                    cidadeAtual = saidaAtual;   // muda para a nova cidade 
                                    saidaAtual = 0;            // reinicia busca de saídas da nova cidade 
                                }
                            }
                    }
                }
                if (!achouCaminho)
                    if (!pilha.EstaVazia)
                    {
                        var movim = pilha.Desempilhar();
                        saidaAtual = movim.IdCidadeDestino+1; // a cidade destino anterior não presta, ent vamos para a próxima
                        cidadeAtual = movim.IdCidadeOrigem;
                        passou[movim.IdCidadeDestino] = false;
                        saidaAtual++;
                    }
            }

            return pilha.CopiaInvertida();
        }


        public List<PilhaLista<CaminhoEntreCidades>> BuscarCaminhos(int origem, int destino)
        {
            var ret = new List<PilhaLista<CaminhoEntreCidades>>(); // lista de todos os caminhos possíveis
            var pilha = new PilhaLista<CaminhoEntreCidades>(); //  caminho atual

            // variáveis auxiliares e suas inicializações
            int cidadeAtual = origem, saidaAtual = 0;
            bool achouCaminho, naoTemSaida = false;

            bool[] passou = new bool[qtasCidades];
            for (int indice = 0; indice < qtasCidades; indice++)   // inicia os valores de “passou” como false 
                passou[indice] = false;                            // pois ainda não foi em nenhuma cidade

            while (!naoTemSaida) //  enquanto houver saída
            {
                achouCaminho = false;

                while (!achouCaminho && !naoTemSaida) // enquanto não acharmos o destino
                {
                    // guarda em boolean se já passamos por todas as cidades do mapa e a pilha está vazia, isto é, não há saída
                    naoTemSaida = (cidadeAtual == origem && saidaAtual == qtasCidades && pilha.EstaVazia);
                    if (!naoTemSaida)
                    {
                        while ((saidaAtual < qtasCidades) && !achouCaminho)
                        {
                            // se não há saída pela cidade testada, verifica a próxima
                            if (matriz[cidadeAtual, saidaAtual] == null)
                                saidaAtual++;
                            else // há caminho da cidade atual pra saida
                                if (passou[saidaAtual])
                                saidaAtual++;
                            else // há caminho e ainda não passou por ela
                            {
                                pilha.Empilhar(matriz[cidadeAtual, saidaAtual]);

                                if (saidaAtual == destino) // achamos a cidade destino
                                    achouCaminho = true;
                                else
                                {
                                    passou[cidadeAtual] = true; // marca a passagem pela cidade atual
                                    cidadeAtual = saidaAtual;   // muda para a nova cidade 
                                    saidaAtual = 0;            // reinicia busca de saídas da nova cidade 
                                }
                            }
                        }
                    }
                    if (!achouCaminho)
                        if (!pilha.EstaVazia)
                        {   // desempilha a configuração atual da pilha
                            // para a pilha da lista de parâmetros
                            var movim = pilha.Desempilhar();
                            saidaAtual = movim.IdCidadeDestino + 1; // a cidade destino anterior não presta, ent vamos para a próxima
                            cidadeAtual = movim.IdCidadeOrigem;
                            passou[movim.IdCidadeDestino] = false;
                            saidaAtual++;
                        }
                }
                if (pilha == null || pilha.EstaVazia)// escolher 1 dos 2
                    break;
                ret.Add(pilha.CopiaInvertida()); // adiciona o caminho encontrado na lista de caminhos possíveis
                saidaAtual++; // setta a nova cidade a ser testada atual+1
                pilha.Desempilhar();
            }
            return ret; 
        }

        public void Exibir(DataGridView dgv)
        {
            dgv.RowCount = dgv.ColumnCount = qtasCidades;
            for (int coluna = 0; coluna < qtasCidades; coluna++)
            {
                dgv.Columns[coluna].HeaderText = coluna.ToString();
                dgv.Rows[coluna].HeaderCell.Value = coluna.ToString();
                dgv.Columns[coluna].Width = 30;
            }
            for (int linha = 0; linha < qtasCidades; linha++)
                for (int coluna = 0; coluna < qtasCidades; coluna++)
                    if (matriz[linha, coluna] != null)
                        dgv[coluna, linha].Value = matriz[linha, coluna].ToString();
        }

    }
}
