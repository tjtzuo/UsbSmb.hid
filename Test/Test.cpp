
// Test.cpp : Defines the class behaviors for the application.
//

#include "stdafx.h"
#include "Test.h"
#include "TestDlg.h"
#include "UsbSmb.h"

#pragma comment(lib, "UsbSmb.lib")

#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// CTestApp

BEGIN_MESSAGE_MAP(CTestApp, CWinApp)
	ON_COMMAND(ID_HELP, &CWinApp::OnHelp)
END_MESSAGE_MAP()


// CTestApp construction

CTestApp::CTestApp()
{
	// TODO: add construction code here,
	// Place all significant initialization in InitInstance
}


// The one and only CTestApp object

CTestApp theApp;


// CTestApp initialization

BOOL CTestApp::InitInstance()
{
	CWinApp::InitInstance();


	// Standard initialization
	// If you are not using these features and wish to reduce the size
	// of your final executable, you should remove from the following
	// the specific initialization routines you do not need
	// Change the registry key under which our settings are stored
	// TODO: You should modify this string to be something appropriate
	// such as the name of your company or organization
	SetRegistryKey(_T("Local AppWizard-Generated Applications"));

	int nDevCount = UsbInit();
	if (nDevCount)
	{
		//WORD wVersion = UsbGetVersion(0);
		//VERIFY(!UsbSendIoCmd(nDevCount - 1, 0x7F));
		//VERIFY(!UsbSendIoCmd(0, 0xFF));
	}

	CTestDlg dlg;
	m_pMainWnd = &dlg;
	INT_PTR nResponse = dlg.DoModal();
	if (nResponse == IDOK)
	{
		// TODO: Place code here to handle when the dialog is
		//  dismissed with OK
	}
	else if (nResponse == IDCANCEL)
	{
		// TODO: Place code here to handle when the dialog is
		//  dismissed with Cancel
	}
	else if (nResponse == -1)
	{
		TRACE(traceAppMsg, 0, "Warning: dialog creation failed, so application is terminating unexpectedly.\n");
	}


#ifndef _AFXDLL
	ControlBarCleanUp();
#endif

	// Since the dialog has been closed, return FALSE so that we exit the
	//  application, rather than start the application's message pump.
	return FALSE;
}

