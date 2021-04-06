using BuyFood_Template.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BuyFood_Template.ViewModels
{
    public class yuBoardViewModel
    {
        private TBoard iv_Board = null;
        public TBoard Board { get { return iv_Board; } }

        public yuBoardViewModel(TBoard p)
        {
            iv_Board = p;
        }
        public yuBoardViewModel()
        {
            iv_Board = new TBoard();
        }


        public int CBoardId { get; set; }
        public int? CProductId { get; set; }
        public int CMemberId { get; set; }
        public decimal? CGrades { get; set; }
        public string CContent { get; set; }
        public string CPicture { get; set; }
        public DateTime? CBoardTime { get; set; }
        public string CBordStatus { get; set; }

        public virtual TMember CMember { get; set; }
        public virtual TProduct CProduct { get; set; }
    }

}
