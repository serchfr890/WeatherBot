﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherEchoBotV4.Models
{
    public class QnAMakerModel
    {
        public List<Answers> Answers { get; set; }
    }

    public class Answers
    {
        public List<string> Questions { get; set; }
        public string Answer { get; set; }
        public double Score { get; set; }
        public int Id { get; set; }
        public string Source { get; set; } 
        public List<Metadata> Metada { get;set; }

        public class Metadata
        {
            public string Name { get; set; }
            public string Value { get; set; }
        }
    }
}
