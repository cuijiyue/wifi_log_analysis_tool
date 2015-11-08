#include "stdafx.h"

#include <windows.h>
#include <process.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "logMethod.h"

//全局变量，链表
logInfoList *p_logInfoList = NULL;
logInfoList *list_tail = NULL;
int logLines = 0;
//互斥量
HANDLE  hMutex;


void init()
{
	p_logInfoList = (logInfoList*)malloc(sizeof(logInfoList));
	list_tail = p_logInfoList;
	list_tail->p_logInfo = NULL;
	list_tail->pNext = NULL;
	//创建互斥量
	hMutex = CreateMutex(NULL, FALSE, NULL);
}

void close()
{
	//销毁互斥量
	CloseHandle(hMutex);
	//freeList(p_logInfoList);
}

void addList(logInfo *p)
{
	//等待互斥量
	WaitForSingleObject(hMutex,INFINITE);
	//printf("add list>>type:%d, time:%s, cmd:%s\n", p->type, p->time, p->cmd);
	list_tail->p_logInfo = p;	
	logInfoList *temp = (logInfoList*)malloc(sizeof(logInfoList));
	list_tail->pNext = temp;
	list_tail = temp;
	list_tail->p_logInfo = NULL;
	list_tail->pNext = NULL;
	
	//释放互斥量
	ReleaseMutex(hMutex);
}

logInfoList* getList(){
	return p_logInfoList;
}

void showlog(logInfo * logArray, int len){

	for(int i=0; i<len; i++)
	{
		logInfo *temp = logArray + i;
		if (temp->type == 0x06) {
			scan_result_list *p_scan_result = (scan_result_list*)temp->cmd;
			printf("line:%d, type:%d, time value:%d, time:%s\n", temp->line, temp->type, temp->time_v, temp->time);
			while (p_scan_result->pNext != NULL)
			{
				printf("%s\n", p_scan_result->result);
				p_scan_result = p_scan_result->pNext;
			}
		}else {
			printf("line:%d, type:%d, time value:%d, time:%s, cmd:%s\n", temp->line, temp->type, temp->time_v, temp->time, temp->cmd);
		}
	}
}


//qsort所需的比较函数,按行排序
int logCmp(const void *a,const void *b)
{
	return ((logInfo*)a)->line - ((logInfo*)b)->line;
}

//生成排序好的数组
logInfo *logInfoSort(logInfoList* plist)
{
	logInfoList* p = plist;
	//获取链表长度，然后放到数组中，方便排序
	int len = 0;
	while(p->pNext != NULL){
		len++;
		p = p->pNext;
	}
	logLines = len;
	printf("log array len:%d\n", len);
	logInfo *logInfoAtrray = (logInfo*)malloc(sizeof(logInfo) * len);
	p = plist;
	for(int i = 0; i<len; i++){
		//printf("log array line:%d\n", i);
		logInfoAtrray[i] = *(p->p_logInfo);
		p = p->pNext;
	}
	printf("log array is ready!\n");
	
	//释放链表内存,链表的尾未存储信息
	p = plist;
	while(p != NULL){		
		plist = p->pNext;
		free(p);
		p = plist;
	}
	printf("log list is free\n");

	qsort(logInfoAtrray, logLines, sizeof(logInfo),logCmp);


	return logInfoAtrray;
}

int getLogLines(){
	return logLines;
}

char* get_time(char *log) {
	//log中时间长度为12个字节 "04-23 08:40:19.100 12613 12613 D wpa_supplicant: nl80211:"
	char *time = (char*)malloc(13);
	strncpy(time, log+6, 12);
	time[12] = '\0';
	return time;
}

//计算时间的值，便于后续排序
int time_value(char *time) {
	//时间格式为08:40:19.100
	//time[2] = '\0';
	//time[5] = '\0';
	//time[8] = '\0';
	int tmp = atoi(time) * 3600000;
	tmp += atoi(time+3) * 60000;
	tmp += atoi(time+6) * 1000;
	tmp += atoi(time+9);
	return tmp;
}

//检测是否包含字符串，后续会更改方法
int is_belong(char *log, char *sign) {
	if(strstr(log, sign) != NULL)
		return 1;
	return 0;
}




/**
*以下为处理线程，每一个线程只处理一个log type
*/
//
//type 0x11
//标记WifiStateMachine: invokeEnterMethods: DisconnectedState, 进入上层状态机
unsigned int __stdcall wifi_framwork_sta_thread(void *pPM)
{
	char *pfile = (char *)pPM;

	//按行循环处理
	char *plog;
	char *pTemp;
	int len = 0;
	int line = 0;
	while (1) {
		pTemp = strchr(pfile, '\n');
		if (pTemp == NULL)
			break;
		line++; //统计行数
		len = pTemp - pfile;
		plog = (char*)malloc(len + 1);
		strncpy(plog, pfile, len);
		plog[len] = '\0';
		pfile = pTemp + 1;
		//printf("线程编号id为%d, len:%d >>>>>>%s<<<<<<<<<<<\n", GetCurrentThreadId(), len, plog);
		if (is_belong(plog, "WifiStateMachine: invokeEnterMethods: "))
		{
			//获取命令内容，最后一个单词
			//只有一对单引号，正向查找一次，然后再反向查找一次
			char *start = strrchr(plog, ' ');
			if (start == NULL){
				free(plog);
				continue;
			}
			char *end = plog + strlen(plog);
			char *cmd = (char*)malloc(end - start);
			strncpy(cmd, start + 1, end - start - 1);
			cmd[end - start - 1] = '\0';
			char *time = get_time(plog);
			int time_v = time_value(time);

			//生成结构体，加载到列表中
			logInfo *p_logInfo = (logInfo*)malloc(sizeof(logInfo));
			p_logInfo->type = 0x11;
			p_logInfo->line = line;
			p_logInfo->time_v = time_v;
			p_logInfo->time = time;
			p_logInfo->cmd = cmd;

			addList(p_logInfo);
		}
		free(plog);
	}
	return 0;
}

//type 0x12
//标记AlarmManager: >>> SCREEN_ON <<<, 屏幕状态， ON屏幕亮， OFF屏幕灭
unsigned int __stdcall screen_sta_thread(void *pPM)
{
	char *pfile = (char *)pPM;

	//按行循环处理
	char *plog;
	char *pTemp;
	int len = 0;
	int line = 0;
	while (1) {
		pTemp = strchr(pfile, '\n');
		if (pTemp == NULL)
			break;
		line++; //统计行数
		len = pTemp - pfile;
		plog = (char*)malloc(len + 1);
		strncpy(plog, pfile, len);
		plog[len] = '\0';
		pfile = pTemp + 1;
		//printf("线程编号id为%d, len:%d >>>>>>%s<<<<<<<<<<<\n", GetCurrentThreadId(), len, plog);
		if (is_belong(plog, ": >>> SCREEN_"))
		{
			//改变命令：使能为OK， 不使能为ON
			char *cmd = (char*)malloc(3);
			if (is_belong(plog, "SCREEN_ON"))
			{
				strcpy(cmd, "OK");
				cmd[2] = '\0';
			}
			else if (is_belong(plog, "SCREEN_OFF"))
			{
				strcpy(cmd, "NO");
				cmd[2] = '\0';
			}			
			else{
				free(plog);
				continue;
			}
			char *time = get_time(plog);
			int time_v = time_value(time);

			//生成结构体，加载到列表中
			logInfo *p_logInfo = (logInfo*)malloc(sizeof(logInfo));
			p_logInfo->type = 0x12;
			p_logInfo->line = line;
			p_logInfo->time_v = time_v;
			p_logInfo->time = time;
			p_logInfo->cmd = cmd;

			addList(p_logInfo);
		}
		free(plog);
	}
	return 0;
}

//type 0x13
//进入wifi设置：ActivityManager: Displayed com.android.wifi/.Settings$WifiSettingsActivity:
//退出wifi设置：ActivityManager: Complete pause: ActivityRecord{13bddbe9 u0 com.android.wifi/.Settings$WifiSettingsActivity t205}
unsigned int __stdcall wifisetting_sta_thread(void *pPM)
{
	char *pfile = (char *)pPM;

	//按行循环处理
	char *plog;
	char *pTemp;
	int len = 0;
	int line = 0;
	while (1) {
		pTemp = strchr(pfile, '\n');
		if (pTemp == NULL)
			break;
		line++; //统计行数
		len = pTemp - pfile;
		plog = (char*)malloc(len + 1);
		strncpy(plog, pfile, len);
		plog[len] = '\0';
		pfile = pTemp + 1;
		//printf("线程编号id为%d, len:%d >>>>>>%s<<<<<<<<<<<\n", GetCurrentThreadId(), len, plog);
		if (is_belong(plog, "com.android.wifi/.Settings$WifiSettingsActivity"))
		{
			//改变命令：使能为OK， 不使能为ON
			char *cmd = (char*)malloc(3);
			if (is_belong(plog, "Displayed"))
			{
				strcpy(cmd, "OK");
				cmd[2] = '\0';
			}
			else if (is_belong(plog, "Complete pause"))
			{
				strcpy(cmd, "NO");
				cmd[2] = '\0';
			}
			else{
				free(plog);
				continue;
			}
			char *time = get_time(plog);
			int time_v = time_value(time);

			//生成结构体，加载到列表中
			logInfo *p_logInfo = (logInfo*)malloc(sizeof(logInfo));
			p_logInfo->type = 0x13;
			p_logInfo->line = line;
			p_logInfo->time_v = time_v;
			p_logInfo->time = time;
			p_logInfo->cmd = cmd;

			addList(p_logInfo);
		}
		free(plog);
	}
	return 0;
}

//type 0x14
//当前连接网络的数据，ssid，rssid，bssid等：WifiStateMachine:  L2ConnectedState !CMD_RSSI_POLL 1 0 "lenovo-internet" 18:64:72:e9:75:01 rssi=-127 f=-1。。。。。。。。
unsigned int __stdcall wifi_connected_sta_thread(void *pPM)
{
	char *pfile = (char *)pPM;

	//按行循环处理
	char *plog;
	char *pTemp;
	int len = 0;
	int line = 0;
	while (1) {
		pTemp = strchr(pfile, '\n');
		if (pTemp == NULL)
			break;
		line++; //统计行数
		len = pTemp - pfile;
		plog = (char*)malloc(len + 1);
		strncpy(plog, pfile, len);
		plog[len] = '\0';
		pfile = pTemp + 1;
		//printf("线程编号id为%d, len:%d >>>>>>%s<<<<<<<<<<<\n", GetCurrentThreadId(), len, plog);
		if (is_belong(plog, "WifiStateMachine:  L2ConnectedState !CMD_RSSI_POLL"))
		{
			char *start = strchr(plog, '\"');
			if (start == NULL){
				free(plog);
				continue;
			}
			char *end = plog + strlen(plog);
			char *cmd = (char*)malloc(end - start + 1);
			strncpy(cmd, start, end - start);
			cmd[end - start] = '\0';
			char *time = get_time(plog);
			int time_v = time_value(time);

			//生成结构体，加载到列表中
			logInfo *p_logInfo = (logInfo*)malloc(sizeof(logInfo));
			p_logInfo->type = 0x14;
			p_logInfo->line = line;
			p_logInfo->time_v = time_v;
			p_logInfo->time = time;
			p_logInfo->cmd = cmd;

			addList(p_logInfo);
		}
		free(plog);
	}
	return 0;
}



//type 0x01
//标记wpa_supplicant: wlan0: Control interface command 'RECONNECT'，''内为命令内容
unsigned int __stdcall monitor_cmd_thread(void *pPM)
{
	char *pfile = (char *)pPM;

	//按行循环处理
	char *plog;
	char *pTemp;
	int len = 0;
	int line = 0;
	while(1) {
		pTemp = strchr(pfile, '\n');
		if(pTemp == NULL)
			break;
		line++; //统计行数
		len = pTemp - pfile;
		plog = (char*)malloc(len+1);
		strncpy(plog, pfile, len);
		plog[len] = '\0';
		pfile = pTemp + 1;
		//printf("线程编号id为%d, len:%d >>>>>>%s<<<<<<<<<<<\n", GetCurrentThreadId(), len, plog);
		if(is_belong(plog, "wpa_supplicant: wlan0: Control interface command")) 
		{
			//获取命令内容，最后单引号内字符串
			//只有一对单引号，正向查找一次，然后再反向查找一次
			char *start = strchr(plog, '\'');
			char *end = strrchr(plog, '\'');
			if (start == NULL || end == NULL){
				free(plog);
				continue;
			}
			char *cmd = (char*)malloc(end-start);
			strncpy(cmd, start+1, end-start-1);
			cmd[end-start-1] = '\0';
			char *time = get_time(plog);
			int time_v = time_value(time);

			//生成结构体，加载到列表中
			logInfo *p_logInfo = (logInfo*)malloc(sizeof(logInfo));
			p_logInfo->type = 0x01;
			p_logInfo->line = line;
			p_logInfo->time_v = time_v;
			p_logInfo->time = time;
			p_logInfo->cmd = cmd;
			
			addList(p_logInfo);
		}
		free(plog);
	}
	return 0;
}


//type 0x02
//wpa_supplicant: CTRL_IFACE monitor sent successfully to /data/misc/wifi/sockets/wpa_ctrl_1161-10\x00, msg:[]
unsigned int __stdcall monitor_rebck_thread(void *pPM)
{
	char *pfile = (char *)pPM;

	//按行循环处理
	char *plog;
	char *pTemp;
	int len = 0;
	int line = 0;
	while(1) {
		pTemp = strchr(pfile, '\n');
		if(pTemp == NULL)
			break;
		line++;
		len = pTemp - pfile;
		plog = (char*)malloc(len + 1);
		strncpy(plog, pfile, len);
		plog[len] = '\0';
		pfile = pTemp + 1;
		//printf("线程编号id为%d, len:%d >>>>>>%s<<<<<<<<<<<\n", GetCurrentThreadId(), len, plog);
		if(is_belong(plog, "CTRL_IFACE monitor sent successfully to")) 
		{
			//获取命令内容，最后单引号内字符串
			//正反搜索两次括号
			char *start = strchr(plog, '[');
			char *end = strrchr(plog, ']');
			if (start == NULL || end == NULL){
				free(plog);
				continue;
			}
			char *cmd = (char*)malloc(end-start);
			strncpy(cmd, start+1, end-start-1);
			cmd[end-start-1] = '\0';
			char *time = get_time(plog);
			int time_v = time_value(time);

			//生成结构体，加载到列表中
			logInfo *p_logInfo = (logInfo*)malloc(sizeof(logInfo));
			p_logInfo->type = 0x02;
			p_logInfo->line = line;
			p_logInfo->time_v = time_v;
			p_logInfo->time = time;
			p_logInfo->cmd = cmd;
			
			addList(p_logInfo);
		}
		free(plog);
	}	
	return 0;
}


//type 0x03
//标记wpa_supplicant: nl80211: Drv Event 46 (NL80211_CMD_CONNECT) received for wlan0
unsigned int __stdcall driver_cmd_thread(void *pPM)
{
	char *pfile = (char *)pPM;

	//按行循环处理
	char *plog;
	char *pTemp;
	int len = 0;
	int line = 0;
	while(1) {
		pTemp = strchr(pfile, '\n');
		if(pTemp == NULL)
			break;
		line++;
		len = pTemp - pfile;
		plog = (char*)malloc(len + 1);
		strncpy(plog, pfile, len);
		plog[len] = '\0';
		pfile = pTemp + 1;
		//printf("线程编号id为%d, len:%d >>>>>>%s<<<<<<<<<<<\n", GetCurrentThreadId(), len, plog);
		if(is_belong(plog, "nl80211: Drv Event ")) 
		{
			//获取命令内容，最后单引号内字符串
			//正反搜索两次括号
			char *start = strchr(plog, '(');
			char *end = strrchr(plog, ')');
			if (start == NULL || end == NULL){
				free(plog);
				continue;
			}
			char *cmd = (char*)malloc(end-start);
			strncpy(cmd, start+1, end-start-1);
			cmd[end-start-1] = '\0';
			char *time = get_time(plog);
			int time_v = time_value(time);

			//生成结构体，加载到列表中
			logInfo *p_logInfo = (logInfo*)malloc(sizeof(logInfo));
			p_logInfo->type = 0x03;
			p_logInfo->line = line;
			p_logInfo->time_v = time_v;
			p_logInfo->time = time;
			p_logInfo->cmd = cmd;
			
			addList(p_logInfo);
		}
		free(plog);
	}
	return 0;
}


//type 0x04
//标记wpa_supplicant: wlan0: Event SCAN_STARTED (49) received, 
unsigned int __stdcall driver_rebck_thread(void *pPM)
{
	char *pfile = (char *)pPM;

	//按行循环处理
	char *plog;
	char *pTemp;
	int len = 0;
	int line = 0;
	while(1) {
		pTemp = strchr(pfile, '\n');
		if(pTemp == NULL)
			break;
		line++;
		len = pTemp - pfile;
		plog = (char*)malloc(len + 1);
		strncpy(plog, pfile, len);
		plog[len] = '\0';
		pfile = pTemp + 1;
		//printf("线程编号id为%d, len:%d >>>>>>%s<<<<<<<<<<<\n", GetCurrentThreadId(), len, plog);
		if(is_belong(plog, "wpa_supplicant: wlan0: Event ")) 
		{
			//获取命令内容，命令就是匹配字符串的后一个单词
			char *start = strstr(plog, "Event ");
			if (start == NULL){
				free(plog);
				continue;
			}
			start += strlen("Event") + 1;
			char *end = strchr(start, ' ');
			if (end == NULL){
				free(plog);
				continue;
			}

			char *cmd = (char*)malloc(end - start + 1);
			strncpy(cmd, start, end - start);
			cmd[end - start] = '\0';
			char *time = get_time(plog);
			int time_v = time_value(time);

			//生成结构体，加载到列表中
			logInfo *p_logInfo = (logInfo*)malloc(sizeof(logInfo));
			p_logInfo->type = 0x04;
			p_logInfo->line = line;
			p_logInfo->time_v = time_v;
			p_logInfo->time = time;
			p_logInfo->cmd = cmd;
			
			addList(p_logInfo);
		}
		free(plog);
	}
	return 0;
}


//type 0x05
//标记wpa_supplicant: wlan0: State: ASSOCIATING -> ASSOCIATED
unsigned int __stdcall wps_sta_thread(void *pPM)
{
	char *pfile = (char *)pPM;

	//按行循环处理
	char *plog;
	char *pTemp;
	int len = 0;
	int line = 0;
	while(1) {
		pTemp = strchr(pfile, '\n');
		if(pTemp == NULL)
			break;
		line++;
		len = pTemp - pfile;
		plog = (char*)malloc(len + 1);
		strncpy(plog, pfile, len);
		plog[len] = '\0';
		pfile = pTemp + 1;
		//printf("线程编号id为%d, len:%d >>>>>>%s<<<<<<<<<<<\n", GetCurrentThreadId(), len, plog);
		if(is_belong(plog, "wpa_supplicant: wlan0: State:")) 
		{
			//获取命令内容，最后单引号内字符串
			//从最后一个冒号开始
			char *start = strrchr(plog, ':');
			if (start == NULL){
				free(plog);
				continue;
			}
			start += 2;
			int len = strlen(plog) - (start-plog) + 1;
			char *cmd = (char*)malloc(len);
			strncpy(cmd, start, len-1);
			cmd[len-1] = '\0';
			char *time = get_time(plog);
			int time_v = time_value(time);

			//生成结构体，加载到列表中
			logInfo *p_logInfo = (logInfo*)malloc(sizeof(logInfo));
			p_logInfo->type = 0x05;
			p_logInfo->line = line;
			p_logInfo->time_v = time_v;
			p_logInfo->time = time;
			p_logInfo->cmd = cmd;
			
			addList(p_logInfo);
		}
		free(plog);
	}
	return 0;
}




//type 0x06
//内容标记wpa_supplicant: dump_last_scan_result: bssid=18:64:72:e9:75:11 ssid=lenovo-internet freq=5240 level=-68 noise=0 qual=0 flags=0xb
//结尾标记dump_last_scan_result over
unsigned int __stdcall wps_scan_result_thread(void *pPM)
{
	char *pfile = (char *)pPM;
	//初始化链表
	scan_result_list *p_scan_result_list = (scan_result_list*)malloc(sizeof(scan_result_list));
	scan_result_list *p_scan_result_tail = p_scan_result_list;
	p_scan_result_tail->pNext = NULL;

	//按行循环处理
	char *plog;
	char *pTemp;
	int len = 0;
	int line = 0;
	while(1) {
		pTemp = strchr(pfile, '\n');
		if(pTemp == NULL)
			break;
		line++;
		len = pTemp - pfile;
		plog = (char*)malloc(len + 1);
		strncpy(plog, pfile, len);
		plog[len] = '\0';
		pfile = pTemp + 1;
		//printf("线程编号id为%d, len:%d >>>>>>%s<<<<<<<<<<<\n", GetCurrentThreadId(), len, plog);
		if(is_belong(plog, "dump_last_scan_result:")) 
		{
			//获取命令内容，最后单引号内字符串
			//从最后一个冒号开始
			char *start = strstr(plog, "dump_last_scan_result:");
			if (start == NULL){
				free(plog);
				continue;
			}
				
			start += strlen("dump_last_scan_result: ");
			char *end = strstr(start, "noise=");
			if (end == NULL){
				free(plog);
				continue;
			}

			int len = end -start;
			char *cmd = (char*)malloc(len);
			strncpy(cmd, start, len-1);
			cmd[len-1] = '\0';
			//添加到扫描结果链表
			p_scan_result_tail->result = cmd;
			scan_result_list *ptemp = (scan_result_list*)malloc(sizeof(scan_result_list));
			ptemp->pNext = NULL;
			p_scan_result_tail->pNext = ptemp;
			p_scan_result_tail = ptemp;
		} else if (is_belong(plog, "dump_last_scan_result over")) {
			//该链表需要结束，然后加载到总链表中，同时生成新的结果链表
			char *time = get_time(plog);
			int time_v = time_value(time);
			//生成结构体，加载到列表中
			logInfo *p_logInfo = (logInfo*)malloc(sizeof(logInfo));
			p_logInfo->type = 0x06;
			p_logInfo->line = line;
			p_logInfo->time_v = time_v;
			p_logInfo->time = time;
			//输出结果的时候需要做类型转换
			p_logInfo->cmd = (char*)p_scan_result_list;
			addList(p_logInfo);

			//新的扫描结果链表
			p_scan_result_list = (scan_result_list*)malloc(sizeof(scan_result_list));
			p_scan_result_tail = p_scan_result_list;
			p_scan_result_tail->pNext = NULL;
		}
	}
	free(plog);
	return 0;
}



//type 0x07
//标记wpa_supplicant: wlan0: Request association with 18:64:72:ea:0f:d1
unsigned int __stdcall wps_selectbss_thread(void *pPM)
{
	char *pfile = (char *)pPM;

	//按行循环处理
	char *plog;
	char *pTemp;
	int len = 0;
	int line = 0;
	while (1) {
		pTemp = strchr(pfile, '\n');
		if (pTemp == NULL)
			break;
		line++;
		len = pTemp - pfile;
		plog = (char*)malloc(len + 1);
		strncpy(plog, pfile, len);
		plog[len] = '\0';
		pfile = pTemp + 1;
		//printf("线程编号id为%d, len:%d >>>>>>%s<<<<<<<<<<<\n", GetCurrentThreadId(), len, plog);
		if (is_belong(plog, "wpa_supplicant: wlan0: Request association with"))
		{
			//从最后一个空格开始
			char *start = strrchr(plog, ' ');
			if (start == NULL){
				free(plog);
				continue;
			}
			start += 1;
			int len = strlen(plog) - (start - plog) + 1;
			char *cmd = (char*)malloc(len);
			strncpy(cmd, start, len - 1);
			cmd[len - 1] = '\0';
			char *time = get_time(plog);
			int time_v = time_value(time);

			//生成结构体，加载到列表中
			logInfo *p_logInfo = (logInfo*)malloc(sizeof(logInfo));
			p_logInfo->type = 0x07;
			p_logInfo->line = line;
			p_logInfo->time_v = time_v;
			p_logInfo->time = time;
			p_logInfo->cmd = cmd;

			addList(p_logInfo);
		}
		free(plog);
	}
	return 0;
}



//type 0x08
//标记dhcpcd  : wlan0: acknowledged 10.117.209.99 from 10.100.23.225
unsigned int __stdcall dhcpcd_thread(void *pPM)
{
	char *pfile = (char *)pPM;

	//按行循环处理
	char *plog;
	char *pTemp;
	int len = 0;
	int line = 0;
	while (1) {
		pTemp = strchr(pfile, '\n');
		if (pTemp == NULL)
			break;
		line++;
		len = pTemp - pfile;
		plog = (char*)malloc(len + 1);
		strncpy(plog, pfile, len);
		plog[len] = '\0';
		pfile = pTemp + 1;
		//printf("线程编号id为%d, len:%d >>>>>>%s<<<<<<<<<<<\n", GetCurrentThreadId(), len, plog);
		if (is_belong(plog, "dhcpcd  : wlan0: acknowledged"))
		{
			//从最后一个空格开始
			char *start = strrchr(plog, 'd');
			if (start == NULL){
				free(plog);
				continue;
			}
			start += 2;
			int len = strlen(plog) - (start - plog) + 1;
			char *cmd = (char*)malloc(len);
			strncpy(cmd, start, len - 1);
			cmd[len - 1] = '\0';
			char *time = get_time(plog);
			int time_v = time_value(time);

			//生成结构体，加载到列表中
			logInfo *p_logInfo = (logInfo*)malloc(sizeof(logInfo));
			p_logInfo->type = 0x08;
			p_logInfo->line = line;
			p_logInfo->time_v = time_v;
			p_logInfo->time = time;
			p_logInfo->cmd = cmd;

			addList(p_logInfo);
		}
		free(plog);
	}
	return 0;
}


//type 0x09
//标记WifiService: setWifiEnabled:
unsigned int __stdcall wifi_openclose_thread(void *pPM)
{
	char *pfile = (char *)pPM;

	//按行循环处理
	char *plog;
	char *pTemp;
	int len = 0;
	int line = 0;
	while (1) {
		pTemp = strchr(pfile, '\n');
		if (pTemp == NULL)
			break;
		line++;
		len = pTemp - pfile;
		plog = (char*)malloc(len + 1);
		strncpy(plog, pfile, len);
		plog[len] = '\0';
		pfile = pTemp + 1;
		//printf("线程编号id为%d, len:%d >>>>>>%s<<<<<<<<<<<\n", GetCurrentThreadId(), len, plog);
		if (is_belong(plog, "WifiService: setWifiEnabled:"))
		{
			//从最后一个空格开始
			char *start = strrchr(plog, ':');
			if (start == NULL){
				free(plog);
				continue;
			}
			start += 2;
			int len = strlen(plog) - (start - plog) + 1;
			char *cmd = (char*)malloc(len);
			strncpy(cmd, start, len - 1);
			cmd[len - 1] = '\0';
			char *time = get_time(plog);
			int time_v = time_value(time);

			//生成结构体，加载到列表中
			logInfo *p_logInfo = (logInfo*)malloc(sizeof(logInfo));
			p_logInfo->type = 0x09;
			p_logInfo->line = line;
			p_logInfo->time_v = time_v;
			p_logInfo->time = time;
			p_logInfo->cmd = cmd;

			addList(p_logInfo);
		}
		free(plog);
	}
	return 0;
}