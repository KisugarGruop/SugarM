namespace SugarM.Models {
    public class Size : Maptable {
        public string CaneYear { get; set; }
        public string SizeCode { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public string DeleteFlag { get; set; } = "N";

    }
}