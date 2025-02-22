namespace src.Models.Request{
    public class TickRequest{
        public string Channel_id{get; set;}
        public string Return_url{get; set;}
        public List<CommonModel> Settings{get; set;}

    }
}
