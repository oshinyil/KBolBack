using System.Collections.Generic;
using System.Web.Mvc;

namespace KBolBack.Web.Models
{
    public class SharedData
    {
        public static SelectList GetAnswerLetters()
        {
            return new SelectList(  
                new List<SelectListItem>
                {
                    new SelectListItem { Text = "A", Value = "A" },
                    new SelectListItem { Text = "B", Value = "B" },
                    new SelectListItem { Text = "C", Value = "C" },
                    new SelectListItem { Text = "D", Value = "D" }
                }, "Value", "Text");
        }
    }
}