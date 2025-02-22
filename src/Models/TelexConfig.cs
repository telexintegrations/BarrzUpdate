using System.ComponentModel;
using System.Text.Json.Serialization;

namespace src.Models{
    public class TelexConfig{
        public Data? Data{get; set;}
    }
    public class Data{
        public Date? Date{get; set;}
        public Descriptions? Descriptions{get; set;}
        public bool? Is_active{get; set;}
        public string? Integration_type{get; set;}
        public string? Integration_category{get; set;}
        public List<string>? Key_features{get; set;}
        public string? Author{get; set;}
        public List<CommonModel>? Settings{get; set;}
        public string? Target_url{get; set;}
        public string? Tick_url{get; set;}
    }

    public class Date{
        public string? Created_at{get; set;}
        public string? Updated_at{get; set;}
    }

    public class Descriptions{
        public string? App_name{get; set;}
        public string? App_description{get; set;}
        public string? App_logo{get; set;}
        public string? App_url{get; set;}
        public string? Background_color{get; set;}
    }
}