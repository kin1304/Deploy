using Newtonsoft.Json;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Models.DTO
{
    public class HocSinhLopResponse
    {
        [JsonProperty("Hoc Sinh theo id")]
        public string HocSinhId { get; set; }
    }
}
