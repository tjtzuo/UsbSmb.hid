
// TestDlg.h : header file
//

#pragma once


// CTestDlg dialog
class CTestDlg : public CDialog
{
// Construction
public:
	CTestDlg(CWnd* pParent = NULL);	// standard constructor

// Dialog Data
#ifdef AFX_DESIGN_TIME
	enum { IDD = IDD_TEST_DIALOG };
#endif

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV support


// Implementation
protected:
	HICON m_hIcon;

	// Generated message map functions
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()
public:
	afx_msg void OnBnClickedButtonRead();
	afx_msg void OnBnClickedButtonWrite();
	//afx_msg void OnBnClickedButtonReset();
	afx_msg void OnBnClickedButtonReset2();
	afx_msg void OnBnClickedButtonD1d2();
	afx_msg void OnBnClickedButtonReadByte();
	afx_msg void OnBnClickedButtonWriteByte();
	afx_msg void OnBnClickedButtonWriteBatch();
};
