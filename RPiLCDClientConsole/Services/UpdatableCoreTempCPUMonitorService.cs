﻿using System;
using GetCoreTempInfoNET;
using System.Linq;
using RPiLCDShared.Services;

namespace RPiLCDClientConsole.Services
{
    public class UpdatableCoreTempCPUMonitorService : UpdateableService, IUpdatableService
    {
        private readonly Tag _tabTag = UpdateTags.CPUTabTag;
        private readonly CoreTempInfo _coreTemp;
        private readonly ILoggingService _loggingService;

        public UpdatableCoreTempCPUMonitorService(ILoggingService loggingService)
        {
            _loggingService = loggingService;
            _coreTemp = new CoreTempInfo();
            _coreTemp.ReportError += new ErrorOccured(CoreTemp_ReportError);
        }

        public Tag GetTabTag()
        {
            return _tabTag;
        }

        public string GetUpdateString()
        {
            try
            {
                _coreTemp.GetData();

                var coreLoad= string.Join(",", _coreTemp.GetCoreLoad.Take(4));

                var str = coreLoad.ToString();
                return TagifyString(coreLoad.ToString(), UpdateTags.CPULoadTag);

            }
            catch (Exception e)
            {
                _loggingService.LogError(e.Message);
            }

            return "";
        }

        void CoreTemp_ReportError(ErrorCodes ErrCode, string ErrMsg)
        {
            _loggingService.LogError(ErrMsg);
        }

        public void StartService()
        {
        }

        public void EndService()
        {
        }

    }
}
