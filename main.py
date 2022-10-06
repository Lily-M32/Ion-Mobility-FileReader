from PyQt5.QtWidgets import QDialog, QApplication, QPushButton, QMainWindow,QFileDialog, QCheckBox, QMessageBox, QListView, QAbstractItemView, QTreeView, QLabel, QLineEdit, QGridLayout
import subprocess
import pandas as pd
import numpy as np
import sys
import os
import matplotlib.pyplot as plt

from PIL import Image,ImageDraw
import xml.etree.ElementTree as ET

class Window(QDialog,QMainWindow):
    def __init__(self, parent=None):
        self.polygonpath = ''
        self.xscale123 = 0
        self.pwd = os.getcwd()
        self.pwd = '\\\\'.join(self.pwd.split('\\'))
        self.finalrawpath = ''
        self.finalrawpcsv = ''
        self.dpath = []
        self.exportlocation = ''
        self.MIDACdpath = ''
        self.filepathnameonly = ''
        self.finalrawpaths1 = ''
        self.abunclass = ''
        self.arrclass = ''
        self.line2 = ''
        self.line3 = ''
        super(Window, self).__init__(parent) #initizalizing our Gui
        self.setWindowTitle("Ion-Mobility File Reader")
        self.button1 = QPushButton('Load .m File')
        self.button4 = QPushButton('Load .d File(s) to be Batched')
        self.button2 = QPushButton('Choose Export Location of Files')
        self.button3 = QPushButton('Start Batch Processing!')
        self.check1 = QCheckBox("Save Raw CSV")
        self.check2 = QCheckBox("Don't use Polygon Selection")
        self.check3 = QCheckBox("Choose Specific Time Range")
        self.check4 = QCheckBox("Display Graph For Every File Processed")
        self.line = QLineEdit("Enter Starting Time")
        self.line1 = QLineEdit("Enter Ending Time")
        self.label = QLabel("                                                                                                                                                            ")
        self.setFixedSize(775,600)
        self.line.setFixedSize(125,25)
        self.line1.setFixedSize(125,25)
        self.line.move(0,0)
        self.button1.setFixedSize(750,100)
        self.button2.setFixedSize(750,100)
        self.button3.setFixedSize(750,100)
        self.button4.setFixedSize(750, 100)
        self.button1.clicked.connect(self.polyload)
        self.button4.clicked.connect(self.fileload)
        self.button2.clicked.connect(self.outputpath)
        self.button3.clicked.connect(self.main)
        layout = QGridLayout()
        layout.addWidget(self.button1)
        layout.addWidget(self.check2)
        layout.addWidget(self.button4)
        layout.addWidget(self.check1)
        layout.addWidget(self.button2)
        layout.addWidget(self.button3)
        layout.addWidget(self.check4,3,3)
        layout.addWidget(self.check3,6,0)
        layout.addWidget(self.line,6,1)
        layout.addWidget(self.line1,6,2)
        layout.addWidget(self.label,6,3)
        self.setLayout(layout)

    def main(self):
        for i in range(0,len(self.dpath)-1):
            self.i123 = i
            Window.MIDAC(self)
            try:
                path = self.pwd + "\\binframenums.csv"
                df = pd.read_csv(path, header=None, delimiter=',', dtype='int')
                self.line2 = df[0][0]
                self.line3 = df[1][0]
            except:
                pass
            if self.check3.isChecked() == True and self.check2.isChecked() == True:
                pass
            else:
                Window.fileconcat(self)
            if self.check3.isChecked() == True:
                if self.check2.isChecked() == True:
                    Window.arrprocessframe(self)
                if self.check2.isChecked() == False:
                    Window.polygonselframe(self)
            if self.check3.isChecked() == False:
                if self.check2.isChecked() == True:
                    Window.arrprocessnopoly(self)
                if self.check2.isChecked() == False:
                    Window.arrprocess(self)
            pathlistyappend = self.pwd + "\\binbinnum.csv"
            try:
                os.remove(path)
                os.remove(pathlistyappend)
            except:
                pass
            if self.check4.isChecked() == True:
                Window.graph(self)
        try:
            os.remove(self.pwd+"\\binbinnum.csv")
        except:
            pass
        msg = QMessageBox()
        msg.setText("Complete!")
        msg.setInformativeText('Please Re-Open Program Before Batching More Files')
        msg.setWindowTitle("Batch Complete!")
        msg.exec_()
    def graph(self):
        filename = self.exportlocation + self.filepathnameonly[self.i123] + ".csv"
        arr = np.loadtxt(filename, dtype=float, delimiter=',')
        y = arr[:, 0]
        x = arr[:, 1]
        plt.plot(y, x)
        plt.show()
    def polyload(self): #Gets x and y cords from .m files for the polygon
        path = QFileDialog.getExistingDirectory(self,"Select .m File")
        path = '\\\\'.join(path.split('/'))
        path = path + "\\\\DaMethod\\\\ImsBrowser\\\\browser.xml"
        try: #make sure we are loading a .m file
            tree = ET.parse(path)
        except:
            msg = QMessageBox()
            msg.setIcon(QMessageBox.Critical)
            msg.setText("Error")
            msg.setInformativeText('Wrong File or File Type Loaded!')
            msg.setWindowTitle("Error")
            msg.exec_()
        self.polygonpath = path

    def outputpath(self): #where to put the final files
        path = QFileDialog.getExistingDirectory(self, "Select Output Location")
        path = '\\\\'.join(path.split('/'))
        path = path + "\\\\"
        self.exportlocation = path

    def fileload(self): #gets location of .d files to load
        try:
            file_dialog = QFileDialog() #Use some custom pyqt file loader b/c pyqt doesnt let you load more than one folder natively (lol)
            file_dialog.setFileMode(QFileDialog.DirectoryOnly)
            file_dialog.setOption(QFileDialog.DontUseNativeDialog, True)
            file_view = file_dialog.findChild(QListView, 'listView')

            # to make it possible to select multiple directories:
            if file_view:
                file_view.setSelectionMode(QAbstractItemView.MultiSelection)
            f_tree_view = file_dialog.findChild(QTreeView)
            if f_tree_view:
                f_tree_view.setSelectionMode(QAbstractItemView.MultiSelection)

            if file_dialog.exec():
                paths = file_dialog.selectedFiles()
            self.dpath = paths
            paths = self.dpath
            pathslst = []
            for i in range(0,len(paths)):
                temp = Window.find_between_r(self,paths[i],"/",".d")
                pathslst.append(temp)
            pathslst.pop(0)
            self.filepathnameonly = pathslst
            filepath = paths[0]
            abc = "\\"
            filepath = filepath.replace("/",abc)
            list2 = [filepath + "\\\\" + x + ".d" for x in pathslst]
            self.finalrawpath1 = list2
        except:
            msg = QMessageBox()
            msg.setIcon(QMessageBox.Critical)
            msg.setText("Error")
            msg.setInformativeText('No File Loaded!')
            msg.setWindowTitle("Error")
            msg.exec_()

    def MIDAC(self): #runs midacaexe
        print("Starting Processing of .d file "+ str(self.i123 + 1)+ "/" + str(len(self.dpath)-1))
        tempfileoutput = self.pwd + "\\\\temp\\\\"
        Midacexe = self.pwd+"\\\\bin\\\\MidacApp.exe"
        paths = self.finalrawpath1
        if self.check3.isChecked() == False:
            if self.polygonpath == "" and self.check2.isChecked() == False:
                msg = QMessageBox()
                msg.setIcon(QMessageBox.Critical)
                msg.setText("Error")
                msg.setInformativeText('No Polygon Loaded!')
                msg.setWindowTitle("Error")
                msg.exec_()
                msg.clickedButton(sys.exit())
            subprocess.run([Midacexe,tempfileoutput, paths[self.i123],"false"])
        else:
            if self.check2.isChecked() == True:
                subprocess.run([Midacexe, tempfileoutput, paths[self.i123],"true",self.line.text(),self.line1.text(),"false"])
            else:
                if self.polygonpath == "":
                    msg = QMessageBox()
                    msg.setIcon(QMessageBox.Critical)
                    msg.setText("Error")
                    msg.setInformativeText('No Polygon Loaded!')
                    msg.setWindowTitle("Error")
                    msg.exec_()
                    msg.clickedButton(sys.exit())
                subprocess.run([Midacexe, tempfileoutput, paths[self.i123], "true", self.line.text(), self.line1.text(), "true"])
        print(str(self.i123 + 1)+ "/" + str(len(self.dpath)-1) + " .d files processed!")

    def arrprocess(self): #arrprocess with polygon selection
        arr, abun = Window.filereader(self)
        arr = np.array(arr, dtype=[('O', float)]).astype(float)  # converts numpy array of object dtype to float dtype
        width = len(arr[0])
        self.arrclass = arr[0]
        self.abunclass = abun
        height = len(arr)
        filename = self.exportlocation + self.filepathnameonly[self.i123] + ".csv"
        num1 = abun[0]
        num1 = float(num1)
        lenabun = len(abun)
        num2 = abun[lenabun-2]
        num2 = float(num2)
        scale = num2
        scale = int(scale)
        self.xscale123 = lenabun/scale
        mask = Window.polygonsel(self,width, height)
        finalarr = np.ma.masked_array(arr,np.logical_not(mask)) #removes all values not in the polygon
        arrsum = np.sum(finalarr, 0)  # sums up all columns in array, this is our final mass array
        massarr = np.array(arrsum, dtype='float')  # convert our masked array into a float array
        with open(filename, 'w') as f:  # write it to a file! This will give us one list with masses and abundances!
            for i in range(0, len(massarr)):
                a = round(massarr[i], 1)
                a = str(a)
                b = float(abun[i])
                b = str(b)
                text = b + "," + a
                f.write(text + "\n")
        if self.check1.isChecked() == True:
            output = self.exportlocation + self.filepathnameonly[self.i123] + "_RAW" + ".csv"
            path1 = self.finalrawpcsv
            os.rename(path1, output)
        path = self.pwd + "\\\\temp1\\\\"
        for f in os.listdir(path):
            os.remove(os.path.join(path, f))
        print(str(self.i123 + 1)+ "/" + str(len(self.dpath)-1) + " files processed!")

    def polygonselframe(self): #polygon selection with frame selection
        path = self.pwd + "\\temp1\\"
        path1 = os.listdir(path)
        path = path1[0]
        self.finalrawpcsv = self.pwd + "\\temp1\\" + path
        arr, abun = Window.filereader(self)
        arr = np.array(arr, dtype=[('O', float)]).astype(float)  # converts numpy array of object dtype to float dtype
        width = len(arr[0])
        height = len(arr)
        filename = self.exportlocation + self.filepathnameonly[self.i123] + ".csv"
        num1 = abun[0]
        num1 = float(num1)
        lenabun = len(abun)
        num2 = abun[lenabun - 2]
        num2 = float(num2)
        scale = num2
        scale = int(scale)
        self.xscale123 = lenabun/scale
        self.abunclass = abun
        mask = Window.polygonsel(self, width, height)
        finalarr = np.ma.masked_array(arr, np.logical_not(mask))  # removes all values not in the polygon
        arrsum = np.sum(finalarr, 0)  # sums up all columns in array, this is our final mass array
        massarr = np.array(arrsum, dtype='float')  # convert our masked array into a float array
        with open(filename, 'w') as f:  # write it to a file! This will give us one list with masses and abundances!
            for i in range(0, len(massarr)):
                a = round(massarr[i], 1)
                a = str(a)
                b = float(abun[i])
                b = str(b)
                text = b + "," + a
                f.write(text + "\n")
        if self.check1.isChecked() == True:
            output = self.exportlocation + self.filepathnameonly[self.i123] + "_RAW" + ".csv"
            path1 = self.finalrawpcsv
            os.rename(path1, output)
        path = self.pwd + "\\\\temp1\\\\"
        for f in os.listdir(path):
            os.remove(os.path.join(path, f))
        print(str(self.i123 + 1) + "/" + str(len(self.dpath) - 1) + " files processed!")

    def polygonsel(self,width,height): #selection our polygon from our array
            list1 = []
            path = self.polygonpath
            tree = ET.parse(path)  # location to retrive polygon cords from
            root = tree.getroot()  # index our xml file
            vert = root.findall(".//Vertices")  # find where the tag?? "vertices" is
            for v_tags in vert:  # In that tag, find all the attributes of vertices (our x and y cords)
                for v_tag in v_tags:
                    list1.append(v_tag.attrib)  # store the dict in list1
            listx = []
            listy = []
            for i in range(0, len(list1)):  # tag is saved as a dict, find the x and y keys of this dict
                listy.append(list1[i]["x"]) #x and y are flipped in xml file, so we swap them here back
                listx.append(list1[i]["y"])
            listy = [float(i)for i in listy] #convert to float
            listx = [float(i)for i in listx]
            pathlistyappend = self.pwd + "\\binbinnum.csv"
            binandframe = pd.read_csv(pathlistyappend, header=None, delimiter=',', dtype='int')
            binframe = binandframe[0][0]
            frame = binandframe[1][0]
            if self.check3.isChecked() == True:
                a1 = self.line2
                a2 = self.line3
                num = (int(a1) - int(a2)) + 1
                num1 = (frame*num)/60
                listy = [i * int(num1) for i in listy]
            if self.check3.isChecked() == False:
                listyappend = binframe / 60 #"bins" on image are always 60, so this should scale our y polygon correctly
                listy = [i * listyappend for i in listy]
            listx = [float(i) for i in listx]
            listxround = [round(i) for i in listx] # round the cords
            abunlist = self.abunclass
            abunlist = np.array(abunlist, dtype=[('O', float)]).astype(float)
            #abunlist = abunlist.astype(np.float)
            abunround = np.floor(abunlist)
            listx1 = []
            for i in range(0,len(listxround)):
                x = listxround[i]
                temp = np.where(abunround==x)[0][0]
                listx1.append(temp)
            listxfinal = listx1
            listyround = [round(float(i)) for i in listy]
            listxfinal.reverse() #flip our x and y lists as the coords come in flipped from the xml file
            listyround.reverse()
            listcord = [list(i) for i in zip(listxfinal, listyround)]  # combine our x and y lists into one list [[x1,y1][x2,y2]...]
            listcord = [tuple(l) for l in listcord]
            polygon = listcord
            width = width
            height = height
            img = Image.new('L', (width, height), 0)# create a new canvas using PIL
            #img = Image.new("RGB", (width, height), "#f9f9f9")
            ImageDraw.Draw(img).polygon(polygon, outline=1, fill=1)  # draw our polygon ontop of that image
            img = img.transpose(method=Image.FLIP_TOP_BOTTOM) #flip our top and bottom, so image is created from bottom to top, mimicing how the polygon is drawn on MASSHUNTER
            mask = np.array(img)  # turn that image into a mask
            return mask

    def fileconcat(self):
        #concactinating all files made into one big file, may multiprocess later.
        #Done in pure python, much faster than any other solution
        if self.check3.isChecked()==False:
            outputpath = self.pwd + "\\temp1\\"
            paths = self.dpath
            path = self.pwd + "\\temp\\" +self.filepathnameonly[self.i123] + "_{qqqq}.csv"
            #path = 'C:\\Users\\Lily\\Desktop\\Chop_file\\Chop_Test\\08_dajs_50ngHSA001_{qqqq}.csv'
            x = Window.find_between_r(self,path,"\\","{qqqq}")
            self.finalrawpath = outputpath+x
            self.finalrawpcsv = self.finalrawpath + ".csv"
            with open(self.finalrawpcsv, "wb") as fout:
                for num in range(1, 9):
                    with open(path.format(qqqq=int(num)),"rb") as f:
                        fout.write(f.read())
            path1 = self.pwd + "\\\\temp\\\\"
            for f in os.listdir(path1):
                os.remove(os.path.join(path1, f))
        else:
            outputpath = self.pwd + "\\temp1\\"
            paths = self.dpath
            path = self.pwd + "\\temp\\" +self.filepathnameonly[self.i123] + "_{qqqq}.csv"
            #path = 'C:\\Users\\Lily\\Desktop\\Chop_file\\Chop_Test\\08_dajs_50ngHSA001_{qqqq}.csv'
            x = Window.find_between_r(self,path,"\\","{qqqq}")
            self.finalrawpath = outputpath+x
            self.finalrawpcsv = self.finalrawpath + ".csv"
            with open(self.finalrawpcsv, "wb") as fout:
                for num in range(int(self.line2), int(self.line3)+1):
                    with open(path.format(qqqq=int(num)),"rb") as f:
                        fout.write(f.read())
            path1 = self.pwd + "\\\\temp\\\\"
            for f in os.listdir(path1):
                os.remove(os.path.join(path1, f))

    def find_between_r(self, s, first, last ): #used to find filenames
        try:
            start = s.rindex( first ) + len( first )
            end = s.rindex( last, start )
            return s[start:end]
        except ValueError:
            return ""

    def arrprocessnopoly(self): #array process without a polygon
        arr, abun = Window.filereader(self)
        arr = np.array(arr, dtype=[('O', float)]).astype(float)  # converts numpy array of object dtype to float dtype
        filename = self.exportlocation + self.filepathnameonly[self.i123] + ".csv"
        num1 = abun[0]
        num1 = float(num1)
        lenabun = len(abun)
        num2 = abun[lenabun - 2]
        num2 = float(num2)
        scale = num2
        finalarr = arr# removes all values not in the polygon
        arrsum = np.sum(finalarr, 0)  # sums up all columns in array, this is our final mass array
        massarr = np.array(arrsum, dtype='float')  # convert our masked array into a float array
        with open(filename, 'w') as f:  # write it to a file! This will give us one list with masses and abundances!
            for i in range(0, len(massarr)):
                a = round(massarr[i], 1)
                a = str(a)
                b = float(abun[i])
                b = str(b)
                text = b + "," + a
                f.write(text + "\n")
        if self.check1.isChecked() == True:
            output = self.exportlocation + self.filepathnameonly[self.i123] + "_RAW" + ".csv"
            path1 = self.finalrawpcsv
            os.rename(path1, output)
        path = self.pwd + "\\\\temp1\\\\"
        for f in os.listdir(path):
            os.remove(os.path.join(path, f))
        print(str(self.i123 + 1)+ "/" + str(len(self.dpath)-1) + " files processed!")

    def arrprocessframe(self): #arrprocess if certain frames are selected. Almost the same code as no polygon arrprocess
        path = self.pwd+"\\temp\\"
        path1 = os.listdir(path)
        path = path1[0]
        self.finalrawpcsv = self.pwd + "\\temp\\" + path
        arr, abun = Window.filereader(self)
        arr = np.array(arr, dtype=[('O', float)]).astype(float)  # converts numpy array of object dtype to float dtype
        filename = self.exportlocation + self.filepathnameonly[self.i123] + ".csv"
        num1 = abun[0]
        num1 = float(num1)
        lenabun = len(abun)
        num2 = abun[lenabun - 2]
        num2 = float(num2)
        scale = num2
        scale = int(scale)
        finalarr = arr  # removes all values not in the polygon
        arrsum = np.sum(finalarr, 0)  # sums up all columns in array, this is our final mass array
        massarr = np.array(arrsum, dtype='float')  # convert our masked array into a float array
        with open(filename, 'w') as f:  # write it to a file! This will give us one list with masses and abundances!
            for i in range(0, len(massarr)):
                a = round(massarr[i], 1)
                a = str(a)
                b = float(abun[i])
                b = str(b)
                text = b + "," + a
                f.write(text + "\n")
        if self.check1.isChecked() == True:
            output = self.exportlocation + self.filepathnameonly[self.i123] + "_RAW" + ".csv"
            path1 = self.finalrawpcsv
            os.rename(path1, output)
        path = self.pwd + "\\\\temp\\\\"
        for f in os.listdir(path):
            os.remove(os.path.join(path, f))
        print(str(self.i123 + 1)+ "/" + str(len(self.dpath)-1) + " files processed!")

    def filereader(self): # This function takes our files produced by MIDAC and proccesses them into a numpy array
        path = self.finalrawpcsv
        df = pd.read_csv(path,header=None,delimiter=',',dtype='string') #load file into a pandas df
        array = df.to_numpy() #convert the df into a numpy array
        firstAt = df[0] >= '@' #find where there are @s in the 0th column (by creating a bool mask)
        atnum = firstAt.value_counts().loc[True] #counts how many @s there are in a given file in column 0
        rowL = next(iter(firstAt.index[firstAt]))#Count how many numbers there are between the @
        list = []
        var = atnum+2
        for i in range(1,var): #creates a list of int to use to index the df
            if i == 1:
                list.append(0)
                list.append(rowL)
            else:
                dce = ((rowL+1) * (i-1))
                edf = dce + (rowL)
                list.append(dce)
                list.append(edf)
        arraylist = []
        for i in range(0,len(list)-1): #converts numpy array into list of numpy arrays at every bin
            if i % 2 == 0:
                z = array[list[i]:list[i+1]]
                arraylist.append(z)
        A = np.array(arraylist,dtype='object')#convert arraylist into an array (same speed as converting directly into an array)
        arraylen = len(A)-1
        A = A[:arraylen] #removes final point of array to give array even size and shape
        arlist = []
        abunlist = A[0][:,0] #retrives abundance list
        for i in range(0,len(A)): #makes list of arrays, each array being one "bin"
                    x = A[i][:,1]
                    arlist.append(x)
        finalarr = np.array(arlist,dtype='float') #change dtype to float
        print(str(self.i123 + 1)+ "/" + str(len(self.dpath)-1) + " files loaded!")
        return finalarr,abunlist #return abundance and mass arrays
if __name__ == '__main__':
    try:
        app = QApplication(sys.argv)
    except:
        pass
    window = Window()
    window.show()
    sys.exit(app.exec_())
