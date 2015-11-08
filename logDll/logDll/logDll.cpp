// logDll.cpp : ���� DLL Ӧ�ó���ĵ���������
//

#include "stdafx.h"
#include "logDll.h"

#include <windows.h>
#include <process.h>
#include <string.h>
#include <stdio.h>
#include <stdlib.h>
#include <tchar.h>

#include "logMethod.h"

//�����̵߳�����
#define THREAD_NUM 13
HANDLE  handle[THREAD_NUM];



void CharToTchar(const char * _char, TCHAR * tchar)
{
	int iLength;

	iLength = MultiByteToWideChar(CP_ACP, 0, _char, strlen(_char) + 1, NULL, 0);
	MultiByteToWideChar(CP_ACP, 0, _char, strlen(_char) + 1, tchar, iLength);
}
int analysis(char *file_path)
{
	printf("C DLL, filepath:%s\n", file_path);
	TCHAR szPath[100];
	CharToTchar(file_path, szPath);

	// ����1 ���ļ�FILE_FLAG_WRITE_THROUGH 
	HANDLE hFile = CreateFile(
		szPath,
		GENERIC_READ | GENERIC_WRITE,// ���Ҫӳ���ļ����˴�������Ϊֻ��(GENERIC_READ)���д
		0, // ����Ϊ���ļ����κγ��Ծ���ʧ��
		NULL,
		OPEN_EXISTING,
		FILE_ATTRIBUTE_NORMAL, //|FILE_FLAG_WRITE_THROUGH,����1��
		NULL);
	if (hFile != INVALID_HANDLE_VALUE)// �ļ���ʧ�ܷ��ؾ��Ϊ-1
	{
		printf("�ļ��򿪳ɹ�\n");
	}
	else {
		printf("�ļ���ʧ�ܣ�error:%d\n", GetLastError());
	}

	// ����2 �����ڴ�ӳ���ļ�
	DWORD dwFileSize = GetFileSize(hFile, NULL);
	printf("�ļ���СΪ��%d\n", dwFileSize);
	HANDLE hFileMap = CreateFileMapping(
		hFile, // �����ֵΪINVALID_HANDLE_VALUE,�ǺϷ��ģ��ϲ�һ�����԰�
		NULL, // Ĭ�ϰ�ȫ��
		PAGE_READWRITE, // �ɶ�д
		0, // 2��32λ��ʾ1��64λ��������ļ��ֽ�����
		// ���ֽڣ��ļ���СС��4Gʱ�����ֽ���ԶΪ0
		0,//dwFileSize, // ��Ϊ���ֽڣ�Ҳ��������Ҫ�Ĳ��������Ϊ0��ȡ�ļ���ʵ��С
		NULL);
	if (hFileMap != NULL) {
		printf("�ڴ�ӳ���ļ������ɹ�~!\n");
	}
	else {
		printf("�ڴ�ӳ���ļ�����ʧ��~��\n");
		goto mapfail;
	}

	// ����3�����ļ�����ӳ�䵽���̵ĵ�ַ�ռ�
	PVOID pvFile = MapViewOfFile( //pvFile���ǵõ���ָ�룬������ֱ�Ӳ����ļ�
		hFileMap,
		FILE_MAP_WRITE, // ��д
		0, // �ļ�ָ��ͷλ�� ���ֽ�
		0, // �ļ�ָ��ͷλ�� ���ֽ� ��Ϊ�������ȵ�������,windows������Ϊ64K
		0); // Ҫӳ����ļ�β�����Ϊ0�����ָ��ͷ����ʵ�ļ�β
	if (pvFile != NULL) {
		printf("�ļ�����ӳ�䵽���̵ĵ�ַ�ɹ�~!\n");
	}
	else {
		printf("�ļ�����ӳ�䵽���̵ĵ�ַʧ��~!\n");
		goto pfail;
	}

	// ����4: ������ڴ�һ�������ļ�,
	handle[0] = (HANDLE)_beginthreadex(NULL, 0, monitor_cmd_thread, pvFile, 0, NULL);
	handle[1] = (HANDLE)_beginthreadex(NULL, 0, monitor_rebck_thread, pvFile, 0, NULL);
	handle[2] = (HANDLE)_beginthreadex(NULL, 0, driver_cmd_thread, pvFile, 0, NULL);
	handle[3] = (HANDLE)_beginthreadex(NULL, 0, driver_rebck_thread, pvFile, 0, NULL);
	//WaitForMultipleObjects(4, handle, TRUE, INFINITE);

	handle[4] = (HANDLE)_beginthreadex(NULL, 0, wps_sta_thread, pvFile, 0, NULL);
	handle[5] = (HANDLE)_beginthreadex(NULL, 0, wps_scan_result_thread, pvFile, 0, NULL);	
	handle[6] = (HANDLE)_beginthreadex(NULL, 0, wifi_framwork_sta_thread, pvFile, 0, NULL);
	handle[7] = (HANDLE)_beginthreadex(NULL, 0, screen_sta_thread, pvFile, 0, NULL);
	//WaitForMultipleObjects(4, handle+4, TRUE, INFINITE);

	handle[8] = (HANDLE)_beginthreadex(NULL, 0, wifisetting_sta_thread, pvFile, 0, NULL);
	handle[9] = (HANDLE)_beginthreadex(NULL, 0, wifi_connected_sta_thread, pvFile, 0, NULL);
	handle[10] = (HANDLE)_beginthreadex(NULL, 0, wps_selectbss_thread, pvFile, 0, NULL);
	handle[11] = (HANDLE)_beginthreadex(NULL, 0, dhcpcd_thread, pvFile, 0, NULL);
	handle[12] = (HANDLE)_beginthreadex(NULL, 0, wifi_openclose_thread, pvFile, 0, NULL);
	WaitForMultipleObjects(THREAD_NUM, handle, TRUE, INFINITE);

	printf("�����߳��ѽ���\n");


	// ����5: ��ص��ͷŹ���
pfail:
	UnmapViewOfFile(pvFile); // �ͷ��ڴ�ӳ���ļ���ͷָ��
mapfail:
	CloseHandle(hFileMap); // �ڴ�ӳ���ļ����
filefail:
	CloseHandle(hFile); // �ر��ļ�

	return 0;
}

int main(int argc, char *argv[])
{
	//init();
	//analysis("demo.txt");

	//logInfoList* p_list = getList();
	//logInfo *p_array = logInfoSort(p_list);
	//int lines = getLogLines();
	//showlog(p_array, lines);
	//close();
	AnalysisLogFile("D:\\BUG\\2015_06_04_16_18_04\\aplog\\LOGANALYSIS");
	getchar();
}

//����ֵ��һ���ṹ�����飬ÿ���ṹ����log������
LOGDLL_API char* __stdcall AnalysisLogFile(char* path)
{
	init();
	printf("DLL, log path:%s\n", path);
	//TCHAR *filePath = path;
	//printf("DLL, log path:%s\n", filePath);
	analysis(path);
	logInfoList* p_list = getList();
	logInfo *p_array = logInfoSort(p_list);
	close();

	return (char*)p_array;
}

LOGDLL_API int __stdcall getLen(){
	int lines = getLogLines();
	printf("DLL, log lines:%d\n", lines);
	return lines;
}