using System;
using System.Collections.Generic;

namespace ClassLibrary1.Models;

public partial class StudentInfo
{
    public int StudentId { get; set; }

    public int UserId { get; set; }

    public string StudentCardImage { get; set; }

    public string StudentCode { get; set; }

    public string University { get; set; }

    public DateTime? CreatedDate { get; set; }

    public virtual User User { get; set; }
}
