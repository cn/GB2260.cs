namespace GB2260
{
    public class Division
    {
        public string Source {get;set;}
        public string Name { get; set; }
        public string Code { get; set; }
        public Revision Revision { get; set; }
        public string Province { get; set; }
        public string Prefecture { get; set; }

        public override string ToString()
        {
            return $"{Province ?? ""} {Prefecture ?? ""} {Name ?? ""}";
        }
    }
}