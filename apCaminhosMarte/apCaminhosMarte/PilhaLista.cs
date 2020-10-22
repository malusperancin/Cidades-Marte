using System;
using System.Windows.Forms;
using System.Threading;

class PilhaLista<Dado> : ListaSimples<Dado>
                        where Dado : IComparable<Dado>
{
    public Dado Desempilhar()
    {
        if (EstaVazia)
            throw new Exception("pilha vazia!");

        Dado valor = base.Primeiro.Info;

        NoLista<Dado> pri = base.Primeiro;
        NoLista<Dado> ant = null;
        base.RemoverNo(ref pri, ref ant);
        return valor;
    }

    public void Empilhar(Dado elemento)
    {
        base.InserirAntesDoInicio
            (
                new NoLista<Dado>(elemento, null)
            );
    }

    new public bool EstaVazia
    {
        get => base.EstaVazia;
    }

    public Dado OTopo()
    {
        if (EstaVazia)
            throw new Exception("pilha vazia!");

        return base.Primeiro.Info;
    }

    public int Tamanho { get => base.QuantosNos; }


    public void Exibir(DataGridView dgv)
    {
        dgv.ColumnCount = Tamanho;
        dgv.RowCount = 2;
        for (int j = 0; j < dgv.ColumnCount; j++)
            dgv[j, 0].Value = "";

        var auxiliar = new PilhaLista<Dado>();
        int i = 0;
        while (!this.EstaVazia)
        {
            dgv[i++, 0].Value = this.OTopo();
            Thread.Sleep(300);
            Application.DoEvents();
            auxiliar.Empilhar(this.Desempilhar());
        }

        while (!auxiliar.EstaVazia)
            this.Empilhar(auxiliar.Desempilhar());
    }

    public PilhaLista<Dado> Copia()
    {
        var copia = new PilhaLista<Dado>();  // nova instância para retorno
        Dado[] aux = new Dado[this.Tamanho]; // vetor auxiliar

        for (int i = 0; !this.EstaVazia; i++)
        {
            aux[i] = this.OTopo(); // guarda-se a pilha em um vetor auxiliar na ordem original
            this.Desempilhar();
        }

        for (int i = aux.Length - 1; i >= 0; i--)
        {
            this.Empilhar(aux[i]); // vai empilhando de trás para frente na pilha original, para manter a ordem
            copia.Empilhar(aux[i]);
        }

        return copia;
    }

    public PilhaLista<Dado> CopiaInvertida()
    {
        var copia = new PilhaLista<Dado>(); // nova instância para retorno
        Dado[] aux = new Dado[this.Tamanho]; // vetor auxiliar

        for (int i = 0; !this.EstaVazia; i++)
        {
            aux[i] = this.OTopo(); //guarda-se a pilha em um vetor auxiliar na ordem original
            this.Desempilhar();
            copia.Empilhar(aux[i]); //  empilha-se na cópia, para salvar em ordem invertida
        }

        for (int i = aux.Length - 1; i >= 0; i--)
        {
            this.Empilhar(aux[i]); // vai empilhando de trás para frente na pilha original, para manter a ordem
        }

        return copia;
    }
}
