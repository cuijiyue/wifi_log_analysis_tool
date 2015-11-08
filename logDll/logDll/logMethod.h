/*
**底层共有6种类型
**0x01----monitor_command，上层下发到wps的命令
**0x02----monitor_reback，wps返回上层的事件
**0x03----driver_command，wps发送到驱动的命令
**0x04----driver_reback，wps从驱动收到的事件
**0x05----wps_sta，底层状态机
**0x06----收到扫描结果的信号强度，加密方式，SSID，BSSID信息，存储到链表里，以BSSID匹配
**0x07----与某个SSID连接上
××0x08----统计各部分的耗时，存放在一个特殊的链表里
*/




//存储信息的结构体
typedef struct 
{
	int type;
	int line;
	int time_v;
	char *time;
	char *cmd;
}logInfo; 

//log信息链表
typedef struct List
{
	logInfo *p_logInfo;
	struct List *pNext;
}logInfoList;


//type 0x06， 扫描结果链表，链表需要做转换添加到总链表中
typedef struct scan_results
{
	char *result;
	struct scan_results *pNext;
}scan_result_list;




char* get_time(char *log);
int time_value(char *time);
int is_belong(char *log, char *sign);

void showlog(logInfo * logArray, int len);
logInfoList* getList();
logInfo *logInfoSort(logInfoList* plist);
int getLogLines();
void addList(logInfo *p);
void init();
void close();

//处理线程
unsigned int __stdcall wifi_framwork_sta_thread(void *pPM);
unsigned int __stdcall screen_sta_thread(void *pPM);
unsigned int __stdcall wifisetting_sta_thread(void *pPM);
unsigned int __stdcall wifi_connected_sta_thread(void *pPM);

unsigned int __stdcall monitor_cmd_thread(void *pPM);
unsigned int __stdcall monitor_rebck_thread(void *pPM);
unsigned int __stdcall driver_cmd_thread(void *pPM);
unsigned int __stdcall driver_rebck_thread(void *pPM);
unsigned int __stdcall wps_sta_thread(void *pPM);
unsigned int __stdcall wps_scan_result_thread(void *pPM);
unsigned int __stdcall wps_selectbss_thread(void *pPM);
unsigned int __stdcall dhcpcd_thread(void *pPM);
unsigned int __stdcall wifi_openclose_thread(void *pPM);
