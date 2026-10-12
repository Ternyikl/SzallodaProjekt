public class Szoba
{
    private int szobaId;
    private int szobaszam;
    private string kategoria;
    private int arPerEjszaka;

    public int SzobaId
    {
        get => this.szobaId;
        set => this.szobaId = value;
    }

    public int Szobaszam
    {
        get => this.szobaszam;
        set => this.szobaszam = value;
    }

    public string Kategoria
    {
        get => this.kategoria;
        set => this.kategoria = value;
    }

    public int ArPerEjszaka
    {
        get => this.arPerEjszaka;
        set => this.arPerEjszaka = value;
    }

    public Szoba(int szobaId, int szobaszam, string kategoria, int arPerEjszaka)
    {
        this.szobaId = szobaId;
        this.szobaszam = szobaszam;
        this.kategoria = kategoria;
        this.arPerEjszaka = arPerEjszaka;
    }
}
