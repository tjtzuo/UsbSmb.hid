#include "stdafx.h"
#include "UsbSmb.h"
#include "SMBus.h"

#ifdef _DEBUG
#undef TRACE
void TRACE(PCSTR str, ...)
{
	va_list args;
	va_start(args, str);
	CHAR szTemp[256];
	if (_vsnprintf_s(szTemp, _countof(szTemp), _TRUNCATE, str, args) > 0)
		OutputDebugStringA(szTemp);
	va_end(args);
}
#else
//#define TRACE(...)
#endif


FTC_STATUS Status = FTC_INVALID_HANDLE;
char sbuf[100];
CString g_strSMBus;
BOOL g_bPEC = FALSE;
BOOL g_bHDQ = FALSE;
int nDevIndex = 0;

#ifdef _N_SBS_
BYTE Addr = 0xAA;
#else
BYTE Addr = 0x16;	// Smart Battery
#endif

#define READ_CMD2		0x46		// registered I2C control command


/****************************************************************************

htoi

ASCII Hex string to integer conversion.

****************************************************************************/
int htoi(const TCHAR *s)
{
    int n;

    /* skip white space */
    for ( ; *s == _T(' ') || *s == _T('\n') || *s == _T('\t'); s++);

    /* compute hexadecimal value */
    for (n=0; ; s++)
    {
        if ((*s >= _T('0')) && (*s <= _T('9')))
           n = (n * 0x10) + (*s - _T('0'));

        else if ((*s >= _T('a')) && (*s <= _T('f')))
                n = (n * 0x10) + (*s - _T('a') + 0xa);

        else if ((*s >= _T('A')) && (*s <= _T('F')))
                n = (n * 0x10) + (*s - _T('A') + 0xA);

        else return(n);
    }
}


int ReadBytes(BYTE byCommand, PBYTE pBuf/* = NULL*/, UINT nReadBytes/* = 32*/)
{
	int nRet = 0;
	UINT i;
	BYTE buf[64];

	if (pBuf == NULL) pBuf = (PBYTE)sbuf;
	memset(pBuf, 0, nReadBytes);
	Status = FTC_INVALID_HANDLE;

	if (g_bHDQ)
	{
		for (i = 0; i < nReadBytes; i++)
		{
			ReadHDQ(byCommand++, &buf[i]);
			TRACE("%02X", buf[i]);

			if (Status != FTC_SUCCESS)
				break;
		}
		TRACE("\n");

		if (Status == FTC_SUCCESS)
		{
			memcpy(pBuf, buf, nReadBytes);
			nRet = i;
		}
	}
	else
	{
		if (UsbSmbRead(nDevIndex, Addr, byCommand, FALSE, nReadBytes, pBuf))
		{
			for (i = 0; i < nReadBytes; i++)
				TRACE("%02X", pBuf[i]);
			TRACE("\n");
			nRet = nReadBytes;
			Status = FTC_SUCCESS;
		}
	}

	return nRet;
}

int ReadBlock(BYTE byCommand)
{
	int nRet = 0;

	memset(sbuf, 0, sizeof(sbuf));
	Status = FTC_INVALID_HANDLE;
	const UINT nReadBytes = 33;
	BYTE buf[nReadBytes];
	if (UsbSmbRead(nDevIndex, Addr, byCommand, g_bPEC, nReadBytes, buf))
	{
		for (UINT i = 0; i < nReadBytes; i++)
			TRACE("%02X", buf[i]);
		TRACE("\n");

		if (buf[0] > 0 && buf[0] <= 32)
		{
			nRet = buf[0];
			memcpy(sbuf, &buf[1], buf[0]);

			Status = FTC_SUCCESS;
		}
	}

	if (Status == FTC_INVALID_HANDLE)
		g_strSMBus = _T("SM Bus ERROR !!");
	else
		g_strSMBus.Empty();

	return nRet;
}

BOOL ReadBlock(BYTE byCommand, UINT unCount)
{
	int nRet = FALSE;

	if (unCount > 60)
		unCount = 60;

	memset(sbuf, 0, sizeof(sbuf));
	Status = FTC_INVALID_HANDLE;
	UINT nReadBytes = unCount + 1;
	BYTE buf[61];
	if (UsbSmbRead(nDevIndex, Addr, byCommand, g_bPEC, nReadBytes, buf))
	{
		for (UINT i = 0; i < nReadBytes; i++)
			TRACE("%02X", buf[i]);
		TRACE("\n");

		if (buf[0] == unCount)
		{
			nRet = TRUE;
			memcpy(sbuf, &buf[1], buf[0]);

			Status = FTC_SUCCESS;
		}
	}

	if (Status)
		g_strSMBus = _T("SM Bus ERROR !!");
	else
		g_strSMBus.Empty();

	return nRet;
}

void WriteBytes(BYTE byCommand, UINT len /* = 32 */)
{
	Status = FTC_INVALID_HANDLE;
	if (g_bHDQ)
	{
		for (UINT i = 0; i < len; i++)
		{
			WriteHDQ(byCommand++, sbuf[i]);
			Sleep(10);
			if (Status != FTC_SUCCESS)
				break;
		}
	}
	else
	{
		if (UsbSmbWrite(nDevIndex, Addr, byCommand, FALSE, len, (PBYTE)sbuf))
		{
			// Success
			Status = FTC_SUCCESS;
		}
	}
}

void WriteBlock(BYTE byCommand, UINT len /* = 32 */)
{
	Status = FTC_INVALID_HANDLE;
	BYTE buf[60];
	if (len > 59)
		len = 59;
	buf[0] = len;
	memcpy(&buf[1], sbuf, len);
	if (UsbSmbWrite(nDevIndex, Addr, byCommand, g_bPEC, len + 1, buf))
		Status = FTC_SUCCESS;
}

void WriteString(BYTE byCommand, const CString& sData)
{
	UINT len = min(sData.GetLength(), 32);
	BYTE buf[33];
	buf[0] = len;
	//memcpy(&buf[1], sData, len);
	for (UINT i = 0; i < len; i++)
		buf[1+i] = (BYTE)sData[i];
	if (UsbSmbWrite(nDevIndex, Addr, byCommand, g_bPEC, len + 1, buf))
		Status = FTC_SUCCESS;
	else
		Status = FTC_INVALID_HANDLE;
}

void WriteWord(BYTE byCommand, WORD wValue)
{
	Status = FTC_INVALID_HANDLE;
	if (UsbSmbWrite(nDevIndex, Addr, byCommand, g_bPEC, sizeof(WORD), (PBYTE)&wValue))
		Status = FTC_SUCCESS;
}

void ReadWord(BYTE byCommand, PWORD pwValue)
{
	Status = FTC_INVALID_HANDLE;
	if (UsbSmbRead(nDevIndex, Addr, byCommand, g_bPEC, sizeof(WORD), (PBYTE)pwValue))
		Status = FTC_SUCCESS;

	if (Status)
		g_strSMBus = _T("SM Bus ERROR !!");
	else
		g_strSMBus.Empty();
}

void Read2(BYTE byCommand, PWORD pwValue)
{
	Status = FTC_INVALID_HANDLE;

	UINT nReadBytes = sizeof(WORD)+g_bPEC;
	BYTE buf[64];

	buf[0] = READ_CMD2;	// send module command
	buf[1] = Addr;		// send Address
	buf[2] = byCommand;	// send SMBus command
	buf[3] = nReadBytes;
	DWORD RecvLength = nReadBytes + 2;

	if (UsbOpen(nDevIndex) == NO_ERROR)
	{
		if (SendReceivePacket(buf, 4, buf, &RecvLength, 500, 1000))
		{
			if (RecvLength == nReadBytes+2 && buf[0] == READ_CMD2 && buf[1] == nReadBytes)
			{
				if (!g_bPEC || CheckRCrc(Addr, byCommand, (PBYTE)buf))
					Status = FTC_SUCCESS;

				*pwValue = *(PWORD)&buf[2];
			}
		}
	}
	//UsbClose();

	if (Status)
		g_strSMBus = "SM Bus ERROR !!";
	else
		g_strSMBus.Empty();
}

void ReadByte(BYTE byCommand, PBYTE pValue)
{
	Status = FTC_INVALID_HANDLE;
	if (g_bHDQ)
		ReadHDQ(byCommand, pValue);
	else
		if (UsbSmbRead(nDevIndex, Addr, byCommand, FALSE, sizeof(BYTE), pValue))
			Status = FTC_SUCCESS;
}

void WriteByte(BYTE byCommand, BYTE byValue)
{
	Status = FTC_INVALID_HANDLE;
	if (g_bHDQ)
		WriteHDQ(byCommand, byValue);
	else
		if (UsbSmbWrite(nDevIndex, Addr, byCommand, FALSE, sizeof(BYTE), &byValue))
			Status = FTC_SUCCESS;
}

void SendByte(BYTE byCommand)
{
	if (UsbSmbWrite(nDevIndex, Addr, byCommand, g_bPEC, 0, NULL))
		Status = FTC_SUCCESS;
	else
		Status = FTC_INVALID_HANDLE;
}

//#undef Addr

void WriteI2C(WORD wCommand)
{
	CString s;

	Status = FTC_INVALID_HANDLE;
	s.Format(_T("%02X%02X : "), HIBYTE(wCommand), LOBYTE(wCommand));
	//if (Status == FTC_SUCCESS)
	if (UsbSmbWrite(nDevIndex, ADDR, HIBYTE(wCommand), FALSE, sizeof(BYTE), (PBYTE)&wCommand))
	{
		Status = FTC_SUCCESS;
		s += "Success";
	}

    TRACE("%s\n", s);
//		((CMainFrame*)AfxGetMainWnd())->m_wndStatusBar.SetPaneText(1, s);
}

BOOL WriteLoc(BYTE cmd, WORD loc, UINT len)
{
	BYTE buf[60];

	if (len > 58)
		len = 58;

    buf[0] = HIBYTE(loc);
    buf[1] = LOBYTE(loc);
    memcpy(&buf[2], sbuf, len);

	return UsbSmbWrite(nDevIndex, ADDR, cmd, FALSE, len + 2, (PBYTE)buf);
}

void WriteF4(UINT loc, UINT len)
{
	Status = FTC_INVALID_HANDLE;
	if (WriteLoc(0xF4, loc, len))
		Status = FTC_SUCCESS;
}

void ReadF5(UINT nReadBytes)
{
	Status = FTC_INVALID_HANDLE;
	if (UsbSmbRead(nDevIndex, ADDR, 0xF5, FALSE, nReadBytes, (PBYTE)sbuf))
		Status = FTC_SUCCESS;
}

void WriteF6(UINT loc, UINT len)
{
	Status = FTC_INVALID_HANDLE;
	if (WriteLoc(0xF6, loc, len))
		Status = FTC_SUCCESS;
}

void SendIoCmd(BYTE byCommand)
{
	Status = FTC_INVALID_HANDLE;
	if (UsbSendIoCmd(nDevIndex, byCommand))
		Status = FTC_SUCCESS;
}

void WriteHDQ(BYTE byCommand, BYTE byValue)
{
	Status = FTC_INVALID_HANDLE;
	if (UsbHdqWrite(nDevIndex, byCommand, byValue))
		Status = FTC_SUCCESS;
}

void ReadHDQ(BYTE byCommand, PBYTE pValue)
{
	Status = FTC_INVALID_HANDLE;
	if (UsbHdqRead(nDevIndex, byCommand, pValue))
		Status = FTC_SUCCESS;
}

void WriteEEPROM(UINT loc)
{
	Status = FTC_INVALID_HANDLE;
	if (UsbEepromWrite(nDevIndex, loc, (PBYTE)sbuf))
		Status = FTC_SUCCESS;
}

int ReadEEPROM(UINT loc)
{
	int nRet = 0;

	Status = FTC_INVALID_HANDLE;

	memset(sbuf, 0, sizeof(sbuf));
	if (UsbEepromRead(nDevIndex, loc, (PBYTE)sbuf))
	{
		nRet = 32;
		for (int i = 0; i < 32; i++)
			TRACE("%02X", sbuf[i]);
		TRACE("\n");
		Status = FTC_SUCCESS;
	}

	return nRet;
}

