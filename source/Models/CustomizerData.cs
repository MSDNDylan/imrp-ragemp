using System;
using System.Collections.Generic;
using System.Text;

namespace IMRP.Models
{
    public class CustomizerData
    {
        public List<Head> head { get; set; }
        public int gender { get; set; }
        public Hair hair { get; set; }
        public int skinColor { get; set; }
        public int eyeColor { get; set; }
        public Face face { get; set; }
        public Clothes clothes { get; set; }

        public class Head
        {
            public int index { get; set; }
            public float opacity { get; set; }
            public int color { get; set; }
        }

        public class Hair
        {
            public int style { get; set; }
            public int color { get; set; }
            public int highlights { get; set; }
        }

        public class Face
        {
            public int mother { get; set; }
            public int father { get; set; }
            public int character { get; set; }
            public float resemblance { get; set; }
            public List<float> data { get; set; }
        }

        public class Top
        {
            public string ctype { get; set; }
            public int cid { get; set; }
        }

        public class Pant
        {
            public string ctype { get; set; }
            public int cid { get; set; }
        }

        public class Shoes
        {
            public string ctype { get; set; }
            public int cid { get; set; }
        }

        public class Clothes
        {
            public Top top { get; set; }
            public Pant pant { get; set; }
            public Shoes shoes { get; set; }
        }
    }
}
