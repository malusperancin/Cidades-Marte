/* Autoras:                    20/10/2020
 * Giovanna Pavani Martelli       19173
 * Maria Luiza Sperancin Mancebo  19186
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;

namespace apCaminhosMarte
{
    public partial class FrmCaminhosMarte : Form
    {
        /*objetos globais das classes utilizadas no form*/
        ArvoreDeBusca<Cidades> arvoreCidades;
        GrafoBacktracking grafoCidades; // objeto de leitura do grafo e busca de caminhos
        List<PilhaLista<CaminhoEntreCidades>> listaCaminhos;
        PilhaLista<CaminhoEntreCidades> menorCaminho;

        // ids das cidades origem e destino escolhidas pelo usuario
        int idOrigem, idDestino;
        // tela p/ desenhar as cidades e suas rotas sobre o mapa das cidades
        Graphics g;

        public FrmCaminhosMarte()
        {
            InitializeComponent();
        }

        private void FrmCaminhos_Load(object sender, EventArgs e)
        {  
            arvoreCidades = new ArvoreDeBusca<Cidades>();

            // leitura do arquivo de cidades e armazenamento na arvoreCidades
            dlgAbrir.Title = "Escolha o arquivo das Cidades de Marte";
            if (dlgAbrir.ShowDialog() == DialogResult.OK)  // abre explorador de arquivos p/ usuario escolher arquivo
            {
                StreamReader arq = new StreamReader(dlgAbrir.FileName);
                while(!arq.EndOfStream) // enquanto não estiver no fim do arquivo
                { 
                    string linha = arq.ReadLine(); // le uma linha por vez
                    Cidades cidade = new Cidades(int.Parse(linha.Substring(0, 3)),linha.Substring(3, 15),int.Parse(linha.Substring(18, 5)), int.Parse(linha.Substring(24)));
                    arvoreCidades.Incluir(cidade); // add na arvore
                }
                arq.Close();
            }

            // leitura do arquivo de cidades e armazenamento na arvoreCidades
            dlgAbrir.Title = "Escolha o arquivo dos Caminhos entre as Cidades de Marte";
            if (dlgAbrir.ShowDialog() == DialogResult.OK) // abre explorador de arquivos p/ usuario escolher arquivo
                grafoCidades = new GrafoBacktracking(dlgAbrir.FileName);

            // exibe as cidades e seus ids nos listBoxes 
            arvoreCidades.ExibirLsb(lsbDestino);
            arvoreCidades.ExibirLsb(lsbOrigem);

            // cria grafico para desenhar sobre o picture box com o mapa
            g = Graphics.FromImage(pbMapa.Image);
            arvoreCidades.DesenharMapa(arvoreCidades.Raiz, g);
            // coloca os pontos de cada cidade
        }

        private void lsbOrigem_SelectedIndexChanged(object sender, EventArgs e)
        {
            // guarda o id da cidade origem escolhida
            idOrigem = lsbOrigem.SelectedIndex;
        }

        private void lsbDestino_SelectedIndexChanged(object sender, EventArgs e)
        {
            // guarda o id da cidade destino escolhida
            idDestino = lsbDestino.SelectedIndex;
        }       

        private void BtnBuscar_Click(object sender, EventArgs e)
        {
            if (idOrigem == null || idDestino == null) // usuário não selecionou as cidades
            {
                MessageBox.Show("Selecione as cidades origem e destino!", "ERRO");
                LimparDgvs();
                return;
            }
            else
            if (idOrigem == idDestino) // usuário selecionou cidades iguais
            {
                MessageBox.Show("Selecione origem e destino diferentes!", "ERRO");
                LimparDgvs();
                return;
            }
            else // usuario selecionou cidades válidas
            {
                listaCaminhos = grafoCidades.BuscarCaminhos(idOrigem, idDestino);
                // retorna lista de caminhos possiveis entre as cidades selecionadas

                if (listaCaminhos.Count == 0) // não encontrou caminho
                {
                    MessageBox.Show("Nenhum caminho foi encontrado!", "ERRO");
                    return; // para execução
                }     
                else
                {
                    // copia da lista de caminhos p/ manipulá-la sem estragar os dados, os quais serão utilizados posteriormente
                    List<PilhaLista<CaminhoEntreCidades>> cListaCaminhos = new List<PilhaLista<CaminhoEntreCidades>>();
                    for (int i = 0; i < listaCaminhos.Count; i++)
                     cListaCaminhos.Add(listaCaminhos[i].Copia());

                    // limpa o mapa e os dgvs
                    LimparDgvs();

                    // nº linhas = nº caminhos encontrados
                    int linhas = cListaCaminhos.Count;
                    dgvCaminhos.RowCount = linhas;
                    // nº colunas = 10 (maior caminho possivel cabe)
                    dgvCaminhos.ColumnCount = 10; 
                    // linha única
                    dgvMenorCaminho.RowCount = 1;
                    //setta largura das celúlas do dgv com 90
                    for (int col = 0; col < 10; col++)
                        dgvCaminhos.Columns[col].Width = 90;

                    // variáveis auxiliares
                    int menorDistancia = 0;
                    string cidadeFinal = null;

                    for (int lin = 0; lin < linhas; lin++) // percorre a lista de caminhos
                    {
                        PilhaLista<CaminhoEntreCidades> pilhaCam = cListaCaminhos[lin]; // pega o lin-ésimo caminho
                        PilhaLista<CaminhoEntreCidades> copia = pilhaCam.Copia(); // faz-se cópia para não entragar os dados que serão utilizados posteriormente

                        int distancia = 0;
                        int col;
                        while(!copia.EstaVazia) // soma a distancia total do caminho atual
                        {
                            distancia += copia.OTopo().Distancia;
                            copia.Desempilhar();
                        }

                        if (menorDistancia == 0 || distancia < menorDistancia) // verifica se este é o menor caminho até então
                        {
                            menorDistancia = distancia;
                            menorCaminho = pilhaCam.Copia(); 
                        }

                        for (col = 0; !pilhaCam.EstaVazia; col++) // percorre cada passo do caminho e exibe no dgv
                        {
                            string cidade = arvoreCidades.GetDado(new Cidades(pilhaCam.OTopo().IdCidadeOrigem)).NomeCidade; // exibe apenas a cidade origem
                            dgvCaminhos[col, lin].Value = cidade;
                            pilhaCam.Desempilhar();
                        }
                        // e no final exibe-se a o nome da cidade destino
                        cidadeFinal = arvoreCidades.GetDado(new Cidades(idDestino)).NomeCidade;
                        dgvCaminhos[col, lin].Value = cidadeFinal;
                    }

                    // tira a seleção nos dgvs
                    dgvMenorCaminho.ClearSelection();
                    dgvCaminhos.ClearSelection();

                    // setta nº e tamanho das células do dgv do menor caminho
                    int colunas = menorCaminho.QuantosNos;
                    dgvMenorCaminho.ColumnCount = colunas + 1;
                    for (int col = 0; col < colunas + 1; col++)
                        dgvMenorCaminho.Columns[col].Width = 90;

                    // exibe o menor caminho no dgv, cidade por cidade
                    PilhaLista<CaminhoEntreCidades> copiaMenor = menorCaminho.Copia();
                    for (int col = 0; !copiaMenor.EstaVazia; col++)
                    {
                        string cidade = arvoreCidades.GetDado(new Cidades(copiaMenor.OTopo().IdCidadeOrigem)).NomeCidade;
                        dgvMenorCaminho[col, 0].Value = cidade;
                        copiaMenor.Desempilhar();
                    }
                    // e, por fim, a cidade destino
                    dgvMenorCaminho[colunas, 0].Value = cidadeFinal;
                }
            }
        }

        public void LimparDgvs() // limpa o mapa e zera todos os valores do dgvs, deixando-os com apenas 1 linha vazia
        {
            // limpa mapa
            pbMapa.Refresh();
            g.Dispose();

            // zera dgvs
            dgvCaminhos.RowCount = dgvMenorCaminho.RowCount = 0;
        }

        private void dgvCaminhos_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // ao selecionar uma linha do dgv (um caminho)
            dgvMenorCaminho.ClearSelection();
            int idCaminho = e.RowIndex; // index da linha selecionada
            if (listaCaminhos[0] != null)
            {
                PilhaLista<CaminhoEntreCidades> caminho = listaCaminhos[idCaminho].Copia(); // faz-se cópia para não estragar
                DesenharCaminho(caminho); // desenhar o caminho no mapa
            }
        }

        private void dgvMenorCaminho_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // ao selecionar a linha do menor caminho
            dgvCaminhos.ClearSelection();
            if(menorCaminho != null)
            {
                PilhaLista<CaminhoEntreCidades> caminho = menorCaminho.Copia(); // faz-se cópia para não estragar
                DesenharCaminho(caminho); // desenhar o menor caminho no mapa
            }
        }

        private void DesenharCaminho(PilhaLista<CaminhoEntreCidades> caminho)
        {
            // cria um novo gráfico sobre o pictureBox
            g = pbMapa.CreateGraphics();
            //redesenha as cidades nas coordenadas corretas
            arvoreCidades.DesenharMapa(arvoreCidades.Raiz, g);
            // atualiza
            pbMapa.Refresh();
            while (!caminho.EstaVazia) // percorre o caminho
            {
                var atual = caminho.OTopo();
                // resgata e instancia as cidades de origem e destino de cada passo
                Cidades origem = arvoreCidades.GetDado(new Cidades(atual.IdCidadeOrigem));
                Cidades destino = arvoreCidades.GetDado(new Cidades(atual.IdCidadeDestino));
                // instancia-se caneta para desenhar as linhas
                Pen pen = new Pen(Color.DarkRed, 2); // cor: vermelho escuro - espessura = 2px
                pen.StartCap = LineCap.RoundAnchor; // bolinha no início da seta (origem)
                pen.EndCap = LineCap.ArrowAnchor;  // seta no fim da reta (destino)
                //pontos de coordenadas da origem e do destino
                PointF pOrigem = new PointF(origem.CoordenadaX/4 + 5, origem.CoordenadaY/4 + 5);
                PointF pDestino = new PointF(destino.CoordenadaX / 4 + 5, destino.CoordenadaY/4 + 5);
                g.DrawLine(pen, pOrigem,pDestino); // desenha a linha
                caminho.Desempilhar();
            }
            // atualiza graphic
            g.Dispose();
        }

        private void tbAbas_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ao mudar o tab
            // desenha-se a árvore de cidades lidas e armazenadas
            arvoreCidades.DesenharArvore(true, arvoreCidades.Raiz, 500, 0, Math.PI / 2,
            Math.PI / 2.5, 280, pnlArvore.CreateGraphics());
        }

        private void pbMapa_Paint(object sender, PaintEventArgs e)
        {
            // atualiza o graphic
            e.Dispose();
        }
    }
}
