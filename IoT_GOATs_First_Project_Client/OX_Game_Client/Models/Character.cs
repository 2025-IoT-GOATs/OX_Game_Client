using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX_Game_Client.Models
{
    public class Character : ObservableObject
    {
        private double x = 100;

        public double X
        {
            get => x;
            set => SetProperty(ref x, value);
        }

        private double y = 100;

        public double Y
        {
            get => y;
            set => SetProperty(ref y, value);
        }
    }
}
