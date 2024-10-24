using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Models.DTO
{
    public class PhuHuynhResponse
    {
        [JsonProperty("Phu huynh theo id")]
        public string PhuHuynhId { get; set; }
    }
}
