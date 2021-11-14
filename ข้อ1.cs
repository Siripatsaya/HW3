using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace ThreadCpu.Class
{
    public class Program
    {
        #region 

        #endregion

        #region Fields
        private int _cpuCount = 4;
        private System.Threading.Thread[] _cpu = null;
        private System.Threading.ThreadStart[] _cpuStart = null;
        private List<ThreadCpu> _listThreadCpu = null;
        private int _count = 0;
        private List<string[]> _listData = null;
        #endregion

        #region Constructor

        #region Program
        /// <summary>
        /// Constructor
        /// </summary>
        public Program()
        {
            _cpu = new System.Threading.Thread[_cpuCount];
            _cpuStart = new System.Threading.ThreadStart[_cpuCount];
            _listThreadCpu = new List<ThreadCpu>();
            _listData = GetlistData();
        }
        #endregion

        #endregion

        #region Public Methods

        #region StartThread
        /// <summary>
        /// StartThread
        /// </summary>
        public void Main()
        {
            string input1, input2, input3, input4, input5, input6, input7, input8, input9;
            input1 = Console.ReadLine();
            input2 = Console.ReadLine();
            input3 = Console.ReadLine();
            input4 = Console.ReadLine();
            input5 = Console.ReadLine();
            input6 = Console.ReadLine();
            input7 = Console.ReadLine();
            input8 = Console.ReadLine();
            input9 = Console.ReadLine();

            List<string[]> addlistData = new List<string[]>();
            List<string[]> templistData = new List<string[]>();

            for (int i = 0; i < _cpuCount; i++)
            {
                _listThreadCpu.Add(new ThreadCpu(i + 1));
                _cpuStart[i] = new ThreadStart(_listThreadCpu[i].StartThread);
                _cpu[i] = new System.Threading.Thread(_cpuStart[i]);
                _cpu[i].Start();
            }


            for (int i = 0; i < _listData.Count; i++)
            {
                Console.WriteLine(string.Format("Data {0} ('{1}','{2}')", i + 1, _listData[i][0], _listData[i][1]));
            }

            for (int i = 0; i < _listData.Count; i++)
            {
                if (SetValueToThreadCpu(_listThreadCpu[0], _listData[i]))
                {
                    addlistData.Add(_listData[i]);
                }
                else if (!string.IsNullOrEmpty(_listThreadCpu[0].Instruction) && _listThreadCpu[0].Instruction == _listData[i][0])
                {
                    templistData.Add(_listData[i]);
                }
                else if (SetValueToThreadCpu(_listThreadCpu[1], _listData[i]))
                {
                    addlistData.Add(_listData[i]);
                }
                else if (!string.IsNullOrEmpty(_listThreadCpu[0].Instruction) && _listThreadCpu[1].Instruction == _listData[i][0])
                {
                    templistData.Add(_listData[i]);
                }
                else if (SetValueToThreadCpu(_listThreadCpu[2], _listData[i]))
                {
                    addlistData.Add(_listData[i]);
                }
                else if (!string.IsNullOrEmpty(_listThreadCpu[0].Instruction) && _listThreadCpu[2].Instruction == _listData[i][0])
                {
                    templistData.Add(_listData[i]);
                }
                else if (SetValueToThreadCpu(_listThreadCpu[3], _listData[i]))
                {
                    addlistData.Add(_listData[i]);
                }
                else if (!string.IsNullOrEmpty(_listThreadCpu[0].Instruction) && _listThreadCpu[3].Instruction == _listData[i][0])
                {
                    templistData.Add(_listData[i]);
                }
                else
                {
                    templistData.Add(_listData[i]);
                }

                if (IsMaxCpu(_listThreadCpu))
                {
                    // Cpu Max Reset Data
                    _count += 1;
                    WriteLogCpu(_listThreadCpu, _count);
                    _listThreadCpu[0] = new ThreadCpu(1);
                    _listThreadCpu[1] = new ThreadCpu(2);
                    _listThreadCpu[2] = new ThreadCpu(3);
                    _listThreadCpu[3] = new ThreadCpu(4);
                    addlistData = new List<string[]>();
                    for (int j = i + 1; j < _listData.Count; j++)
                    {
                        // Add Temp To Loop
                        templistData.Add(_listData[j]);
                    }
                    _listData = templistData;
                    templistData = new List<string[]>();
                    i = -1;

                    if (_listData.Count == 0)
                    {
                        // End Process
                        Console.WriteLine(string.Format("CPU Cycles Needed: {0}", _count));
                        break;
                    }
                }

                if (_listData.Count == addlistData.Count)
                {
                    _count += 1;
                    WriteLogCpu(_listThreadCpu, _count);
                    // End Process
                    Console.WriteLine(string.Format("CPU Cycles Needed: {0}", _count));
                }

            }
        }
        #endregion

        #endregion

        #region Private Methods

        #region GetlistData
        /// <summary>
        /// GetlistData
        /// </summary>
        /// <returns>List<string[]></returns>
        private List<string[]> GetlistData()
        {
            List<string[]> listOflistData = new List<string[]>();
            listOflistData.Add(new string[] { "A", "P" });
            listOflistData.Add(new string[] { "A", "Q" });
            listOflistData.Add(new string[] { "B", "Q" });
            listOflistData.Add(new string[] { "C", "R" });
            listOflistData.Add(new string[] { "A", "P" });
            listOflistData.Add(new string[] { "A", "R" });
            listOflistData.Add(new string[] { "A", "S" });
            listOflistData.Add(new string[] { "D", "Q" });
            listOflistData.Add(new string[] { "E", "R" });
            return listOflistData;
        }
        #endregion

        #region SetValueToThreadCpu
        /// <summary>
        /// SetValueToThreadCpu
        /// </summary>
        /// <param name="threadCpu"></param>
        /// <param name="Data"></param>
        /// <returns>IsUseCpu</returns>
        private bool SetValueToThreadCpu(ThreadCpu threadCpu, string[] Data)
        {
            bool result = false;
            string dataCpu = string.Empty;

            if (string.IsNullOrEmpty(threadCpu.Instruction))
            {
                threadCpu.Instruction = Data[0];
            }

            if (threadCpu.Instruction == Data[0])
            {

                if (string.IsNullOrEmpty(threadCpu.Data1))
                {
                    threadCpu.Data1 = Data[1];
                    result = true;
                    return result;
                }
                else if (threadCpu.Data1 == Data[1])
                {
                    result = true;
                    return result;
                }

                if (string.IsNullOrEmpty(threadCpu.Data2))
                {
                    threadCpu.Data2 = Data[1];
                    result = true;
                    return result;
                }
                else if (threadCpu.Data2 == Data[1])
                {
                    result = true;
                    return result;
                }

                if (string.IsNullOrEmpty(threadCpu.Data3))
                {
                    threadCpu.Data3 = Data[1];
                    result = true;
                    return result;
                }
                else if (threadCpu.Data3 == Data[1])
                {
                    result = true;
                    return result;
                }

            }
            return result;
        }
        #endregion

        #region IsMaxCpu
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listThreadCpu"></param>
        /// <returns></returns>
        private bool IsMaxCpu(List<ThreadCpu> listThreadCpu)
        {
            bool result = false;

            for (int i = 0; i < listThreadCpu.Count; i++)
            {
                if (!string.IsNullOrEmpty(listThreadCpu[i].Instruction))
                {
                    result = true;
                }
                else
                {
                    result = false;
                    return result;
                }
            }

            return result;
        }
        #endregion

        #region WriteLogCpu
        /// <summary>
        /// 
        /// </summary>
        /// <param name="listThreadCpu"></param>
        /// <param name="count"></param>
        private void WriteLogCpu(List<ThreadCpu> listThreadCpu, int count)
        {
            Console.WriteLine(string.Format("CPU Time {0}", count));
            for (int i = 0; i < listThreadCpu.Count; i++)
            {
                Console.WriteLine(string.Format("CPU {0}-{1} [{2}][{3}][{4}]", i + 1, listThreadCpu[i].Instruction, listThreadCpu[i].Data1, listThreadCpu[i].Data2, listThreadCpu[i].Data3));
            }
        }
        #endregion

        #endregion

    }
}

namespace ThreadCpu.Class
{
    #region ThreadCpu
    public class ThreadCpu
    {

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int CpuNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Instruction { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Data1 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Data2 { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Data3 { get; set; }

        #endregion

        #region Constructor

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cpuNo"></param>
        public ThreadCpu(int cpuNo)
        {
            this.CpuNo = cpuNo;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// 
        /// </summary>
        public void StartThread()
        {
            Console.WriteLine(string.Format("Start CPU NO. {0}", this.CpuNo));
        }

        #endregion

    }
}
#endregion