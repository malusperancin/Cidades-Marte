using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    class CaminhoEntreCidades : IComparable<CaminhoEntreCidades>
    {
        int idCidadeOrigem, idCidadeDestino, distancia, tempo, custo;

        // contrutor completo
        public CaminhoEntreCidades(int idCidadeOrigem, int idCidadeDestino, int distancia, int tempo, int custo)
        {
            this.IdCidadeOrigem = idCidadeOrigem;
            this.IdCidadeDestino = idCidadeDestino;
            this.Distancia = distancia;
            this.Tempo = tempo;
            this.Custo = custo;
        }

        // contrutor apenas com o campo chave para busca
        public CaminhoEntreCidades(int idCidadeOrigem)
        {
            this.IdCidadeOrigem = idCidadeOrigem;
        }

        // contrutor vazio
        public CaminhoEntreCidades() { }

        // propriedades de todos os atributos com getters e setters
        public int IdCidadeOrigem { get => idCidadeOrigem; set => idCidadeOrigem = value; }
        public int IdCidadeDestino { get => idCidadeDestino; set => idCidadeDestino = value; }
        public int Distancia { get => distancia; set => distancia = value; }
        public int Tempo { get => tempo; set => tempo = value; }
        public int Custo { get => custo; set => custo = value; }
    
        // compara os caminhos pelo id da cidade de origem
        public int CompareTo(CaminhoEntreCidades outro)
        {
            return IdCidadeOrigem - outro.IdCidadeOrigem;
        }

        public override string ToString()
        {
            return "(" + IdCidadeOrigem + ", " + IdCidadeDestino + ")"; 
        }

    }
}
