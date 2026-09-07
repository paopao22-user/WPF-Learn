using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrismLineMonitor.Events
{
    // 定义一个报警广播频道，里面运送的数据是 string
    public class AlarmEvent:PubSubEvent<string>
    {
        // 类体通常完全为空，它只是一张“频道许可证”

    }
}
