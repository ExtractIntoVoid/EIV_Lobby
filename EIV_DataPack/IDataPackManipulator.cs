using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIV_DataPack
{
    public interface IDataPackManipulator
    {
        public DataPack Pack { get; internal set; }
        public void Open();
        public void Close();
    }
}
