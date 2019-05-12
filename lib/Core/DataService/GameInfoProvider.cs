using InnateGlory.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnateGlory
{
    public class GameTypeInfoProvider : IDataService
    {
        private DataService _dataService;

        public GameTypeInfoProvider(DataService dataService)
        {
            this._dataService = dataService;
        }

        public GameType this[int id] => throw new NotImplementedException();
        public GameType this[string name] => throw new NotImplementedException();
    }
}
