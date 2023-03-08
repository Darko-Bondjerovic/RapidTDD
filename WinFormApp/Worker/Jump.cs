namespace WinFormApp
{
    public class Jump
    {
        public string File { get; set; } = "";
        public int Spot { get; set; } = 0;
        public string Desc { get; set; } = "";

        public Jump() { }

        public Jump(string file, int spot, string desc)
        {
            File = file;
            Spot = spot;
            Desc = desc;
        }   
    }
}
