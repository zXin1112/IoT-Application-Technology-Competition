using System;

namespace Protocol_Programming
{
    public static class CRC_16
    {
        public static byte[] GetCRC(byte[] datas, bool ReturnAll = false)
        {
            int crcResult = 0xFFFF;

            for (int i = 0; i < datas.Length; i++)
            {
                crcResult = datas[i] ^ crcResult;
                for (int j = 0; j < 8; j++)
                {
                    if ((crcResult & 0x0001) > 0)
                        crcResult = crcResult >> 1 ^ 0xA001;
                    else
                        crcResult = crcResult >> 1;
                }
            }

            byte[] Results = new byte[2];

            Results[0] = (byte)crcResult;
            Results[1] = (byte)(crcResult >> 8);

            if (ReturnAll)
            {
                byte[] AllResults = new byte[datas.Length + 2];
                datas.CopyTo(AllResults, 0);
                Results.CopyTo(AllResults, datas.Length);

                return AllResults;
            }

            return Results;
        }

        public static bool CheckCRC(byte[] datas)
        {
            if (datas.Length == 0)
                return false;

            int length = datas.Length;
            byte[] data = new byte[length - 2];

            for (int i = 0; i < data.Length; i++)
                data[i] = datas[i];

            byte[] crcResult = GetCRC(data);
            if (crcResult[0] == datas[length - 2] && crcResult[1] == datas[length - 1])
                return true;
            else
                return false;
        }
    }
}


