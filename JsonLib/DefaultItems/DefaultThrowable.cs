﻿using JsonLib.Interfaces;

namespace JsonLib.DefaultItems
{
    public class DefaultThrowable : IThrowable
    {
        public decimal FuseTime { get; set; }
        public bool CanUse { get; set; }
        public float UseTime { get; set; }
        public string BaseID { get; set; } = string.Empty;
        public string ItemType { get; set; } = nameof(IThrowable);
        public decimal Weight { get; set; }
        public string AssetPath { get; set; } = string.Empty;
        public List<string> Tags { get; set; } = [];
        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public override string ToString()
        {
            return $"{BaseID} {ItemType} {Weight} {AssetPath}";
        }
    }
}