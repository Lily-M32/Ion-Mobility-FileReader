
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Reflection;

// For file open dialog
using Agilent.MassSpectrometry.CommonControls.AgtFolderSelectionDialog;
//using Agilent.MassSpectrometry.DataAnalysis.FileDialogControls;

// For MassHunter IMS Data Access
using Agilent.MassSpectrometry.MIDAC;

namespace MidacApp
{
    public class MidacApp
    {
        IMidacImsReader m_reader;
        

        //string filepath = "C:\\Users\\Lily\\Desktop\\CHOP\\08_dajs_50ngHSA001.d";
        static void Main(string[] args)
        {
            string[] filepaths = args;
            //string filepath = @"C:\\Users\\Lily\\Desktop\\CHOP\\08_dajs_50ngHSA001.d";
            string outputpath = @args[0];
            int lenargs = args.Length;
            //string outputpath = @"C:\\Users\\Lily\\Desktop\\Chop_file\\Chop_Multi\\";
            MidacApp test = new MidacApp(filepaths, outputpath, lenargs);

        }
        public MidacApp(string[] filepaths, string outputpath, int lenargs)
        {
            /*
            string[] args1 = filepaths;
            string filepath1 = filepaths[1];
            test(filepath1);
            System.Environment.Exit(1);
            if (filepaths[2] == "true")
            */
            m_reader = MidacFileAccess.ImsDataReader(filepaths[1]);
            int numbins = m_reader.FileInfo.MaxNonTfsMsPerFrame;
            int framenum1 = m_reader.FileInfo.NumFrames;
            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path1 = path + "binnum.csv";
            string stringwrite = Convert.ToString(numbins) + "," + Convert.ToString(framenum1);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path1))
            {
                file.WriteLine(stringwrite);
            }
                if (filepaths[2] == "true")
            {
                string[] args = filepaths;
                m_reader = null;
                string filepath = filepaths[1];
                try
                {
                    m_reader = MidacFileAccess.ImsDataReader(filepath);
                }
                catch
                {
                    Console.WriteLine("Wrong file type or file loaded!");
                    MessageBox.Show("Wrong file type or file loaded!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    System.Environment.Exit(15);
                }
                int numDriftBins = m_reader.FileInfo.MaxNonTfsMsPerFrame;
                int maxDBin = numDriftBins - 1;
                var framenum = m_reader.FileInfo.NumFrames;
                m_reader.Close();
                var matches = Regex.Matches(filepath, @"(?<=\\\\)(.+?)(?=\.d)"); //finds the string between \\\\ and .d (file name) the \\\\ will be fixed in python.
                string output = matches[0] + "";
                m_reader = MidacFileAccess.ImsDataReader(filepath);
                if (filepaths[5] == "true")
                {
                    Task task1 = Task.Factory.StartNew(() => FramePolySelect(filepath, framenum, numDriftBins, outputpath, output, args));
                    Task.WaitAll(task1);
                }
                else
                {
                    Task task1 = Task.Factory.StartNew(() => FrameSelect(filepath, framenum, numDriftBins, outputpath, output, args));
                    Task.WaitAll(task1);
                }
            }
            else
            {
                    m_reader = null;
                    string filepath = filepaths[1];
                    try
                    {
                        m_reader = MidacFileAccess.ImsDataReader(filepath);
                    }
                    catch
                    {
                        Console.WriteLine("Wrong file type or file loaded!");
                        MessageBox.Show("Wrong file type or file loaded!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        System.Environment.Exit(15);
                    }
                    int numDriftBins = m_reader.FileInfo.MaxNonTfsMsPerFrame;
                    //int numDriftBins = 40;
                    //int numDriftBins = 5;
                    //int minDBin = 0;
                    int maxDBin = numDriftBins - 1;
                    var framenum = m_reader.FileInfo.NumFrames;
                    //var framenum = 16;
                    //int framenum1 = 1;
                    //int dbin1 = 1;
                    m_reader.Close();
                    //string firstString = "//";
                    //string lastString = ".d";
                    //int pos1 = filepath.IndexOf(firstString) + firstString.Length;
                    //int pos2 = filepath.Substring(pos1).IndexOf(lastString);
                    //string result = filepath.Substring(pos1, pos2);
                    //string dotd = ".d";
                    var matches = Regex.Matches(filepath, @"(?<=\\\\)(.+?)(?=\.d)"); //finds the string between \\\\ and .d (file name) the \\\\ will be fixed in python.
                    string output = matches[0] + "";
                    //test(filepath, maxDBin, framenum);
                    //filereada(i, filepath, framenum, numDriftBins, outputpath);
                    //for (int i = 0; i < numDriftBins; i++)
                    //    List<Thread> threads = new List<Thread>();
                    //for (int i = 1; i < 3; i++)
                    //{
                    //    Thread abc1 = new Thread(() => filereada(i, filepath, framenum, numDriftBins, outputpath));
                    //    abc1.Start();
                    //    threads.Add(abc1);
                    //    //Task.WaitAll(abc1);
                    //}
                    //foreach (Thread thread in threads)
                    //{
                    //    thread.Join();
                    //}
                    m_reader = MidacFileAccess.ImsDataReader(filepath);
                    Task task1 = Task.Factory.StartNew(() => filereada(filepath, framenum, numDriftBins, outputpath, output));

                    Task task2 = Task.Factory.StartNew(() => filereada1(filepath, framenum, numDriftBins, outputpath, output));

                    Task task3 = Task.Factory.StartNew(() => filereada2(filepath, framenum, numDriftBins, outputpath, output));
                    //Task.WaitAll(task1);
                    Task task4 = Task.Factory.StartNew(() => filereada3(filepath, framenum, numDriftBins, outputpath, output));
                    Task task5 = Task.Factory.StartNew(() => filereada4(filepath, framenum, numDriftBins, outputpath, output));
                    Task task6 = Task.Factory.StartNew(() => filereada5(filepath, framenum, numDriftBins, outputpath, output));
                    Task task7 = Task.Factory.StartNew(() => filereada6(filepath, framenum, numDriftBins, outputpath, output));
                    Task task8 = Task.Factory.StartNew(() => filereada7(filepath, framenum, numDriftBins, outputpath, output));
                    Task.WaitAll(task1, task2, task3, task4, task5, task6, task7, task8);
                    //filereada(i, filepath, framenum, numDriftBins, outputpath);
                
            }   

        }
        public void FramePolySelect(string filepath, int framenum, int numDriftBins, string outputpath, string m, string[] args) //frame selection with polygon selection
        {
            m_reader = MidacFileAccess.ImsDataReader(filepath);
            var convert = m_reader.FileUnitConverter;
            double convertunit = Convert.ToDouble(args[3]);
            double convertunit1 = Convert.ToDouble(args[4]);
            var TotalTimeRange = convert.AcqTimeRange;
            double max = TotalTimeRange.Max;
            if(convertunit1 > max)
            {
                Console.WriteLine("Time range is outside of aquired time range!");
                MessageBox.Show("Time range is outside of aquired time range!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(15);
            }
            convert.Convert(MidacUnits.Minutes, MidacUnits.FrameNumber, ref convertunit);
            convert.Convert(MidacUnits.Minutes, MidacUnits.FrameNumber, ref convertunit1);
            int roundconvert1 = Convert.ToInt16(Math.Round(convertunit));
            int roundconvert2 = Convert.ToInt16(Math.Round(convertunit1));
            string path3 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path4 = path3 + "framenums.csv";
            string write = Convert.ToString(roundconvert1) + ","+ Convert.ToString(roundconvert2);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path4))
            {
                file.WriteLine(write);
            }
            List<float[]> allsums = new List<float[]>();
            for (int i = roundconvert1; i < roundconvert2+1; i++)
            {
                List<float[]> abun = new List<float[]>();
                for (int l = 1; l < numDriftBins; l++)
                {
                    //IMidacSpecDataMs specData1 = m_reader.FrameMs(l, dbin, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
                    float[] b = Specdatareader(filepath, i, l).YArray;
                    abun.Add(b);
                }
                GC.Collect();
                double[] xarray = Specdatareader1(filepath).XArray;
                //float[] b = specData.YArray;
                int xarraylength = xarray.GetLength(0);
                //int xarrayl = xarraylength + 1;
                string docpath = (outputpath);
                string num = Convert.ToString(m);
                string path = docpath + num +"_" +Convert.ToString(i) + ".csv";
                using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
                {
                    for (int k = 0; k < abun.Count; k++)
                    {
                        for (int j = 0; j < xarraylength; j++)
                        {
                            if (k == 0)
                            {
                                string xstring = Convert.ToString(xarray[j]);
                                float[] ystring1 = abun[k];
                                string ystring = Convert.ToString(ystring1[j]);
                                string xy = xstring + "," + ystring;
                                file.WriteLine(xy);
                            }
                            else
                            {
                                string xstring = "0";
                                float[] ystring1 = abun[k];
                                string ystring = Convert.ToString(ystring1[j]);
                                string xy = xstring + "," + ystring;
                                file.WriteLine(xy);
                            }
                        }
                        file.WriteLine("@");
                    }
                    GC.Collect();
                }

            }
           
        }
        public void FrameSelect(string filepath, int framenum, int numDriftBins, string outputpath, string m, string[] args) //Frame Selection With No Polygon Selection
        {
            int Dbindiv = numDriftBins / 8;
            //Console.WriteLine(Dbindiv);
            int minDBin = 1;
            //int maxDBin = numDriftBins - 1;
            int maxDBin = Dbindiv * 1;
            //int maxDBin = 6;
            //abc = 1;
            //maxDBin = numDriftBins / 4;
            //maxDBin = 46;

            List<float[]> allsums = new List<float[]>();
            m_reader = MidacFileAccess.ImsDataReader(filepath);
            var convert = m_reader.FileUnitConverter;
            double convertunit = Convert.ToDouble(args[3]);
            double convertunit1 = Convert.ToDouble(args[4]);
            var TotalTimeRange = convert.AcqTimeRange;
            double max = TotalTimeRange.Max;
            if (convertunit1 > max)
            {
                Console.WriteLine("Time range is outside of aquired time range!");
                MessageBox.Show("Time range is outside of aquired time range!", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Environment.Exit(15);
            }
            convert.Convert(MidacUnits.Minutes, MidacUnits.FrameNumber, ref convertunit);
            convert.Convert(MidacUnits.Minutes, MidacUnits.FrameNumber, ref convertunit1);
            int roundconvert1 = Convert.ToInt16(Math.Round(convertunit));
            int roundconvert2 = Convert.ToInt16(Math.Round(convertunit1));
            string path3 = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            string path4 = path3 + "framenums.csv";
            string write = Convert.ToString(roundconvert1) + ","+Convert.ToString(roundconvert2);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path4))
            {
                file.WriteLine(write);
            }
            for (int i = roundconvert1; i < roundconvert2+1;i++)
            {
                List<float[]> abun = new List<float[]>();

                for (int l = 1; l < numDriftBins; l++)
                {
                    //IMidacSpecDataMs specData1 = m_reader.FrameMs(l, dbin, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
                    float[] b = Specdatareader(filepath, i, l).YArray;
                    abun.Add(b);
                }
                //Console.WriteLine(abun.Count);
                GC.Collect();
                //List<float[]> abun2 = new List<float[]>();
                float[] sums = new float[abun[0].Length];
                foreach (float[] array in abun)
                {
                    for (int p = 0; p < sums.Length; p++)
                    {
                        sums[p] += array[p];
                    }
                    //Console.WriteLine(sums.Length + "Sum length");
                }
                allsums.Add(sums);
                GC.Collect();
            }
            //IMidacSpecDataMs specData = m_reader.FrameMs(framenum1, dbin1, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
            int lenallsums = allsums.Count;
            //Console.WriteLine(lenallsums);
            //Console.WriteLine("Done!");
            List<int> intlist = new List<int>();
            for (int i = minDBin; i < maxDBin + 2; i++)
            {
                intlist.Add(i);
            }
            double[] xarray = Specdatareader1(filepath).XArray;
            //float[] b = specData.YArray;
            int xarraylength = xarray.GetLength(0);
            //int xarrayl = xarraylength + 1;
            string docpath = (outputpath);
            string num = Convert.ToString(m);
            string path = docpath + num + "_1" + ".csv";
            GC.Collect();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                for (int i = 0; i < lenallsums; i++)
                {
                    for (int j = 0; j < xarraylength; j++)
                    {
                        if (i == 0)
                        {
                            string xstring = Convert.ToString(xarray[j]);
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                        else
                        {
                            string xstring = "0";
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                    }
                    file.WriteLine("@");
                }
                GC.Collect();
            }
        }

        public void filereada(string filepath,int framenum,int numDriftBins,string outputpath,string m)
            {
            int Dbindiv = numDriftBins / 8;
            //Console.WriteLine(Dbindiv);
            int minDBin = 1;
            //int maxDBin = numDriftBins - 1;
            int maxDBin = Dbindiv * 1;
            //int maxDBin = 6;
            //abc = 1;
            //maxDBin = numDriftBins / 4;
            //maxDBin = 46;
           
            List<float[]> allsums = new List<float[]>();
            for (int dbin = minDBin; dbin <= maxDBin; dbin++)
            {
                List<float[]> abun = new List<float[]>();

                for (int l = 1; l < framenum; l++)
                {
                    //IMidacSpecDataMs specData1 = m_reader.FrameMs(l, dbin, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
                    float[] b = Specdatareader(filepath, l, dbin).YArray;
                    abun.Add(b);
                }
                //Console.WriteLine(abun.Count);
                GC.Collect();
                //List<float[]> abun2 = new List<float[]>();
                float[] sums = new float[abun[0].Length];
                foreach (float[] array in abun)
                {
                    for (int p = 0; p < sums.Length; p++)
                    {
                        sums[p] += array[p];
                    }
                    //Console.WriteLine(sums.Length + "Sum length");
                }
                allsums.Add(sums);
                GC.Collect();
            }
            //IMidacSpecDataMs specData = m_reader.FrameMs(framenum1, dbin1, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
            int lenallsums = allsums.Count;
            //Console.WriteLine(lenallsums);
            //Console.WriteLine("Done!");
            List<int> intlist = new List<int>();
            for (int i = minDBin; i < maxDBin+2; i++)
            {
                intlist.Add(i);
            }
            double[] xarray = Specdatareader1(filepath).XArray;
            //float[] b = specData.YArray;
            int xarraylength = xarray.GetLength(0);
            //int xarrayl = xarraylength + 1;
            string docpath = (outputpath);
            string num = Convert.ToString(m);
            string path = docpath + num + "_1" + ".csv";
            GC.Collect();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                for (int i = 0; i < lenallsums; i++)
                {
                    for (int j = 0; j < xarraylength; j++)
                    {
                        if (i == 0)
                        {
                            string xstring = Convert.ToString(xarray[j]);
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                        else
                        {
                            string xstring = "0";
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                    }
                    file.WriteLine("@");
                }
                GC.Collect();
            }
        }
        public void filereada1(string filepath, int framenum, int numDriftBins, string outputpath,string m)
        {
            int Dbindiv = numDriftBins / 8;
            //int minDBin = 1;
            ////int maxDBin = numDriftBins - 1;
            //int maxDBin = 1;
            //int minDBi;
            //int maxDbi;
            //minDBi = numDriftBins / 4;
            //minDBin = minDBi + 1;
            int minDBin = Dbindiv +1;
            //maxDbi = numDriftBins / 4;
            //maxDBin = maxDbi * 2;
            int maxDBin = Dbindiv *2;
            //m_reader = MidacFileAccess.ImsDataReader(filepath);

            List<float[]> allsums = new List<float[]>();
            for (int dbin = minDBin; dbin <= maxDBin; dbin++)
            {
                List<float[]> abun = new List<float[]>();

                for (int l = 1; l < framenum; l++)
                {
                    //IMidacSpecDataMs specData1 = m_reader.FrameMs(l, dbin, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
                    float[] b = Specdatareader(filepath, l, dbin).YArray;
                    abun.Add(b);
                }
                //Console.WriteLine(abun.Count);
                GC.Collect();
                //List<float[]> abun2 = new List<float[]>();
                float[] sums = new float[abun[0].Length];
                foreach (float[] array in abun)
                {
                    for (int p = 0; p < sums.Length; p++)
                    {
                        sums[p] += array[p];
                    }
                    //Console.WriteLine(sums.Length + "Sum length");
                }
                allsums.Add(sums);
                GC.Collect();
            }
            //IMidacSpecDataMs specData = m_reader.FrameMs(framenum1, dbin1, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
            int lenallsums = allsums.Count;
            //Console.WriteLine(lenallsums);
            //Console.WriteLine("Done!");
            List<int> intlist = new List<int>();
            for (int i = minDBin; i < maxDBin + 2; i++)
            {
                intlist.Add(i);
            }
            double[] xarray = Specdatareader1(filepath).XArray;
            //float[] b = specData.YArray;
            int xarraylength = xarray.GetLength(0);
            //int xarrayl = xarraylength + 1;
            string docpath = (outputpath);
            string num = Convert.ToString(m);
            string path = docpath +num + "_2" + ".csv";
            GC.Collect();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                for (int i = 0; i < lenallsums; i++)
                {
                    for (int j = 0; j < xarraylength; j++)
                    {
                        if (i == 0)
                        {
                            string xstring = Convert.ToString(xarray[j]);
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                        else
                        {
                            string xstring = "0";
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                    }
                    file.WriteLine("@");
                }
                GC.Collect();
            }

        }
        public void filereada2(string filepath, int framenum, int numDriftBins, string outputpath, string m)
        {
            int DbinDiv = numDriftBins / 8;
            //int minDBin = 1;
            ////int maxDBin = numDriftBins - 1;
            //int maxDBin = 1;
            //int minDBi;
            //int maxDbi;
            //abc = 3;
            //minDBi = numDriftBins / 4;
            //minDBin = minDBi * 2 + 1;
            int minDBin = (DbinDiv * 2) + 1;
            //maxDbi = numDriftBins / 4;
            //maxDBin = maxDbi * 3;
            int maxDBin = DbinDiv * 3;
            //m_reader = MidacFileAccess.ImsDataReader(filepath);


            List<float[]> allsums = new List<float[]>();
            for (int dbin = minDBin; dbin <= maxDBin; dbin++)
            {
                List<float[]> abun = new List<float[]>();

                for (int l = 1; l < framenum; l++)
                {
                    //IMidacSpecDataMs specData1 = m_reader.FrameMs(l, dbin, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
                    float[] b = Specdatareader(filepath, l, dbin).YArray;
                    abun.Add(b);
                }
                //Console.WriteLine(abun.Count);
                GC.Collect();
                //List<float[]> abun2 = new List<float[]>();
                float[] sums = new float[abun[0].Length];
                foreach (float[] array in abun)
                {
                    for (int p = 0; p < sums.Length; p++)
                    {
                        sums[p] += array[p];
                    }
                    //Console.WriteLine(sums.Length + "Sum length");
                }
                allsums.Add(sums);
                GC.Collect();
            }
            //IMidacSpecDataMs specData = m_reader.FrameMs(framenum1, dbin1, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
            int lenallsums = allsums.Count;
            //Console.WriteLine(lenallsums);
            //Console.WriteLine("Done!");
            List<int> intlist = new List<int>();
            for (int i = minDBin; i < maxDBin + 2; i++)
            {
                intlist.Add(i);
            }
            double[] xarray = Specdatareader1(filepath).XArray;
            //float[] b = specData.YArray;
            int xarraylength = xarray.GetLength(0);
            //int xarrayl = xarraylength + 1;
            string docpath = (outputpath);
            string num = Convert.ToString(m);
            string path = docpath +num + "_3" + ".csv";
            GC.Collect();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                for (int i = 0; i < lenallsums; i++)
                {
                    for (int j = 0; j < xarraylength; j++)
                    {
                        if (i == 0)
                        {
                            string xstring = Convert.ToString(xarray[j]);
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                        else
                        {
                            string xstring = "0";
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                    }
                    file.WriteLine("@");
                }
                GC.Collect();
            }
        }
        public void filereada3(string filepath, int framenum, int numDriftBins, string outputpath, string m)
        {
            int DbinDiv = numDriftBins / 8;
            //int minDBin = 1;
            ////int maxDBin = numDriftBins - 1;
            //int maxDBin = 1;
            //int minDBi;
            //int maxDbi;
            //abc = 3;
            //minDBi = numDriftBins / 4;
            //minDBin = minDBi * 2 + 1;
            int minDBin = (DbinDiv * 3) + 1;
            //maxDbi = numDriftBins / 4;
            //maxDBin = maxDbi * 3;
            int maxDBin = DbinDiv * 4;
            m_reader = MidacFileAccess.ImsDataReader(filepath);


            List<float[]> allsums = new List<float[]>();
            for (int dbin = minDBin; dbin <= maxDBin; dbin++)
            {
                List<float[]> abun = new List<float[]>();

                for (int l = 1; l < framenum; l++)
                {
                    //IMidacSpecDataMs specData1 = m_reader.FrameMs(l, dbin, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
                    float[] b = Specdatareader(filepath, l, dbin).YArray;
                    abun.Add(b);
                }
                //Console.WriteLine(abun.Count);
                GC.Collect();
                //List<float[]> abun2 = new List<float[]>();
                float[] sums = new float[abun[0].Length];
                foreach (float[] array in abun)
                {
                    for (int p = 0; p < sums.Length; p++)
                    {
                        sums[p] += array[p];
                    }
                    //Console.WriteLine(sums.Length + "Sum length");
                }
                allsums.Add(sums);
                GC.Collect();
            }
            //IMidacSpecDataMs specData = m_reader.FrameMs(framenum1, dbin1, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
            int lenallsums = allsums.Count;
            //Console.WriteLine(lenallsums);
            //Console.WriteLine("Done!");
            List<int> intlist = new List<int>();
            for (int i = minDBin; i < maxDBin + 2; i++)
            {
                intlist.Add(i);
            }
            double[] xarray = Specdatareader1(filepath).XArray;
            //float[] b = specData.YArray;
            int xarraylength = xarray.GetLength(0);
            //int xarrayl = xarraylength + 1;
            string docpath = (outputpath);
            string num = Convert.ToString(m);
            string path = docpath +num +"_4" + ".csv";
            GC.Collect();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                for (int i = 0; i < lenallsums; i++)
                {
                    for (int j = 0; j < xarraylength; j++)
                    {
                        if (i == 0)
                        {
                            string xstring = Convert.ToString(xarray[j]);
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                        else
                        {
                            string xstring = "0";
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                    }
                    file.WriteLine("@");
                }
                GC.Collect();
            }
        }
        public Agilent.MassSpectrometry.MIDAC.IMidacSpecDataMs Specdatareader(string filepath, int l, int dbin)
        {
            IMidacSpecDataMs specData1 = m_reader.FrameMs(l, dbin, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
            return specData1;
        }
        public Agilent.MassSpectrometry.MIDAC.IMidacSpecDataMs Specdatareader1(string filepath)
        {
            m_reader = MidacFileAccess.ImsDataReader(filepath);
            IMidacSpecDataMs specData1 = m_reader.FrameMs(1, 1, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
            return specData1;

        }
        public void filereada4(string filepath, int framenum, int numDriftBins, string outputpath, string m)
        {
            int DbinDiv = numDriftBins / 8;
            //int minDBin = 1;
            ////int maxDBin = numDriftBins - 1;
            //int maxDBin = 1;
            //int minDBi;
            //int maxDbi;
            //abc = 3;
            //minDBi = numDriftBins / 4;
            //minDBin = minDBi * 2 + 1;
            int minDBin = (DbinDiv * 4) + 1;
            //maxDbi = numDriftBins / 4;
            //maxDBin = maxDbi * 3;
            int maxDBin = DbinDiv * 5;
            m_reader = MidacFileAccess.ImsDataReader(filepath);


            List<float[]> allsums = new List<float[]>();
            for (int dbin = minDBin; dbin <= maxDBin; dbin++)
            {
                List<float[]> abun = new List<float[]>();

                for (int l = 1; l < framenum; l++)
                {
                    //IMidacSpecDataMs specData1 = m_reader.FrameMs(l, dbin, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
                    float[] b = Specdatareader(filepath, l, dbin).YArray;
                    abun.Add(b);
                }
                //Console.WriteLine(abun.Count);
                GC.Collect();
                //List<float[]> abun2 = new List<float[]>();
                float[] sums = new float[abun[0].Length];
                foreach (float[] array in abun)
                {
                    for (int p = 0; p < sums.Length; p++)
                    {
                        sums[p] += array[p];
                    }
                    //Console.WriteLine(sums.Length + "Sum length");
                }
                allsums.Add(sums);
                GC.Collect();
            }
            //IMidacSpecDataMs specData = m_reader.FrameMs(framenum1, dbin1, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
            int lenallsums = allsums.Count;
            //Console.WriteLine(lenallsums);
            //Console.WriteLine("Done!");
            List<int> intlist = new List<int>();
            for (int i = minDBin; i < maxDBin + 2; i++)
            {
                intlist.Add(i);
            }
            double[] xarray = Specdatareader1(filepath).XArray;
            //float[] b = specData.YArray;
            int xarraylength = xarray.GetLength(0);
            //int xarrayl = xarraylength + 1;
            string docpath = (outputpath);
            string num = Convert.ToString(m);
            string path = docpath + num+"_5" + ".csv";
            GC.Collect();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                for (int i = 0; i < lenallsums; i++)
                {
                    for (int j = 0; j < xarraylength; j++)
                    {
                        if (i == 0)
                        {
                            string xstring = Convert.ToString(xarray[j]);
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                        else
                        {
                            string xstring = "0";
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                    }
                    file.WriteLine("@");
                }
                GC.Collect();
            }
        }
        public void filereada5(string filepath, int framenum, int numDriftBins, string outputpath, string m)
        {
            int DbinDiv = numDriftBins / 8;
            //int minDBin = 1;
            ////int maxDBin = numDriftBins - 1;
            //int maxDBin = 1;
            //int minDBi;
            //int maxDbi;
            //abc = 3;
            //minDBi = numDriftBins / 4;
            //minDBin = minDBi * 2 + 1;
            int minDBin = (DbinDiv * 5) + 1;
            //maxDbi = numDriftBins / 4;
            //maxDBin = maxDbi * 3;
            int maxDBin = DbinDiv * 6;
            m_reader = MidacFileAccess.ImsDataReader(filepath);


            List<float[]> allsums = new List<float[]>();
            for (int dbin = minDBin; dbin <= maxDBin; dbin++)
            {
                List<float[]> abun = new List<float[]>();

                for (int l = 1; l < framenum; l++)
                {
                    //IMidacSpecDataMs specData1 = m_reader.FrameMs(l, dbin, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
                    float[] b = Specdatareader(filepath, l, dbin).YArray;
                    abun.Add(b);
                }
                //Console.WriteLine(abun.Count);
                GC.Collect();
                //List<float[]> abun2 = new List<float[]>();
                float[] sums = new float[abun[0].Length];
                foreach (float[] array in abun)
                {
                    for (int p = 0; p < sums.Length; p++)
                    {
                        sums[p] += array[p];
                    }
                    //Console.WriteLine(sums.Length + "Sum length");
                }
                allsums.Add(sums);
                GC.Collect();
            }
            //IMidacSpecDataMs specData = m_reader.FrameMs(framenum1, dbin1, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
            int lenallsums = allsums.Count;
            //Console.WriteLine(lenallsums);
            //Console.WriteLine("Done!");
            List<int> intlist = new List<int>();
            for (int i = minDBin; i < maxDBin + 2; i++)
            {
                intlist.Add(i);
            }
            double[] xarray = Specdatareader1(filepath).XArray;
            //float[] b = specData.YArray;
            int xarraylength = xarray.GetLength(0);
            //int xarrayl = xarraylength + 1;
            string docpath = (outputpath);
            string num = Convert.ToString(m);
            string path = docpath +num +"_6" + ".csv";
            GC.Collect();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                for (int i = 0; i < lenallsums; i++)
                {
                    for (int j = 0; j < xarraylength; j++)
                    {
                        if (i == 0)
                        {
                            string xstring = Convert.ToString(xarray[j]);
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                        else
                        {
                            string xstring = "0";
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                    }
                    file.WriteLine("@");
                }
                GC.Collect();
            }
        }
        public void filereada6(string filepath, int framenum, int numDriftBins, string outputpath, string m)
        {
            int DbinDiv = numDriftBins / 8;
            //int minDBin = 1;
            ////int maxDBin = numDriftBins - 1;
            //int maxDBin = 1;
            //int minDBi;
            //int maxDbi;
            //abc = 3;
            //minDBi = numDriftBins / 4;
            //minDBin = minDBi * 2 + 1;
            int minDBin = (DbinDiv * 6) + 1;
            //maxDbi = numDriftBins / 4;
            //maxDBin = maxDbi * 3;
            int maxDBin = DbinDiv * 7;
            m_reader = MidacFileAccess.ImsDataReader(filepath);


            List<float[]> allsums = new List<float[]>();
            for (int dbin = minDBin; dbin <= maxDBin; dbin++)
            {
                List<float[]> abun = new List<float[]>();

                for (int l = 1; l < framenum; l++)
                {
                    //IMidacSpecDataMs specData1 = m_reader.FrameMs(l, dbin, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
                    float[] b = Specdatareader(filepath, l, dbin).YArray;
                    abun.Add(b);
                }
                //Console.WriteLine(abun.Count);
                GC.Collect();
                //List<float[]> abun2 = new List<float[]>();
                float[] sums = new float[abun[0].Length];
                foreach (float[] array in abun)
                {
                    for (int p = 0; p < sums.Length; p++)
                    {
                        sums[p] += array[p];
                    }
                    //Console.WriteLine(sums.Length + "Sum length");
                }
                allsums.Add(sums);
                GC.Collect();
            }
            //IMidacSpecDataMs specData = m_reader.FrameMs(framenum1, dbin1, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
            int lenallsums = allsums.Count;
            //Console.WriteLine(lenallsums);
            //Console.WriteLine("Done!");
            List<int> intlist = new List<int>();
            for (int i = minDBin; i < maxDBin + 2; i++)
            {
                intlist.Add(i);
            }
            double[] xarray = Specdatareader1(filepath).XArray;
            //float[] b = specData.YArray;
            int xarraylength = xarray.GetLength(0);
            //int xarrayl = xarraylength + 1;
            string docpath = (outputpath);
            string num = Convert.ToString(m);
            string path = docpath + num+"_7" + ".csv";
            GC.Collect();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                for (int i = 0; i < lenallsums; i++)
                {
                    for (int j = 0; j < xarraylength; j++)
                    {
                        if (i == 0)
                        {
                            string xstring = Convert.ToString(xarray[j]);
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                        else
                        {
                            string xstring = "0";
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                    }
                    file.WriteLine("@");
                }
                GC.Collect();
            }
        }
        public void filereada7(string filepath, int framenum, int numDriftBins, string outputpath, string m)
        {
            int DbinDiv = numDriftBins / 8;
            //int minDBin = 1;
            ////int maxDBin = numDriftBins - 1;
            //int maxDBin = 1;
            //int minDBi;
            //int maxDbi;
            //abc = 3;
            //minDBi = numDriftBins / 4;
            //minDBin = minDBi * 2 + 1;
            int minDBin = (DbinDiv * 7) + 1;
            //maxDbi = numDriftBins / 4;
            //maxDBin = maxDbi * 3;
            int maxDBin = numDriftBins-1;
            m_reader = MidacFileAccess.ImsDataReader(filepath);


            List<float[]> allsums = new List<float[]>();
            for (int dbin = minDBin; dbin <= maxDBin; dbin++)
            {
                List<float[]> abun = new List<float[]>();

                for (int l = 1; l < framenum; l++)
                {
                    //IMidacSpecDataMs specData1 = m_reader.FrameMs(l, dbin, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
                    float[] b = Specdatareader(filepath, l, dbin).YArray;
                    abun.Add(b);
                }
                //Console.WriteLine(abun.Count);
                GC.Collect();
                //List<float[]> abun2 = new List<float[]>();
                float[] sums = new float[abun[0].Length];
                foreach (float[] array in abun)
                {
                    for (int p = 0; p < sums.Length; p++)
                    {
                        sums[p] += array[p];
                    }
                    //Console.WriteLine(sums.Length + "Sum length");
                }
                allsums.Add(sums);
                GC.Collect();
            }
            //IMidacSpecDataMs specData = m_reader.FrameMs(framenum1, dbin1, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
            int lenallsums = allsums.Count;
            //Console.WriteLine(lenallsums);
            //Console.WriteLine("Done!");
            List<int> intlist = new List<int>();
            for (int i = minDBin; i < maxDBin + 2; i++)
            {
                intlist.Add(i);
            }
            double[] xarray = Specdatareader1(filepath).XArray;
            //float[] b = specData.YArray;
            int xarraylength = xarray.GetLength(0);
            //int xarrayl = xarraylength + 1;
            string docpath = (outputpath);
            string num = Convert.ToString(m);
            string path = docpath +num+ "_8" + ".csv";
            GC.Collect();
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
            {
                for (int i = 0; i < lenallsums; i++)
                {
                    for (int j = 0; j < xarraylength; j++)
                    {
                        if (i == 0)
                        {
                            string xstring = Convert.ToString(xarray[j]);
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                        else
                        {
                            string xstring = "0";
                            float[] ystring1 = allsums[i];
                            string ystring = Convert.ToString(ystring1[j]);
                            string xy = xstring + "," + ystring;
                            file.WriteLine(xy);
                        }
                    }
                    file.WriteLine("@");
                }
                GC.Collect();
            }
        }
        //public void filereada7(string filepath, int framenum, int numDriftBins, string outputpath)
        //{
        //    int DbinDiv = numDriftBins / 8;
        //    //int minDBin = 1;
        //    ////int maxDBin = numDriftBins - 1;
        //    //int maxDBin = 1;
        //    //int minDBi;
        //    //int maxDbi;
        //    //abc = 3;
        //    //minDBi = numDriftBins / 4;
        //    //minDBin = minDBi * 2 + 1;
        //    int minDBin = (DbinDiv * 7) + 1;
        //    //maxDbi = numDriftBins / 4;
        //    //maxDBin = maxDbi * 3;
        //    int maxDBin = DbinDiv * 8;
        //    m_reader = MidacFileAccess.ImsDataReader(filepath);


        //    for (int dbin = minDBin; dbin <= maxDBin; dbin++)
        //    {
        //        List<float[]> abun = new List<float[]>();

        //        //if (l % 4 == 0)
        //        //{
        //        //    //Agilent.MassSpectrometry.MIDAC.MidacImsFileReader.ReferenceEquals(null);
        //        //    //Agilent.MassSpectrometry.DataAnalysis.BdaImsFileAccess = null;
        //        //    //Agilent.MassSpectrometry.DataAnalysis.IMS.MsScanInfoAccess = null;
        //        //    m_reader.Close();
        //        //    m_reader = null;
        //        //    GC.Collect();
        //        //    GC.WaitForPendingFinalizers();
        //        //    //Component.Dispose();
        //        //    Console.WriteLine("hi!");
        //        //    m_reader = MidacFileAccess.ImsDataReader(filepath);

        //        //}
        //        for (int l = 1; l < framenum; l++)
        //        {
        //            //IMidacSpecDataMs specData1 = m_reader.FrameMs(l, dbin, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
        //            float[] b = Specdatareader(filepath, l, dbin).YArray;
        //            abun.Add(b);
        //        }
        //        GC.Collect();
        //        float[] sums = new float[abun[0].Length];
        //        foreach (float[] array in abun)
        //        {
        //            for (int p = 0; p < sums.Length; p++)
        //            {
        //                sums[p] += array[p];
        //            }
        //        }
        //        GC.Collect();
        //        //IMidacSpecDataMs specData = m_reader.FrameMs(framenum1, dbin1, MidacSpecFormat.Profile, false) as IMidacSpecDataMs;
        //        double[] xarray = Specdatareader1(filepath).XArray;
        //        //float[] b = specData.YArray;
        //        int xarraylength = xarray.GetLength(0);
        //        //int xarrayl = xarraylength + 1;
        //        string docpath = (outputpath);
        //        string stri = Convert.ToString(dbin);
        //        string filenum = stri;
        //        string test = Convert.ToString(abc);
        //        string path = docpath + test + "_" + filenum + ".csv";
        //        GC.Collect();
        //        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
        //        {
        //            for (int j = 0; j < xarraylength; j++)
        //            {
        //                string xstring = Convert.ToString(xarray[j]);
        //                string ystring = Convert.ToString(sums[j]);
        //                string xy = xstring + "," + ystring;
        //                file.WriteLine(xy);
        //            }
        //        }
        //        GC.Collect();
        //    }
        //}
        //public void LMFAO(int l)
        //{
        //    if (l % 12==0)
        //    {
        //        m_reader.Close();
        //        m_reader = MidacFileAccess.ImsDataReader(filepath);
        //    }
        //}



        /*
        List<float[]> test3 = new List<float[]>();
        for (int i=0; i <5; i++)
        {
            float[] test1 = { 1, 2, 3, 4, 5 };
            Console.WriteLine("hi");
            test3.Add(test1);
        }
        int floatlength = abun[0].Length;
        int testlength = test3[0].Length;
        Console.WriteLine(test3[2][2]);
        float[] sums = new float[test3[0].Length];
        foreach (float[] array in test3)
        {
            for (int i = 0; i < sums.Length; i++)
            {
                sums[i] += array[i];
            }
        }
        Console.WriteLine(sums[1]);


    for (int i=1; i<test.Count+1; i++)
    {
        Console.WriteLine(test[i]);
        for (int j = 0; i < testlength; i++)
        {
            var x = j;
        }
    }
        */
        /*
        List<float[]> floatlist = new List<float[]>();
        int floatlength = abun[0].Length;
        List<float[]> sums = new List<float[]>();
        foreach(float[] a in abun)
        {
            for (int i = 0; i < floatlength; i++)
            {
                sums.Add(floatlist[i]);
            }

        }
        */
        /*
        for (int i = 0; i < abun.Count; i++)
        {
            for (int j = 0; j < floatlength; j++)
            {
                float test1 = abun[i][j];
                float test2 = abun[i+1][j];
            }
        }


        /*
        int xarraylength = xarray.GetLength(0);
        //int xarrayl = xarraylength + 1;
        string docpath = (@"C:\Users\Lily\Desktop\Chop_file\1_");
        string stri = Convert.ToString(dbin);
        string filenum = stri;
        string path = docpath + filenum + ".csv";

        using (System.IO.StreamWriter file = new System.IO.StreamWriter(path))
        {
            for (int j = 0; j < xarraylength; j++)
            {
                string xstring = Convert.ToString(xarray[j]);
                string ystring = Convert.ToString(b[j]);
                string xy = xstring + "," + ystring;
                file.WriteLine(xy);
            }
            }
        */


    }
}       
