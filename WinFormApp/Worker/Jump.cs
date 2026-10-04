namespace WinFormApp
{
    public class Jump
    {
        public string File { get; set; } = "";
        public int Spot { get; set; } = 0;
        public string Desc { get; set; } = "";
        public int Line { get; set; } = 0;
        public int Column { get; set; } = 0;
        public Jump() { }
        public Jump(string file, int spot, string desc = "", int line = 0, int column = 0)
        { File=file; Spot=spot; Desc=desc; Line=line; Column=column; }
    }
}
