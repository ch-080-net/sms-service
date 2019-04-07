using System;
using System.Collections.Generic;
using System.Text;
using Model.ViewModels.TestMessageViewModels;

namespace BAL.Interfaces
{
    public interface ITestMessageManager
    {
        void SendTestMessage(TestMessageViewModel message);
    }
}
