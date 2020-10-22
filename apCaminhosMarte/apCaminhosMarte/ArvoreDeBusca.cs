using System;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Threading;

namespace apCaminhosMarte
{
    class ArvoreDeBusca<Dado>  // : IComparable<NoArvore<Dado>> 
                                where Dado : IComparable<Dado>, IRegistro, new()
    {
        NoArvore<Dado> raiz,
                       atual,       // indica o nó que está sendo visitado no momento
                       antecessor;  // indica o nó ancestral daquele que está sendo visitado no momento
        int quantosNos;

        public Panel painelArvore;

        public Panel OndeExibir
        {
            get { return painelArvore; }
            set { painelArvore = value; }
        }

        public ArvoreDeBusca()
        {
            raiz = null;
            atual = null;
            antecessor = null;
            quantosNos = 0;
        }

        public NoArvore<Dado> Raiz
        {
            get => raiz; set => raiz = value;
        }

        public String InOrdem  // propriedade que gera a string do percurso in-ordem da árvore
        {
            get { return FazInOrdem(raiz); }
        }

        public String PreOrdem  // propriedade que gera a string do percurso pre-ordem da árvore
        {
            get { return FazPreOrdem(raiz); }
        }

        public String PosOrdem  // propriedade que gera a string do percurso pos-ordem da árvore
        {
            get { return FazPosOrdem(raiz); }
        }

        private String FazInOrdem(NoArvore<Dado> r)
        {
            if (r == null)
                return "";  // retorna cadeia vazia

            return FazInOrdem(r.Esq) + " " +
                   r.Info.ToString() + " " +
                   FazInOrdem(r.Dir);
        }

        public void ExibirLsb(ListBox lsb)
        {
            Exibir(raiz, lsb);

            void Exibir(NoArvore<Dado> r, ListBox list)
            {
                if (r != null)
                {
                    if (r.Esq != null)
                    {
                        Exibir(r.Esq, list);
                        lsb.Items.Add(r.Info.ToString());
                    }
                    else
                        lsb.Items.Add(r.Info.ToString());
                    Exibir(r.Dir, list);
                }

            }

        }

        private String FazPreOrdem(NoArvore<Dado> r)
        {
            if (r == null)
                return "";  // retorna cadeia vazia

            return r.Info.ToString() + " " +
                   FazPreOrdem(r.Esq) + " " +
                   FazPreOrdem(r.Dir);
        }

        private String FazPosOrdem(NoArvore<Dado> r)
        {
            if (r == null)
                return "";  // retorna cadeia vazia

            return FazPosOrdem(r.Esq) + " " +
                   FazPosOrdem(r.Dir) + " " +
                   r.Info.ToString();
        }

        // Exercícios iniciais

        private bool Eq(NoArvore<Dado> atualA,
                        NoArvore<Dado> atualB)
        {
            if (atualA == null && atualB == null)
                return true;

            if ((atualA == null) != (atualB == null)) // apenas um dos nós é
                return false; // uma é nulo e outra não é

            // os dois nós não são nulos
            if (atualA.Info.CompareTo(atualB.Info) != 0)
                return false; // Infos diferentes

            return Eq(atualA.Esq, atualB.Esq) &&
                   Eq(atualA.Dir, atualB.Dir);
        }

        public bool EquivaleA(ArvoreDeBusca<Dado> outraArvore)
        {
            /* . ambas são vazias
            ou
            .. Info(A) = Info(B) e
            ... Esq(A) eq Esq(B) e Dir(A) eq Dir(B)
            */
            return Eq(this.raiz, outraArvore.raiz);
        }

        public int QtosNos(NoArvore<Dado> noAtual)
        {
            if (noAtual == null)  // árvore vazia ou descendente de folha
                return 0;

            return 1 +                 // conta o nó atual
                QtosNos(noAtual.Esq) + // conta nós da subárvore esquerda
                QtosNos(noAtual.Dir);  // conta nós da subárvore direita
        }

        public int QtasFolhas(NoArvore<Dado> noAtual)
        {
            if (noAtual == null)
                return 0;
            if (noAtual.Esq == null && noAtual.Dir == null) // noAtual é folha
                return 1;
            // noAtual não é folha, portanto procuramos as folhas de cada ramo e as contamos
            return QtasFolhas(noAtual.Esq) + // conta folhas da subárvore esquerda
            QtasFolhas(noAtual.Dir); // conta folhas da subárvore direita
        }

        public bool EstritamenteBinaria(NoArvore<Dado> noAtual)
        {
            if (noAtual == null)
                return true;
            // noAtual não é nulo
            if (noAtual.Esq == null && noAtual.Dir == null)
                return true;
            // um dos descendentes é nulo e o outro não é
            if (noAtual.Esq == null && noAtual.Dir != null)
                return false;
            if (noAtual.Esq != null && noAtual.Dir == null)
                return false;
            // se chegamos aqui, nenhum dos descendentes é nulo, dai testamos a
            // "estrita binariedade" das duas subárvores descendentes do nó atual
            return EstritamenteBinaria(noAtual.Esq) && EstritamenteBinaria(noAtual.Dir);
        }

        public int Altura(NoArvore<Dado> noAtual)
        {
            int alturaEsquerda,
                alturaDireita;
            if (noAtual == null)
                return 0;
            alturaEsquerda = Altura(noAtual.Esq);
            alturaDireita = Altura(noAtual.Dir);
            if (alturaEsquerda >= alturaDireita)
                return 1 + alturaEsquerda;
            return 1 + alturaDireita;
        }

        public string EntreParenteses(NoArvore<Dado> noAtual)
        {
            string saida = "(";
            if (noAtual != null)
                saida += noAtual.Info + ":" +
                EntreParenteses(noAtual.Esq) +
                "," +
                EntreParenteses(noAtual.Dir);
            saida += ")";
            return saida;
        }

        public void Trocar(NoArvore<Dado> noAtual)
        {
            if (noAtual != null)
            {
                NoArvore<Dado> auxiliar = noAtual.Esq;
                noAtual.Esq = noAtual.Dir;
                noAtual.Dir = auxiliar;
                Trocar(noAtual.Esq);
                Trocar(noAtual.Dir);
            }
        }

        public string PercursoPorNiveis(NoArvore<Dado> noAtual)
        {
            string saida = "";
            //Filalista<NoArvore<Dado>> umaFila = new FilaLista<NoArvore<Dado>>();
            var umaFila = new Queue<NoArvore<Dado>>();
            while (noAtual != null)
            {
                if (noAtual.Esq != null)
                    umaFila.Enqueue(noAtual.Esq);
                if (noAtual.Dir != null)
                    umaFila.Enqueue(noAtual.Dir);
                saida += " " + noAtual.Info;
                if (umaFila.Count == 0)
                    noAtual = null;
                else
                    noAtual = umaFila.Dequeue();
            }
            return saida;
        }

        int[] quantosNoNivel = new int[1000]; // GLOBAL NA CLASSE
        public int largura(NoArvore<Dado> noAtual)
        {
            for (int i = 0; i < 1000; i++)
                quantosNoNivel[i] = 0;
            ContarNosNosNiveis(noAtual, 0);
            // acha o nível com o maior contador de nós
            int indiceMaior = 0;
            for (int i = 0; i < 1000; i++)
                if (quantosNoNivel[i] > quantosNoNivel[indiceMaior])
                    indiceMaior = i;
            return quantosNoNivel[indiceMaior];
        }
        public void ContarNosNosNiveis(NoArvore<Dado> noAtual, int qualNivel)
        {
            if (noAtual != null)
            {
                ++quantosNoNivel[qualNivel];
                ContarNosNosNiveis(noAtual.Esq, qualNivel + 1);
                ContarNosNosNiveis(noAtual.Dir, qualNivel + 1);
            }
        }

        bool achou = false;
        private string EscreverAntecessores(NoArvore<Dado> atual, Dado procurado)
        {
            string saida = "";
            if (atual != null)
            {
                if (!achou)
                    EscreverAntecessores(atual.Esq, procurado);
                if (!achou)
                    EscreverAntecessores(atual.Dir, procurado);
                if (atual.Info.CompareTo(procurado) == 0)
                    achou = true;
                if (achou)
                    saida += " " + atual.Info;
            }
            return saida;
        }
        public string PreparaEscritaDosAntecessores(Dado procurado)
        {
            achou = false;
            return EscreverAntecessores(Raiz, procurado);
        }
        public void DesenharArvore(bool primeiraVez, NoArvore<Dado> raiz,
                                  int x, int y, double angulo, double incremento,
                                  double comprimento, Graphics g)
        {
            int xf, yf;
            if (raiz != null)
            {
                Pen caneta = new Pen(Color.Red);
                xf = (int)Math.Round(x + Math.Cos(angulo) * comprimento);
                yf = (int)Math.Round(y + Math.Sin(angulo) * comprimento);
                if (primeiraVez)
                    yf = 25;
                g.DrawLine(caneta, x, y, xf, yf);
                Thread.Sleep(100);
                DesenharArvore(false, raiz.Esq, xf, yf, Math.PI / 2 + incremento,
                                       incremento * 0.60, comprimento * 0.8, g);
                DesenharArvore(false, raiz.Dir, xf, yf, Math.PI / 2 - incremento,
                                        incremento * 0.60, comprimento * 0.8, g);
                Thread.Sleep(100);
                SolidBrush preenchimento = new SolidBrush(Color.LightBlue);
                g.FillEllipse(preenchimento, xf - 25, yf - 15, 42, 30);
                g.DrawString(Convert.ToString(raiz.Info.ToString()), new Font("Comic Sans", 10),
                    new SolidBrush(Color.Black), xf - 23, yf - 7);

            }
        }

        public  void DesenharMapa(NoArvore<Cidades> raiz, Graphics g)
        {
            if (raiz != null)
            {
                if (raiz.Esq != null) // verifica se há mais nós à esquerda
                {
                    DesenharMapa(raiz.Esq, g); // vai para o nó esquerdo

                    // desenha elipse com as coordenadas das cidades
                    SolidBrush preenchimento = new SolidBrush(Color.Black);
                    g.FillEllipse(preenchimento, raiz.Info.CoordenadaX / 4, raiz.Info.CoordenadaY / 4, 10, 10);
                    // desenha a escrita com os nomes das cidades
                    g.DrawString(Convert.ToString(raiz.Info.NomeCidade), new Font("Comic Sans", 10),
                        new SolidBrush(Color.Black), raiz.Info.CoordenadaX / 4 - 15, raiz.Info.CoordenadaY / 4 - 15);
                }
                else // se não houver mais nós à esquerda
                {
                    SolidBrush preenchimento = new SolidBrush(Color.Black);
                    g.FillEllipse(preenchimento, raiz.Info.CoordenadaX/4, raiz.Info.CoordenadaY/4 , 10, 10);
                    g.DrawString(Convert.ToString(raiz.Info.NomeCidade), new Font("Comic Sans", 10),
                        new SolidBrush(Color.Black), raiz.Info.CoordenadaX/4 - 15, raiz.Info.CoordenadaY/4 - 15);
                }
                DesenharMapa(raiz.Dir, g); // vai para o nó direito
            }
        }
        

        private bool Existe(NoArvore<Dado> local, Dado procurado)
        {
            antecessor = null;
            while (local != null)
            {
                if (local.Info.CompareTo(procurado) == 0)
                {
                    atual = local;
                    return true;
                }
                else
                {
                    antecessor = local;
                    if (procurado.CompareTo(local.Info) < 0)
                        local = local.Esq;     // Desloca apontador para o ramo à esquerda
                    else
                        local = local.Dir;     // Desloca apontador para o ramo à direita
                }
            }
            return false;       // Se local == null, a chave não existe
        }

        public bool Existe(Dado procurado)
        {
            return Existe(raiz, procurado);
        }

        public bool ExisteRec(NoArvore<Dado> local, Dado procurado)
        {
            if (local == null)
                return false;
            else
              if (local.Info.CompareTo(procurado) == 0)
            {
                atual = local;
                return true;
            }
            else
            {
                antecessor = local;
                if (procurado.CompareTo(local.Info) < 0)
                    return ExisteRec(local.Esq, procurado);	 // Desloca apontador na 
                else                                         // próxima instância do 
                    return ExisteRec(local.Dir, procurado);	 // método
            }
        }

        public Dado GetDado(Dado dadoProcurado) // retorna dado passado por parâmetro
        {
            return GetDado(raiz, dadoProcurado);

            Dado GetDado(NoArvore<Dado> local, Dado procurado)
            {
                if (local == null) //  se não encontrar, retorna default (null)
                    return default(Dado);
                else
                  if (local.Info.CompareTo(procurado) == 0) //  se a informação for encontrada
                    {
                        //retorna informação
                        return local.Info;
                    }
                else
                {
                    if (procurado.CompareTo(local.Info) < 0) // se a informação procurada é menor
                        return GetDado(local.Esq, procurado);	 // vai para o nó esquerdo
                    else                                         
                        return GetDado(local.Dir, procurado);	 // vai para o nó direito
                }
            }
        }

        public void Incluir(Dado dadoLido)
        {
            Incluir(ref raiz, dadoLido);
        }


        private void Incluir(ref NoArvore<Dado> atual, Dado dadoLido)
        {
            if (atual == null)
            {
                atual = new NoArvore<Dado>(dadoLido);
            }
            else
              if (dadoLido.CompareTo(atual.Info) == 0)
                throw new Exception("Já existe esse registro!");
            else
              if (dadoLido.CompareTo(atual.Info) > 0)
            {
                NoArvore<Dado> apDireito = atual.Dir;
                Incluir(ref apDireito, dadoLido);
                atual.Dir = apDireito;
            }
            else
            {
                NoArvore<Dado> apEsquerdo = atual.Esq;
                Incluir(ref apEsquerdo, dadoLido);
                atual.Esq = apEsquerdo;
            }
        }

        public void LerArquivoDeRegistros(string nomeArquivo)
        {
            raiz = null;
            Dado dado = new Dado();
            var origem = new FileStream(nomeArquivo, FileMode.OpenOrCreate);
            var arquivo = new BinaryReader(origem);
            MessageBox.Show("Tamanho do arquivo =" + origem.Length + "\n\nTamanho do registro = " + dado.TamanhoRegistro);
            int posicaoFinal = (int)origem.Length / dado.TamanhoRegistro - 1;
            Particionar(0, posicaoFinal, ref raiz);
            origem.Close();

            void Particionar(long inicio, long fim, ref NoArvore<Dado> atual)
            {
                if (inicio <= fim)
                {
                    long meio = (inicio + fim) / 2;

                    dado = new Dado();       // cria um objeto para armazenar os dados
                    dado.LerRegistro(arquivo, meio); // 
                    atual = new NoArvore<Dado>(dado);
                    var novoEsq = atual.Esq;
                    Particionar(inicio, meio - 1, ref novoEsq);   // Particiona à esquerda 
                    atual.Esq = novoEsq;
                    var novoDir = atual.Dir;
                    Particionar(meio + 1, fim, ref novoDir);        // Particiona à direita  
                    atual.Dir = novoDir;
                }
            }

        }

        public void GravarArquivoDeRegistros(string nomeArquivo)
        {
            var destino = new FileStream(nomeArquivo, FileMode.Create);
            var arquivo = new BinaryWriter(destino);
            GravarInOrdem(raiz);
            arquivo.Close();

            void GravarInOrdem(NoArvore<Dado> r)
            {
                if (r != null)
                {
                    GravarInOrdem(r.Esq);

                    r.Info.GravarRegistro(arquivo);

                    GravarInOrdem(r.Dir);
                }
            }
        }

        public void Inserir(Dado novosDados)
        {
            bool achou = false, fim = false;
            NoArvore<Dado> novoNo = new NoArvore<Dado>(novosDados);
            if (raiz == null)         // árvore vazia
                raiz = novoNo;
            else                      // árvore não-vazia
            {
                antecessor = null;
                atual = raiz;
                while (!achou && !fim)
                {
                    antecessor = atual;
                    if (novosDados.CompareTo(atual.Info) < 0)
                    {
                        atual = atual.Esq;
                        if (atual == null)
                        {
                            antecessor.Esq = novoNo;
                            fim = true;
                        }
                    }
                    else
                        if (novosDados.CompareTo(atual.Info) == 0)
                        achou = true;  // pode-se disparar uma exceção neste caso
                    else
                    {
                        atual = atual.Dir;
                        if (atual == null)
                        {
                            antecessor.Dir = novoNo;
                            fim = true;
                        }
                    }
                }
            }
        }


        public bool Excluir(Dado procurado)
        {
            return Excluir(ref raiz);


            bool Excluir(ref NoArvore<Dado> atual)
            {
                NoArvore<Dado> atualAnt;
                if (atual == null)
                    return false;
                else
                  if (atual.Info.CompareTo(procurado) > 0)
                {
                    var temp = atual.Esq;
                    bool result = Excluir(ref temp);
                    atual.Esq = temp;
                    return result;
                }
                else
                    if (atual.Info.CompareTo(procurado) < 0)
                {
                    var temp = atual.Dir;
                    bool result = Excluir(ref temp);
                    atual.Esq = temp;
                    return result;
                }
                else
                {
                    atualAnt = atual;   // nó a retirar 
                    if (atual.Dir == null)
                        atual = atual.Esq;
                    else
                      if (atual.Esq == null)
                        atual = atual.Dir;
                    else
                    {   // pai de 2 filhos 
                        var temp = atual.Esq;
                        Rearranjar(ref temp, ref atualAnt);
                        atual.Esq = temp;
                        atualAnt = null;  // libera o nó excluído
                    }
                    return true;
                }
            }

            void Rearranjar(ref NoArvore<Dado> aux, ref NoArvore<Dado> atualAnt)
            {
                if (aux.Dir != null)
                {
                    NoArvore<Dado> temp = aux.Dir;
                    Rearranjar(ref temp, ref atualAnt);  // Procura Maior
                    aux.Dir = temp;
                }
                else
                {                           // Guarda os dados do nó a excluir
                    atualAnt.Info = aux.Info;   // troca conteúdo!
                    atualAnt = aux;             // funciona com a passagem por referência
                    aux = aux.Esq;
                }
            }

        }

        public bool ApagarNo(Dado chaveARemover)
        {
            atual = raiz;
            antecessor = null;
            bool ehFilhoEsquerdo = true;
            while (atual.Info.CompareTo(chaveARemover) != 0)  // enqto não acha a chave a remover
            {
                antecessor = atual;
                if (atual.Info.CompareTo(chaveARemover) > 0)
                {
                    ehFilhoEsquerdo = true;
                    atual = atual.Esq;
                }
                else
                {
                    ehFilhoEsquerdo = false;
                    atual = atual.Dir;
                }

                if (atual == null)  // neste caso, a chave a remover não existe e não pode
                    return false;   // ser excluída, dai retornamos falso indicando isso
            }  // fim do while

            // se fluxo de execução vem para este ponto, a chave a remover foi encontrada
            // e o ponteiro atual indica o nó que contém essa chave

            if ((atual.Esq == null) && (atual.Dir == null))  // é folha, nó com 0 filhos
            {
                if (atual == raiz)
                    raiz = null;   // exclui a raiz e a árvore fica vazia
                else
                    if (ehFilhoEsquerdo)        // se for filho esquerdo, o antecessor deixará 
                    antecessor.Esq = null;  // de ter um descendente esquerdo
                else                               // se for filho direito, o antecessor deixará de 
                    antecessor.Dir = null;  // apontar para esse filho

                atual = antecessor;  // feito para atual apontar um nó válido ao sairmos do método
            }
            else   // verificará as duas outras possibilidades, exclusão de nó com 1 ou 2 filhos
                if (atual.Dir == null)   // neste caso, só tem o filho esquerdo
            {
                if (atual == raiz)
                    raiz = atual.Esq;
                else
                    if (ehFilhoEsquerdo)
                    antecessor.Esq = atual.Esq;
                else
                    antecessor.Dir = atual.Esq;
                atual = antecessor;
            }
            else
                    if (atual.Esq == null)  // neste caso, só tem o filho direito
            {
                if (atual == raiz)
                    raiz = atual.Dir;
                else
                    if (ehFilhoEsquerdo)
                    antecessor.Esq = atual.Dir;
                else
                    antecessor.Dir = atual.Dir;
                atual = antecessor;
            }
            else // tem os dois descendentes
            {
                NoArvore<Dado> menorDosMaiores = ProcuraMenorDosMaioresDescendentes(atual);
                atual.Info = menorDosMaiores.Info;
                menorDosMaiores = null; // para liberar o nó trocado da memória
            }
            return true;
        }

        public NoArvore<Dado> ProcuraMenorDosMaioresDescendentes(NoArvore<Dado> noAExcluir)
        {
            NoArvore<Dado> paiDoSucessor = noAExcluir;
            NoArvore<Dado> sucessor = noAExcluir;
            NoArvore<Dado> atual = noAExcluir.Dir;   // vai ao ramo direito do nó a ser excluído, pois este ramo contém
            // os descendentes que são maiores que o nó a ser excluído 
            while (atual != null)
            {
                if (atual.Esq != null)
                    paiDoSucessor = atual;
                sucessor = atual;
                atual = atual.Esq;
            }

            if (sucessor != noAExcluir.Dir)
            {
                paiDoSucessor.Esq = sucessor.Dir;
                sucessor.Dir = noAExcluir.Dir;
            }
            return sucessor;
        }

        public int alturaArvore(NoArvore<Dado> atual, ref bool balanceada)
        {
            int alturaDireita, alturaEsquerda, result;
            if (atual != null && balanceada)
            {
                alturaEsquerda = 1 + alturaArvore(atual.Esq, ref balanceada);
                alturaDireita = 1 + alturaArvore(atual.Dir, ref balanceada);
                result = Math.Max(alturaEsquerda, alturaDireita);

                //if (alturaDireita > alturaEsquerda)
                //    result = alturaDireita;
                //else
                //  result = alturaEsquerda;

                if (Math.Abs(alturaDireita - alturaEsquerda) > 1)
                    balanceada = false;
            }
            else
                result = 0;
            return result;
        }

    }

}