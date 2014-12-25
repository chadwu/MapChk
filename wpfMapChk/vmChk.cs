using System;
using System.ComponentModel;
using System.Windows.Input;


namespace wpfMapChk
{
    class vmChk : INotifyPropertyChanged
    {
        //public MODEL model { get; set; }
        private bool _bScreenSaverFlag;
        private string _sTitle;

        public event PropertyChangedEventHandler PropertyChanged;
        //定義一個ICommand型別的參數，他會回傳實作ICommand介面的RelayCommand類別。
        public ICommand UpdateTitle { get { return new RelayCmd(UpdateTitleExecute, CanUpdateTitleExecute); } }

        public vmChk()
        {
            _bScreenSaverFlag = ScrSaver;
            _sTitle = "地圖切換計時器";
        }

        //產生事件的方法
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        //更新Title，原本放在View那邊的邏輯，藉由繫結的方式來處理按下Button的事件。
        void UpdateTitleExecute(string parameter)
        {
            _sTitle = parameter;
        }

        //定義是否可以更新Title
        bool CanUpdateTitleExecute()
        {
            return true;
        }

        public void Recovery()
        {
            ScrSaver = _bScreenSaverFlag;
        }


        public bool ScrSaver
        {
            get { return ScreenSaver.Check(); }
            set
            {
                if (value)
                    ScreenSaver.Enable();
                else
                    ScreenSaver.Disable();
            }
        }
    }
}
