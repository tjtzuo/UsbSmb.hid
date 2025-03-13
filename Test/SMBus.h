
#define FTC_SUCCESS 0 // FT_OK
#define FTC_INVALID_HANDLE 1 // FT_INVALID_HANDLE

typedef ULONG FTC_STATUS;

extern FTC_STATUS Status;
extern char sbuf[100];
extern CString g_strSMBus;
extern BOOL g_bPEC;
extern BOOL g_bHDQ;
extern BYTE Addr;

int ReadBytes(BYTE byCommand);
int ReadBlock(BYTE byCommand);
BOOL ReadBlock(BYTE byCommand, UINT unCount);
void WriteBytes(BYTE byCommand, UINT len = 32);
void WriteBlock(BYTE byCommand, UINT len = 32);
void WriteString(BYTE byCommand, const CString& sData);
void WriteWord(BYTE byCommand, WORD wValue);
void ReadWord(BYTE byCommand, PWORD pwValue);
void Read2(BYTE byCommand, PWORD pwValue);
void ReadByte(BYTE byCommand, PBYTE pValue);
void SendByte(BYTE byCommand);
void WriteByte(BYTE byCommand, BYTE byValue);
void WriteI2C(WORD wCommand);
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
