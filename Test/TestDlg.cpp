
// TestDlg.cpp : implementation file
//

#include "stdafx.h"
#include "Test.h"
#include "TestDlg.h"
#include "UsbSmb.h"
#include "Smbus.h"


#ifdef _DEBUG
#define new DEBUG_NEW
#endif



// CTestDlg dialog



CTestDlg::CTestDlg(CWnd* pParent /*=NULL*/)
	: CDialog(IDD_TEST_DIALOG, pParent)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
}

void CTestDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialog::DoDataExchange(pDX);
}

BEGIN_MESSAGE_MAP(CTestDlg, CDialog)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDC_BUTTON_READ, &CTestDlg::OnBnClickedButtonRead)
	ON_BN_CLICKED(IDC_BUTTON_WRITE, &CTestDlg::OnBnClickedButtonWrite)
	//ON_BN_CLICKED(IDC_BUTTON_RESET, &CTestDlg::OnBnClickedButtonReset)
	ON_BN_CLICKED(IDC_BUTTON_RESET2, &CTestDlg::OnBnClickedButtonReset2)
	ON_BN_CLICKED(IDC_BUTTON_D1D2, &CTestDlg::OnBnClickedButtonD1d2)
	ON_BN_CLICKED(IDC_BUTTON_READ_BYTE, &CTestDlg::OnBnClickedButtonReadByte)
	ON_BN_CLICKED(IDC_BUTTON_WRITE_BYTE, &CTestDlg::OnBnClickedButtonWriteByte)
	ON_BN_CLICKED(IDC_BUTTON_WRITE_BATCH, &CTestDlg::OnBnClickedButtonWriteBatch)
	ON_BN_CLICKED(IDC_BUTTON_READ2, &CTestDlg::OnBnClickedButtonRead2)
END_MESSAGE_MAP()


// CTestDlg message handlers

BOOL CTestDlg::OnInitDialog()
{
	CDialog::OnInitDialog();

	// Set the icon for this dialog.  The framework does this automatically
	//  when the application's main window is not a dialog
	SetIcon(m_hIcon, TRUE);			// Set big icon
	SetIcon(m_hIcon, FALSE);		// Set small icon

	// TODO: Add extra initialization here
	WORD wVersion = UsbGetVersion(0);
	CString str;
	str.Format(_T("USB-HID Device Version : %d.%02x"), HIBYTE(wVersion), LOBYTE(wVersion));
	SetDlgItemText(IDC_MSG, str);
	SetDlgItemText(IDC_EDIT_CMD, L"00");
	SetDlgItemText(IDC_EDIT_DATA, L"00");
	SetDlgItemText(IDC_EDIT_LEN, L"12");

	return TRUE;  // return TRUE  unless you set the focus to a control
}

// If you add a minimize button to your dialog, you will need the code below
//  to draw the icon.  For MFC applications using the document/view model,
//  this is automatically done for you by the framework.

void CTestDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // device context for painting

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// Center icon in client rectangle
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// Draw the icon
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialog::OnPaint();
	}
}

// The system calls this function to obtain the cursor to display while the user drags
//  the minimized window.
HCURSOR CTestDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void CTestDlg::OnBnClickedButtonRead()
{
	// TODO: Add your control notification handler code here
	CString str;
	GetDlgItemText(IDC_EDIT_CMD, str);
	BYTE byCommand = htoi(str);
	BYTE byValue;
	ReadByte(byCommand, &byValue);
	if (Status == FTC_SUCCESS)
	{
		str.Format(_T("%02X"), byValue);
		SetDlgItemText( IDC_MSG, str );
	}
	else
		SetDlgItemText( IDC_MSG, L"Read Fail" );
}


void CTestDlg::OnBnClickedButtonWrite()
{
	// TODO: Add your control notification handler code here
	CString str;
	GetDlgItemText(IDC_EDIT_CMD, str);
	BYTE byCommand = htoi(str);
	GetDlgItemText(IDC_EDIT_DATA, str);
    BYTE byValue = htoi(str);
	WriteByte(byCommand, byValue);
	if (Status == FTC_SUCCESS)
        SetDlgItemText( IDC_MSG, L"Write Success" );
    else
        SetDlgItemText( IDC_MSG, L"Write Fail" );
}

/*
void CTestDlg::OnBnClickedButtonReset()
{
	// TODO: Add your control notification handler code here
	//SetDlgItemText(IDC_EDIT_CMD, "06");
	//SetDlgItemText(IDC_EDIT_DATA, "80");
	if (WriteByte(0x06, 0x80))
        SetDlgItemText( IDC_MSG, "Reset Success" );
    else
        SetDlgItemText( IDC_MSG, "Reset Fail" );
}
*/

void CTestDlg::OnBnClickedButtonReset2()
{
	// TODO: Add your control notification handler code here
	UsbHidReset();
}


void CTestDlg::OnBnClickedButtonD1d2()
{
	// TODO: Add your control notification handler code here
	if (UsbOpen(0) == NO_ERROR)
	{
		BYTE buf[65] = { 0, 0x80, 0 };
		DWORD RecvLength = sizeof(buf);
		VERIFY(SendReceivePacket(buf, RecvLength, buf, &RecvLength, 0, 1) == 1);
		VERIFY(buf[1] == 0x80 && buf[2] == 0);
	}
	UsbClose();
}


void CTestDlg::OnBnClickedButtonReadByte()
{
	// TODO: Add your control notification handler code here
	CString str;
	GetDlgItemText(IDC_EDIT_ADDR, str);
	BYTE byCommand = htoi(str);
	BYTE byValue;
	if (UsbEedataRead(0, byCommand, &byValue))
	{
		str.Format(_T("%02X"), byValue);
		SetDlgItemText(IDC_EDIT_BYTE, str );
        SetDlgItemText( IDC_MSG, L"Read Success" );
	}
	else
		SetDlgItemText( IDC_MSG, L"Read Fail" );
}


void CTestDlg::OnBnClickedButtonWriteByte()
{
	// TODO: Add your control notification handler code here
	CString str;
	GetDlgItemText(IDC_EDIT_ADDR, str);
	BYTE byCommand = htoi(str);
	GetDlgItemText(IDC_EDIT_BYTE, str);
    BYTE byValue = htoi(str);
	if (UsbEedataWrite(0, byCommand, byValue))
        SetDlgItemText( IDC_MSG, L"Write Success" );
    else
        SetDlgItemText( IDC_MSG, L"Write Fail" );
}


void CTestDlg::OnBnClickedButtonWriteBatch()
{
	// TODO: Add your control notification handler code here
	CFileDialog file(TRUE, _T(".ini"), _T("reg"), OFN_HIDEREADONLY | OFN_FILEMUSTEXIST, _T("INI File(*.ini)|*.ini||"), this);
	if (file.DoModal() == IDOK)
	{
		CWaitCursor cursor;
		//BatchEepromWrite(file.GetPathName());
	}
}


void CTestDlg::OnBnClickedButtonRead2()
{
	// TODO: Add your control notification handler code here
	CString str;
	GetDlgItemText(IDC_EDIT_CMD, str);
	BYTE byCommand = htoi(str);
	BYTE vals[32];
	int len = min(GetDlgItemInt(IDC_EDIT_LEN), 32);
	if (ReadBytes(byCommand, vals, len))
	{
		PWSTR szBlockData = str.GetBuffer(100);
		for (int i = 0; i < len; i++)
			wsprintf(&szBlockData[i * 3], L"%02X ", vals[i]);
		str.ReleaseBuffer();
		SetDlgItemText( IDC_MSG, str );
	}
	else
		SetDlgItemText( IDC_MSG, L"Read Fail" );
}
