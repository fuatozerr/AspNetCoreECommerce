﻿using ECommerce.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ECommerce.WebUI.Models
{
    public class CategoryListModel
    {
        public List<Category> Categories { get; set; }
        public string SelectedCategory { get; set; }
    }
}
