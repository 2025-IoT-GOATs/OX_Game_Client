﻿using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OX_Game_Client.Models
{
    public partial class Character : ObservableObject
    {
        [ObservableProperty]
        private string userName;
        [ObservableProperty]
        private double x;
        [ObservableProperty]
        private double y;
        [ObservableProperty]
        private bool isExist = false;

        public Character(string name, double x, double y)
        {
            UserName = name;
            X = x;
            Y = y;
        }

    }
}
