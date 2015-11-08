/*
**�ײ㹲��6������
**0x01----monitor_command���ϲ��·���wps������
**0x02----monitor_reback��wps�����ϲ���¼�
**0x03----driver_command��wps���͵�����������
**0x04----driver_reback��wps�������յ����¼�
**0x05----wps_sta���ײ�״̬��
**0x06----�յ�ɨ�������ź�ǿ�ȣ����ܷ�ʽ��SSID��BSSID��Ϣ���洢���������BSSIDƥ��
**0x07----��ĳ��SSID������
����0x08----ͳ�Ƹ����ֵĺ�ʱ�������һ�������������
*/




//�洢��Ϣ�Ľṹ��
typedef struct 
{
	int type;
	int line;
	int time_v;
	char *time;
	char *cmd;
}logInfo; 

//log��Ϣ����
typedef struct List
{
	logInfo *p_logInfo;
	struct List *pNext;
}logInfoList;


//type 0x06�� ɨ��������������Ҫ��ת����ӵ���������
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

//�����߳�
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
