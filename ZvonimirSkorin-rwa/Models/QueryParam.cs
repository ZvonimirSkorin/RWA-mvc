namespace ZvonimirSkorin_rwa.Models
{
    public class WhereParam
    {
        public string name { get; set; }
        public int? value { get; set; }

        public WhereParam(string name, int? value)
        {
            this.name = name;
            this.value = value;
        }
    }
}
