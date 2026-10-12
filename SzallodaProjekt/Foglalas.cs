public class Foglalas
{
    private int foglalasId;
    private int vendegId;
    private int szobaId;
    private int ejszakakSzama;
    private DateTime foglalasDatuma;

    public int FoglalasId
    {
        get => this.foglalasId;
        set => this.foglalasId = value;
    }

    public int VendegId
    {
        get => this.vendegId;
        set => this.vendegId = value;
    }

    public int SzobaId
    {
        get => this.szobaId;
        set => this.szobaId = value;
    }

    public int EjszakakSzama
    {
        get => this.ejszakakSzama;
        set => this.ejszakakSzama = value;
    }

    public DateTime FoglalasDatuma
    {
        get => this.foglalasDatuma;
        set => this.foglalasDatuma = value;
    }

    public Foglalas(int foglalasId, int vendegId, int szobaId, int ejszakakSzama, DateTime foglalasDatuma)
    {
        this.foglalasId = foglalasId;
        this.vendegId = vendegId;
        this.szobaId = szobaId;
        this.ejszakakSzama = ejszakakSzama;
        this.foglalasDatuma = foglalasDatuma;
    }
}