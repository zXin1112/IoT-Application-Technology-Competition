using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Protocol_Programming.Model
{
    class SensorDataModel
    {
        public int SensorDataId;
        public int GatewayId;
        public string GatewayName;
        public int SensorId;
        public string SensorName;
        public string SensorTag;
        public string SensorUnit;
        public string SensorData;
        public DeviceDataType DataType;
        public DateTime CreateDate;
    }

    enum DeviceDataType
    {
        Numerical=0,
        Bool=3
    }
}
