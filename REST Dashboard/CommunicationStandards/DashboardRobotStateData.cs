﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REST_Dashboard
{
    public class DashboardRobotStateData : RobotStateData, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };
        public string label { get; set; }

        public DashboardRobotStateData(string label_in) : base()
        {
            label = label_in;
        }
        public override byte[] Serialize()
        {
            throw new NotFiniteNumberException();
        }

        public override void Deserialize(byte[] data)
        {
            

            PropertyChanged(this, new PropertyChangedEventArgs("Deserialized"));
        }
    }
}
