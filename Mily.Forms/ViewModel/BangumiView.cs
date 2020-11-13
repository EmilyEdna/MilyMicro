using Mily.Forms.Core;
using Mily.Forms.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Forms.ViewModel
{
    public class BangumiView : BaseView
    {
        public BangumiView()
        {
            VedioURL = new Uri(Imomoe.GetVedio());
        }


        private Uri _VedioURL;
        public Uri VedioURL
        {
            get { return _VedioURL; }
            set
            {
                _VedioURL = value;
                OnPropertyChanged("VedioURL");
            }
        }

    }
}
