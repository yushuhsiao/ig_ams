using InnateGlory.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnateGlory
{
    public class GameInfoProvider
    {
        private DataService _dataService;

        public GameInfoProvider(DataService dataService)
        {
            this._dataService = dataService;
        }

        public GameInfo this[int id] => throw new NotImplementedException();
        public GameInfo this[string name] => throw new NotImplementedException();
    }
}
