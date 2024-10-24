using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class VoucherCuaPh
    {
        public string IdPh { get; set; } = null!;
        public string IdVoucher { get; set; } = null!;
        public int? SoLuong { get; set; }

        public virtual PhuHuynh IdPhNavigation { get; set; } = null!;
        public virtual Voucher IdVoucherNavigation { get; set; } = null!;
    }
}
