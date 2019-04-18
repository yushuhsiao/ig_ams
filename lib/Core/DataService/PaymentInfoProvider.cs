using InnateGlory.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace InnateGlory
{
    public class PaymentInfoProvider 
    {
        private DataService _dataService;

        public PaymentInfoProvider(DataService dataService)
        {
            this._dataService = dataService;
        }

        public PaymentInfo this[Guid id] => throw new NotImplementedException();
        public IEnumerable<PaymentInfo> this[string name] => throw new NotImplementedException();
        public PaymentInfo this[CorpInfo corp, Guid id] => throw new NotImplementedException();
        public PaymentInfo this[CorpInfo corp, string name] => throw new NotImplementedException();
        public PaymentInfo this[Agent parent, Guid id] => throw new NotImplementedException();
        public PaymentInfo this[Agent parent, string name] => throw new NotImplementedException();
    }
}
