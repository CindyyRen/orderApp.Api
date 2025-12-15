using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Domain.Entities;

public enum OrderStatus
{
    Pending,    // 待支付 / 待处理
    Paid,       // 已支付
    Preparing,  // 后厨处理中
    Ready,      // 准备完成
    Delivered,  // 已送达
    Cancelled   // 已取消
}
