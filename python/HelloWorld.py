import win32clipboard as wc
import win32api
import time
import sys
import os
import threading
import string

#获取粘贴板里的内容
def getCopyTxet():
    wc.OpenClipboard()
    copytxet = wc.GetClipboardData()
    wc.CloseClipboard()
    return str(copytxet)


def search(root,state):
    items = os.listdir(root)
    for item in items:
        path = os.path.join(root, item)
        #print("join",path)
        if state == "1" or state =="2" :
            if os.path.isdir(path) :
                if path.find("cocosstudio") > 0 or path.find("SearchTool") > 0 :
                    continue
                elif path.find(".ccs") < 0 :     
                    search(path,state)
                    getTargetDir(path,state)
                    #print(path)
        elif state == "3" :
            if os.path.isdir(path) :
                if path.find("cocosstudio") > 0:
                    cocosstudiolis.append(path)
                    getTargetDir(cocosstudiolis[0],state)
                    break
                else :
                    search(path,state)
                break


#根据指定目录查询目录下所有的文件
def getTargetDir(path,state):
    threefile=[]
    cocosstudiolis.clear()
    for file in os.listdir(path):
        #tmppath = os.path.join(path)
        if os.path.splitext(file)[1] == '.ccs':
            sourcefile = os.path.join(path,file)
            threefile.append(sourcefile)
            # print("搜索全部CCS文件+",threefile)
            if state == "1" :
                for temppath in threefile :
                    splitstr=os.path.splitext(temppath)[0].split('\\')
                    if splitstr[-1].strip() == data.strip():
                        # print(splitstr[-1]+"-----"+getCopyTxet())
                        mes = {"name":data,"path":temppath}
                        print(mes)
            elif state == "2" :
                for dir in threefile : 
                    with open(dir,encoding="utf-8") as f:
                        #print("CCS文件内容+",f.read())
                        if f.read().find(data+".csd") > 0 :
                            mes = {"name":data,"path":dir}
                            print(mes)
        # elif os.path.splitext(file)[1] == '.csd' and state == "3":
            # sourcefile = os.path.join(path,file)
            # threefile.append(sourcefile)
            # for dir in threefile : 
                # with open(dir,encoding="utf-8") as f:
                    # print(f)
                    # # print("CSD文件内容+",f.read())
                    # if f.read().find("Name=\""+data+"\"") > 0 :
                        # mes = {"name":data,"path":dir}
                        # print(dir)

if __name__ == '__main__':
    cocosstudiolis = []
    sys.setrecursionlimit(1000000)
    #存储剪切板内容
    data = getCopyTxet()
    #print("粘贴板有新内容：" + data) #则打印粘贴板信息
    threading.stack_size(200000000)
    thread = threading.Thread(target=search,args=(sys.argv[1],sys.argv[2]))#sys.argv[1] "D:\\G66UI\\ui\\project","2"
    thread.start()