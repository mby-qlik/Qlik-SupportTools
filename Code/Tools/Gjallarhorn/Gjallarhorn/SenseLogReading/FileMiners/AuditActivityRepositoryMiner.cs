﻿using System.Diagnostics;

namespace Gjallarhorn.SenseLogReading.FileMiners
{
    public class AuditActivityRepositoryMiner : BaseDataMiner, IDataMiner
    {
        public string MinerName => "AuditActivity_Repository";

        private int _userIdColumnNr = -1;
        private int _objectIdColumnNr = -1;

        public AuditActivityRepositoryMiner()
        {
            base.DataMinerSettings.NeedDatePerRow = true;
        }

        public void Mine(string line)
        {
            base.MineFile(line, Analyze);
        }

        public override void InitializeNewFile(string headerLine, BasicDataFromCase basicDataFromCase, string path)
        {
            base.InitializeNewFile(headerLine, basicDataFromCase, path);

            if (!base.ColumnNames.TryGetValue("userid", out _userIdColumnNr))
            {
                //todo:logging
                Trace.WriteLine("Failed finding userId column in log {}");
            }
            if (!base.ColumnNames.TryGetValue("objectid", out _objectIdColumnNr))
            {
                //todo:logging
                Trace.WriteLine("Failed finding objectid column in log {}");
            }
        }

        public void FinaliseStatistics()
        {

        }

        private void Analyze(int colNr, string value)
        {
            if (colNr == _userIdColumnNr)
            {
                if (base.BasicDataFromCase.TotalUniqueActiveUsersList.ContainsKey(value))
                    base.BasicDataFromCase.TotalUniqueActiveUsersList[value]++;
                else
                    base.BasicDataFromCase.TotalUniqueActiveUsersList[value] = 1;
            }
            else if (colNr == _objectIdColumnNr)
            {
                if (base.BasicDataFromCase.TotalUniqueActiveAppsList.ContainsKey(value))
                    base.BasicDataFromCase.TotalUniqueActiveAppsList[value]++;
                else
                    base.BasicDataFromCase.TotalUniqueActiveAppsList[value] = 0;
            }
        }
    }
}
