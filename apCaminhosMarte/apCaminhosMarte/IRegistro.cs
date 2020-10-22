using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace apCaminhosMarte
{
    interface IRegistro
    {
        void LerRegistro(BinaryReader arquivo, long qualRegistro);
        void GravarRegistro(BinaryWriter arquivo);
        int TamanhoRegistro { get; }
    }
}
