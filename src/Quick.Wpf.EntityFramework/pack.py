import sys,argparse,os,shutil,zipfile,subprocess
from xml.dom.minidom import parse,parseString
'''
opts,args = getopt.getopt(sys.argv[1:], "h:o:i:")
print(opts)
print(args)
for opt,val in opts:
    print(opt)
    print(val)
'''

'''
ext = os.path.splitext('/root/a')[1];
    print("111");
else:
    print("222")
exit(0)
'''


def try_remove(filePath):
    if os.path.exists(filePath):
        if os.path.isfile(filePath):
            os.remove(filePath)
        else:
            shutil.rmtree(filePath)


def zip_dir(dirName, zipFileName):
  fileList = []

  if os.path.isfile(dirName):
    fileList.append(dirName)
  else :
    for root, dirs, files in os.walk(dirName):
      for name in files:
        fileList.append(os.path.join(root, name))

  zf = zipfile.ZipFile(zipFileName, "w", zipfile.ZIP_DEFLATED)
  for tar in fileList:
    url = tar[len(dirName):]
    zf.write(tar, url)
  zf.close()

def zip_dir_with_rar(dirName, zipFileName):
    curDir = os.getcwd()
    os.chdir(dirName)
    cmd = f'"C:\\Program Files\\WinRAR\\WinRAR.exe" a "{zipFileName}" * -r'
    ps = subprocess.Popen(cmd)
    ps.wait()
    os.chdir(curDir)

def unzip_file(zipFileName, unzipToDir):
    ext = os.path.splitext(zipFileName)[1];

    file_zip = zipfile.ZipFile(zipFileName, 'r')
    for file in file_zip.namelist():
        file_zip.extract(file, unzipToDir)
    file_zip.close()


parser = argparse.ArgumentParser()
parser.add_argument('-Configuration','--Configuration', default='Release')
parser.add_argument('-Version','--Version', default='1.0.6-beta.6')
parser.add_argument('-CoreVersion','--CoreVersion', default='1.0.6-beta.6')
args = parser.parse_args()

Version = args.Version
CoreVersion = args.CoreVersion
Configuration = args.Configuration

ProjectName = 'Quick.Wpf.EntityFramework'
PackageId = 'QuickFramework.Wpf.EntityFramework'
ProjectFileName = ProjectName + '.csproj'
NupkgFileName = ProjectName + '.' + Version + '.nupkg'
NupkgName = ProjectName + '.' + Version

CurDir = sys.path[0]
OutDir = os.path.join(CurDir, 'bin')
ProjectFilePath = os.path.join(CurDir, ProjectFileName)
print(f'The current dir is: {CurDir}')
print(f'The output dir is: {OutDir}')
print(f'The project file path is: {ProjectFilePath}')

res = os.system(f'nuget pack "{ProjectFilePath}" -OutputDirectory "{OutDir}" -Version "{Version}" -Build -Properties Configuration="{Configuration}" -Force')
if(res != 0 ):
    exit(0)

NupkgPath = os.path.join(OutDir, NupkgFileName)
NupkgZipPath = os.path.join(OutDir, NupkgName + ".zip")
NupkgDir = os.path.join(OutDir, NupkgName)

if( os.path.exists(NupkgPath)):
    try_remove(NupkgZipPath)
    try_remove(NupkgDir)
    unzip_file(NupkgPath, NupkgDir)
    os.remove(NupkgPath)
else:
    print(f'{NupkgPath}不存在!')
    exit(0)

print('准备修改.nuspec文件')

DepXml = f"""
<group targetFramework=".NETFramework4.6.1">
        <dependency id="QuickFramework.Wpf" version="[{Version}]" exclude="Build,Analyzers" />
        <dependency id="Microsoft.EntityFrameworkCore" version="[3.1.18,3.1.20]" exclude="Build,Analyzers" />
        <dependency id="Microsoft.EntityFrameworkCore.Relational" version="[3.1.18,3.1.20]" exclude="Build,Analyzers" />
      </group>
"""

XmlFilePath = os.path.join(NupkgDir, ProjectName + '.nuspec')
print(XmlFilePath)
Tree = parse(XmlFilePath)
Doc = Tree.documentElement
IdElement = Doc.getElementsByTagName("id")[0]
IdElement.firstChild.data = PackageId

DepNodes = Doc.getElementsByTagName("dependencies")
DepNodes[0].childNodes.clear()
NewDoc = parseString(DepXml)
DepNodes[0].appendChild(NewDoc.documentElement)

OutXmlFile = open(XmlFilePath,'w', encoding='utf-8')
#将内存中的xml写入到文件
Tree.writexml(OutXmlFile,indent='',addindent='',newl='',encoding='utf-8')
OutXmlFile.close()

print('准备重新打包')
zip_dir_with_rar(NupkgDir, NupkgZipPath)
os.rename(NupkgZipPath, NupkgPath)
print('Done!')
input("Press any key to exit...")
