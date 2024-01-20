﻿using System.Collections.Generic;
 using System.ComponentModel.DataAnnotations;
 using System.Linq;
 using GenmaWebApp.Models;

 namespace GenmaWebApp.Models
{
    public class Searchclass
    {
        public Searchclass()
        {
            Searchtags = new List<int>();
        }
        [StringLength(100)] public string searchword { get; set; }
        public List<int> Searchtags { get; set; }
        public bool random { get; set; }
        public bool gluten { get; set; }
        public bool vegan { get; set; }
        public bool vegetarian { get; set; }
        public bool dairy { get; set; } 
        public bool favorites { get; set; }
        public bool egg { get; set; }
        public bool nuts { get; set; }
        
    }
    
} 