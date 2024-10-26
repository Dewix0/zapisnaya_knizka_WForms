using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zapsinaya_knizka_new
{
    public class Stranica
    {
        int nomer_stranici;
        string text_stranici;

        public Stranica(int nomer_stranici, string text_stranici)
        {
            this.nomer_stranici = nomer_stranici;
            this.text_stranici = text_stranici;
        }

        public int Nomer_stranici { get => nomer_stranici; set => nomer_stranici = value; }
        public string Text_stranici { get => text_stranici; set => text_stranici = value; }
    }
}
