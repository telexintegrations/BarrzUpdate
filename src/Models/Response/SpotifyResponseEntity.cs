namespace src.Models.Response{
    public class ResponseItem{
        public string AlbumName{get; set;} = string.Empty;
        public List<string> Artist{get; set;} = new List<string>();
        public string ReleaseDate{get; set;} = string.Empty;
    }
}