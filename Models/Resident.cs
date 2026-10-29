using System;
using System.Collections.Generic;
using System.Text;

namespace HostelRoomManagement.Models;

public class Resident
{
    public int ResidentId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Department { get; set; } = string.Empty;
}