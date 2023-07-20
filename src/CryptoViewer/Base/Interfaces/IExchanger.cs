using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoViewer.Base.Interfaces
{
    internal interface IExchanger
    {
        string Id { get; }
        string Name { get; }
        float? VolumeUsd { get; }
    }
}
