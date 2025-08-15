
#define FTC_SUCCESS 0 // FT_OK
#define FTC_INVALID_HANDLE 1 // FT_INVALID_HANDLE

typedef ULONG FTC_STATUS;

extern FTC_STATUS Status;
extern BYTE sbuf[100];
extern CString g_strSMBus;
extern BOOL g_bPEC;
extern BOOL g_bHDQ;
extern BYTE Addr;

int ReadBytes(BYTE byCommand, PBYTE pBuf = NULL, UINT nReadBytes = 32);
int ReadBlock(BYTE byCommand);
BOOL ReadBlock(BYTE byCommand, UINT unCount);
void WriteBytes(BYTE byCommand, UINT len = 32);
void WriteBlock(BYTE byCommand, UINT len = 32);
void WriteString(BYTE byCommand, const CString& sData);
void WriteWord(BYTE byCommand, WORD wValue);
void ReadWord(BYTE byCommand, PWORD pwValue);
//void Read2(BYTE byCommand, PWORD pwValue);
void ReadByte(BYTE byCommand, PBYTE pValue);
void SendByte(BYTE byCommand);
void WriteByte(BYTE byCommand, BYTE byValue);
void WriteI2C(WORD wCommand);
BOOL WriteLoc(BYTE cmd, WORD loc, UINT len);
void WriteF4(UINT loc, UINT len);
void WriteF6(UINT loc, UINT len);
void ReadF5(UINT nReadBytes);
int ReadEEPROM(UINT loc);
void WriteEEPROM(UINT loc);
void SendIoCmd(BYTE byCommand);
void WriteHDQ(BYTE byCommand, BYTE byValue);
void ReadHDQ(BYTE byCommand, PBYTE pValue);
int htoi(const TCHAR *s);

#define ADDR 0x16

#ifdef _DEBUG
#undef TRACE
inline void TRACE(PCSTR str, ...)
{
	va_list args;
	va_start(args, str);
	CHAR szTemp[256];
	if (_vsnprintf_s(szTemp, _countof(szTemp), _TRUNCATE, str, args) > 0)
		OutputDebugStringA(szTemp);
	va_end(args);
}
#endif
