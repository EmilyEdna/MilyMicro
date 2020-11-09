using Mily.Forms.Core;
using Mily.Forms.DataModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Forms.DataModel
{
    public class TagView : BaseView
    {
        public TagView()
        {
            Count = Konachan.GetTotalTag();
        }

        #region Property
        private long _Count;
        public long Count
        {
            get { return _Count; }
            set
            {
                _Count = value;
                OnPropertyChanged("Count");
            }
        }
        #endregion
    }
}
