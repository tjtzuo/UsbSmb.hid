using System;
using System.Runtime.InteropServices;
using System.Threading;

namespace PFC_tool
{
    public class SMBus
    {
/*
        const int MaxBuf = 64;
        public const int SUCCESS = 0; // FT_OK
        public const int INVALID_HANDLE = -1; // FT_INVALID_HANDLE
        public int Status = INVALID_HANDLE;
        public byte[] sbuf = new byte[MaxBuf];
        public string strSMBus;
        public bool bPEC = false;
 */
        public byte bPEC = 0;
        public byte Addr = 0x16;	// Smart Battery
        //byte Addr = 0xAA;
        byte nDevIndex = 0xFF;
        public bool bHDQ = false;

        public SMBus()
        {
            UInt32 count = UsbInit();
            nDevIndex = (Byte)(count - 1);
            Console.WriteLine("USB SMBus Count: " + count);
        }
        public ushort GetVersion()
        {
            return UsbGetVersion(nDevIndex);
        }
        public bool ReadByte(int cmd, byte[] pValue)
        {
            if (bHDQ && cmd < 0x80)
                return UsbHdqRead(nDevIndex, (byte)cmd, pValue);
            else
                return UsbSmbRead(nDevIndex, Addr, (byte)cmd, 0, sizeof(Byte), pValue);
        }
        public bool ReadBytes(int cmd, int nCount, byte[] pBuf)
        {
            if (bHDQ && cmd < 0x80)
            {
                for (int i = 0; i < nCount; i++)
                {
                    byte[] pVal = new byte[1];
                    if (!UsbHdqRead(nDevIndex, (byte)(cmd + i), pVal))
                        return false;
                    pBuf[i] = pVal[0];
                }
                return true;
            }
            else
                return UsbSmbRead(nDevIndex, Addr, (byte)cmd, 0, (byte)nCount, pBuf);
        }
        public bool ReadBlock(int cmd, int nCount, byte[] pBuf)
        {
            byte[] pBlock = new byte[nCount + 1];
            bool result = UsbSmbRead(nDevIndex, Addr, (byte)cmd, bPEC, (byte)pBlock.Length, pBlock);
            Array.Copy(pBlock, 1, pBuf, 0, nCount);
            return result && (pBlock[0] <= nCount);
        }
        public bool ReadWord(int cmd, short[] pwValue)
        {
            bool result = false;
            byte[] pBuf = new byte[2];
            if (bHDQ && cmd < 0x80)
            {
                if (UsbHdqRead(nDevIndex, (byte)(cmd + 1), pBuf))
                {
                    pBuf[1] = pBuf[0];
                    if (UsbHdqRead(nDevIndex, (byte)cmd, pBuf))
                        result = true;
                }
            }
            else
            {
                result = UsbSmbRead(nDevIndex, Addr, (byte)cmd, bPEC, sizeof(UInt16), pBuf);
            }
            pwValue[0] = (short)((UInt16)pBuf[0] | (pBuf[1] << 8));
            return result;
        }
        public bool SendByte(int cmd)
        {
            byte[] pBuf = new byte[1];
            return UsbSmbWrite(nDevIndex, Addr, (byte)cmd, 0, 0, pBuf);
        }
        public bool WriteByte(int cmd, int val)
        {
            if (bHDQ && cmd < 0x80)
            {
                return UsbHdqWrite(nDevIndex, (byte)cmd, (byte)val);
            }
            else
            {
                byte[] pBuf = new byte[1];
                pBuf[0] = (byte)val;
                return UsbSmbWrite(nDevIndex, Addr, (byte)cmd, 0, sizeof(Byte), pBuf);
            }
        }
        public bool WriteBytes(int cmd, int nCount, byte[] pBuf)
        {
            if (bHDQ && cmd < 0x80)
            {
                for (int i = 0; i < nCount; i++)
                    if (!UsbHdqWrite(nDevIndex, (byte)(cmd + i), pBuf[i]))
                        return false;
                return true;
            }
            else
                return UsbSmbWrite(nDevIndex, Addr, (byte)cmd, 0, (byte)nCount, pBuf);
        }
        public bool WriteBlock(int cmd, int nCount, byte[] pBuf)
        {
            byte[] pBlock = new byte[nCount + 1];
            Array.Copy(pBuf, 0, pBlock, 1, nCount);
            pBlock[0] = (byte)nCount;
            return UsbSmbWrite(nDevIndex, Addr, (byte)cmd, bPEC, (byte)pBlock.Length, pBlock);
        }
        public bool WriteWord(int cmd, int val)
        {
            if (bHDQ && cmd < 0x80)
            {
                if (UsbHdqWrite(nDevIndex, (byte)cmd, 0xFF))
                    if (UsbHdqWrite(nDevIndex, (byte)(cmd + 1), (byte)(val >> 8)))
                        if (UsbHdqWrite(nDevIndex, (byte)cmd, (byte)val))
                            return true;
                return false;
            }
            else
            {
                byte[] pBuf = new byte[2];
                pBuf[0] = (byte)val;
                pBuf[1] = (byte)(val >> 8);
                return UsbSmbWrite(nDevIndex, Addr, (byte)cmd, bPEC, sizeof(UInt16), pBuf);
            }
        }

        public bool WriteByteVerify(int cmd, int val)
        {
            if (WriteByte(cmd, val))
            {
                Thread.Sleep(10);
                byte[] pByte = new byte[1];
                if (ReadByte(cmd, pByte))
                {
                    if (pByte[0] == val)
                        return true;
                }
            }
            return false;
        }
        public bool WriteWordVerify(int cmd, int val)
        {
            if (WriteWord(cmd, val))
            {
                Thread.Sleep(10);
                short[] pWord = new short[1];
                if (ReadWord(cmd, pWord))
                {
                    if (pWord[0] == val)
                        return true;
                }
            }
            return false;
        }
        public bool ReadDataFlashSector(bool bNewSBS, byte[] sector_buf)
        {
            const int compare_end = 5;
            const int retry_end = 3;
            int retry_count;
            int index;
            byte[] sbuf = new byte[32];
        
            if (sector_buf.Length < 256)
                return false;
        
            //Array.fill(sector_buf, (byte)0xFF);
            Array.Clear(sector_buf, 0, sector_buf.Length);
        
            for (index = 0; index < 8; index++)
            {
                if (bNewSBS)
                {
                    for (retry_count = 0; retry_count < retry_end; retry_count++)
                    {
                        if (WriteByteVerify(0x3f, index))
                            break;

                        Thread.Sleep(10);
                    }
                    if (retry_count == retry_end)
                        break;
                }
            
                //Arrays.fill(sbuf, (byte) 0xFF);
                Array.Clear(sbuf, 0, sbuf.Length);
            
                int compare_count;
                for (compare_count = 0; compare_count < compare_end; compare_count++)
                {
                    byte[] buf = new byte[32];
                    bool result;
                    if (bNewSBS)
                    {
                        result = ReadBytes(0x40, buf.Length, buf);
                    }
                    else
                    {
                        result = ReadBlock(0x78 + index, buf.Length, buf);
                    }
                    if (result)
                    {
                        if (memcmp(buf, sbuf, sbuf.Length) == 0)
                        {
                            buf.CopyTo(sector_buf, index * buf.Length);
                            break;
                        }
                        buf.CopyTo(sbuf, 0);
                    }
                }
                if (compare_count == compare_end)
                    break;
            }
            
            return (index == 8);
        }
        public bool ReadDataFlashAll(bool bNewSBS, byte[] buffer)
        {
            const int retry_end = 3;
            int retry_count;
        
            if (buffer.Length < 2048)
                return false;

            if (bNewSBS)
            {
                for (retry_count = 0; retry_count < retry_end; retry_count++)
                {
                    if (WriteByte(0x61, 0))
                        break;

                    Thread.Sleep(10);
                }
                if (retry_count == retry_end)
                    return false;
            }

            int index;
            for (index = 0; index < 8; index++)
            {
                for (retry_count = 0; retry_count < retry_end; retry_count++)
                {
                    if (bNewSBS)
                    {
                        if (WriteByteVerify(0x3e, index))
                            break;
                    }
                    else
                    {
                        if (WriteWordVerify(0x77, index))
                            break;
                    }
                    Thread.Sleep(10);
                }
                if (retry_count == retry_end)
                    break;

                for (retry_count = 0; retry_count < retry_end; retry_count++)
                {
                    byte[] sector_buf = new byte[256];
                    if (ReadDataFlashSector(bNewSBS, sector_buf))
                    {
                        sector_buf.CopyTo(buffer, index * sector_buf.Length);
                        break;
                    }

                    Thread.Sleep(10);
                }
                if (retry_count == retry_end)
                    break;
            }

            return (index == 8);
        }
        byte Chksum(int len, byte[] sbuf)
        {
            int sum = 0;
            for (int i = 0; i < len; i++)
                sum += sbuf[i];

    	    return (byte)((~sum)+1);
        }
        public bool WriteDataFlashSector(bool bNewSBS, byte[] sector_buf)
        {
            const int retry_end = 3;
            int retry_count;
        
            if (sector_buf.Length < 256)
                return false;
        
            if (bNewSBS)
            {
                int i, j;
                for (i = 0; i < 2; i++)
                {
                    for (j = 0; j < 4; j++)
                    {
                        for (retry_count = 0; retry_count < retry_end; retry_count++)
                        {
                            if (WriteByteVerify(0x3f, i * 4 + j))
                                break;

                            Thread.Sleep(10);
                        }
                        if (retry_count == retry_end)
                            break;

                        byte[] sbuf = new byte[32];
                        Array.Copy(sector_buf, (i * 4 + j) * 32, sbuf, 0, sbuf.Length);
                        if (!WriteBytes(0x40, sbuf.Length, sbuf))
                            break;
                        if (!WriteByte(0x60, Chksum(sbuf.Length, sbuf)))
                            break;
                    
                        if (j == 0)
                            Thread.Sleep(100);
                        else
                            Thread.Sleep(10);

                        for (retry_count = 0; retry_count < retry_end; retry_count++)
                        {
                            byte[] buf = new byte[32];
                            if (ReadBytes(0x40, buf.Length, buf))
                            {
                                if (memcmp(buf, sbuf, sbuf.Length) == 0)
                                    break;
                            }
                        }
                        if (retry_count == retry_end)
                            break;
                    }
                    if (j < 4)
                        break;
                }
            
                return (i == 2);
            }
            else
            {
                int i;
                for (i = 0; i < 8; i++)
                {
                    byte[] sbuf = new byte[32];
                
                    Array.Copy(sector_buf, i * 32, sbuf, 0, sbuf.Length);
                    if (!WriteBlock(0x78 + i, sbuf.Length, sbuf))
                        break;

                    if (i == 0)
                    {
                        short[] pWord = new short[1];
                        if (ReadWord(0x77, pWord))
                        {
                            if (pWord[0] % 4 == 0)
                            {
                                Thread.Sleep(1700);
                            }
                        }
                    }
                    Thread.Sleep(100);

                    for (retry_count = 0; retry_count < retry_end; retry_count++)
                    {
                        byte[] buf = new byte[32];
                        if (ReadBlock(0x78 + i, buf.Length, buf))
                        {
                            //if (memcmp(buf, sbuf, sbuf.Length) == 0)
                                break;
                        }
                        Thread.Sleep(100);
                    }
                    if (retry_count == retry_end)
                        break;
                }

                return (i == 8);
            }
        }
        public bool WriteDataFlashAll(bool bNewSBS, byte[] buffer)
        {
            const int retry_end = 3;
            int retry_count;
        
            if (buffer.Length < 2048)
                return false;

            if (bNewSBS)
            {
                for (retry_count = 0; retry_count < retry_end; retry_count++)
                {
                    if (WriteByte(0x61, 0))
                        break;

                    Thread.Sleep(10);
                }
                if (retry_count == retry_end)
                    return false;
            }

            int index;
            for (index = 0; index < 8; index++)
            {
                if (bNewSBS && index == 4)
                    continue;

                for (retry_count = 0; retry_count < retry_end; retry_count++)
                {
                    if (bNewSBS)
                    {
                        if (WriteByteVerify(0x3e, index))
                            break;
                    }
                    else
                    {
                        if (WriteWordVerify(0x77, index))
                            break;
                    }
                    Thread.Sleep(10);
                }
                if (retry_count == retry_end)
                    break;

                for (retry_count = 0; retry_count < retry_end; retry_count++)
                {
                    byte[] sector_buf = new byte[256];
                    Array.Copy(buffer, index * 256, sector_buf, 0, sector_buf.Length);
                    if (WriteDataFlashSector(bNewSBS, sector_buf))
                        break;

                    Thread.Sleep(10);
                }
                if (retry_count == retry_end)
                    break;
            }

            return (index == 8);
        }

        [DllImport("UsbSmb")]
        public static extern uint UsbInit();
        [DllImport("UsbSmb")]
        public static extern ushort UsbGetVersion(int nDev);
        [DllImport("UsbSmb")]
        public static extern bool UsbSmbWrite(byte nDev, byte Addr, byte cmd, byte bPEC, byte len, byte[] pBuf);
        [DllImport("UsbSmb")]
        public static extern bool UsbSmbRead(byte nDev, byte Addr, byte cmd, byte bPEC, byte len, byte[] pBuf);
        [DllImport("UsbSmb")]
        public static extern bool UsbHdqWrite(byte nDev, byte byCommand, byte byValue);
        [DllImport("UsbSmb")]
        public static extern bool UsbHdqRead(byte nDev, byte byCommand, byte[] pValue);
        [DllImport("UsbSmb")]
        public static extern bool UsbEepromWrite(byte nDev, ushort loc, byte[] pBuf);
        [DllImport("UsbSmb")]
        public static extern bool UsbEepromRead(byte nDev, ushort loc, byte[] pBuf);
        [DllImport("UsbSmb")]
        public static extern bool UsbSendIoCmd(byte nDev, byte byCommand);
        [DllImport("msvcrt.dll")]
        public static extern int memcmp(byte[] arr1, byte[] arr2, int cnt);
    }
}
