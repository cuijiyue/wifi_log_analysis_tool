// logDll.cpp : 定义 DLL 应用程序的导出函数。
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

//处理线程的数量
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

	// 步骤1 打开文件FILE_FLAG_WRITE_THROUGH 
	HANDLE hFile = CreateFile(
		szPath,
		GENERIC_READ | GENERIC_WRITE,// 如果要映射文件：此处必设置为只读(GENERIC_READ)或读写
		0, // 此设为打开文件的任何尝试均将失败
		NULL,
		OPEN_EXISTING,
		FILE_ATTRIBUTE_NORMAL, //|FILE_FLAG_WRITE_THROUGH,【解1】
		NULL);
	if (hFile != INVALID_HANDLE_VALUE)// 文件打开失败返回句柄为-1
	{
		printf("文件打开成功\n");
	}
	else {
		printf("文件打开失败！error:%d\n", GetLastError());
	}

	// 步骤2 建立内存映射文件
	DWORD dwFileSize = GetFileSize(hFile, NULL);
	printf("文件大小为：%d\n", dwFileSize);
	HANDLE hFileMap = CreateFileMapping(
		hFile, // 如果这值为INVALID_HANDLE_VALUE,是合法的，上步一定测试啊
		NULL, // 默认安全性
		PAGE_READWRITE, // 可读写
		0, // 2个32位数示1个64位数，最大文件字节数，
		// 高字节，文件大小小于4G时，高字节永远为0
		0,//dwFileSize, // 此为低字节，也就是最主要的参数，如果为0，取文件真实大小
		NULL);
	if (hFileMap != NULL) {
		printf("内存映射文件创建成功~!\n");
	}
	else {
		printf("内存映射文件创建失败~！\n");
		goto mapfail;
	}

	// 步骤3：将文件数据映射到进程的地址空间
	PVOID pvFile = MapViewOfFile( //pvFile就是得到的指针，用它来直接操作文件
		hFileMap,
		FILE_MAP_WRITE, // 可写
		0, // 文件指针头位置 高字节
		0, // 文件指针头位置 低字节 必为分配粒度的整倍数,windows的粒度为64K
		0); // 要映射的文件尾，如果为0，则从指针头到真实文件尾
	if (pvFile != NULL) {
		printf("文件数据映射到进程的地址成功~!\n");
	}
	else {
		printf("文件数据映射到进程的地址失败~!\n");
		goto pfail;
	}

	// 步骤4: 像操作内存一样操作文件,
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

	printf("所有线程已结束\n");


	// 步骤5: 相关的释放工作
pfail:
	UnmapViewOfFile(pvFile); // 释放内存映射文件的头指针
mapfail:
	CloseHandle(hFileMap); // 内存映射文件句柄
filefail:
	CloseHandle(hFile); // 关闭文件

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

//返回值是一个结构体数组，每个结构体存放log的种类
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