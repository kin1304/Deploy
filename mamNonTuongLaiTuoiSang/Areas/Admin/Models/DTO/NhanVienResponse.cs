using Newtonsoft.Json;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Models.DTO
{
    public class NhanVienResponse
    {
        [JsonProperty("Nhan vien theo id")]
        public string NhanhVienId { get; set; }
    }
}
