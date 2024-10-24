using Newtonsoft.Json;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Models.DTO
{
    public class HocSinhResponse
    {
        [JsonProperty("Hoc Sinh theo id")]
        public string HocSinhId { get; set; }
    }
}
