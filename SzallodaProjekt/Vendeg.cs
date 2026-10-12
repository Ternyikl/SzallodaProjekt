using System;

namespace SzallodaProjekt
{
    public class Vendeg
    {
        private int vendegId;
        private string nev;
        private string varos;

        public int VendegId
        {
            get => this.vendegId;
            set => this.vendegId = value;
        }

        public string Nev
        {
            get => this.nev;
            set => this.nev = value;
        }

        public string Varos
        {
            get => this.varos;
            set => this.varos = value;
        }

        public Vendeg(int vendegId, string nev, string varos)
        {
            this.vendegId = vendegId;
            this.nev = nev;
            this.varos = varos;
        }
    }

   
   
}