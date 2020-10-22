using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte 
{
    class Cidades : IComparable<Cidades>, IRegistro
    {
        int idCidade, coordenadaX, coordenadaY;
        string nomeCidade;

        // contrutor vazio
        public Cidades() { }

        // contrutor completo
        public Cidades(int idCidade,  string nomeCidade, int coordenadaX, int coordenadaY)
        {
            this.IdCidade = idCidade;
            this.CoordenadaX = coordenadaX;
            this.CoordenadaY = coordenadaY;
            this.NomeCidade = nomeCidade;
        }

        // contrutor apenas com o campo chave para busca
        public Cidades(int idCidade)
        {
            this.IdCidade = idCidade;
        }

        // propriedades de todos os atributos com getters e setters
        public int IdCidade { get => idCidade; set => idCidade = value; }
        public int CoordenadaX { get => coordenadaX; set => coordenadaX = value; }
        public int CoordenadaY { get => coordenadaY; set => coordenadaY = value; }
        public string NomeCidade { get => nomeCidade; set => nomeCidade = value; }

        // compara os caminhos pelo id da cidade
        public int CompareTo(Cidades other)
        {
            return IdCidade - other.IdCidade;
        }


        // NÃO UTILIZAMOS A LEITURA DE ARQUIVO POR BINARYREADER, FIZEMOS PELOS ARQUIVOS TEXTO
        public void LerRegistro(BinaryReader arquivo, long qualRegistro)
        {

        }
        public void GravarRegistro(BinaryWriter arquivo)
        {

        }
        public int TamanhoRegistro { get; }

        public override string ToString()
        {
            return this.idCidade + " - " + this.nomeCidade;
        }
    }
}
