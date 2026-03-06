
class AwesomeClass
{
    private static List<UserRecord> why = new List<UserRecord>();
    public static List<UserRecord> Load(String filePath)
    {

        //string totallynotfilePath = "OGE.csv";
        using StreamReader sr = new StreamReader(filePath);
        {
            sr.ReadLine();
        
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                string[] hehehehehe = line.Split(',');
                UserRecord heherecordhehehe = new UserRecord(hehehehehe[0], hehehehehe[1], hehehehehe[2], hehehehehe[3], hehehehehe[4] == "TRUE", hehehehehe[5], hehehehehe[6] == "TRUE", hehehehehe[7], hehehehehe[8], hehehehehe[9], hehehehehe[10], hehehehehe[11], hehehehehe[12]);
                why.Add(heherecordhehehe);
            }

        }
        return why;

    }

   public static int GetTotalInactiveUsersWithAccess(string filePath)
{
    Load(filePath);

    var query =
        (from record in why
         where record.cloudLife == false
               && !string.IsNullOrWhiteSpace(record.access1DisplayName)
         group record by record.identityId into userGroup
         select userGroup.Key);

    return query.Count();
}
public static Dictionary<string, string> GetInactiveUsersWithSystems(string filePath)
{
    Load(filePath);

    var query =
        from record in why
        where record.cloudLife == false
              && !string.IsNullOrWhiteSpace(record.access1DisplayName)
        group record by record.displayName into userGroup
        select new
        {
            Name = userGroup.Key,
            Systems = string.Join(", ",
                        (from r in userGroup
                         select r.access1DisplayName).Distinct())
        };

    return query.ToDictionary(x => x.Name, x => x.Systems);
}
public static Dictionary<string, int> GetInactiveUsersPerDepartment(string filePath)
{
    Load(filePath);

    var query =
        from record in why
        where record.cloudLife == false
              && !string.IsNullOrWhiteSpace(record.access1DisplayName)
              && !string.IsNullOrWhiteSpace(record.department)
        group record by record.department into deptGroup
        orderby deptGroup.Key
        select new
        {
            Department = deptGroup.Key,
            Count = (from r in deptGroup
                     group r by r.identityId into userGroup
                     select userGroup.Key).Count()
        };

    return query.ToDictionary(x => x.Department, x => x.Count);
}
}
  public struct UserRecord
{
    public UserRecord(
        string displayName,
        string firstName,
        string lastName,
        string workEmail,
        bool cloudLife,
        string identityId,
        bool isManager,
        string department,
        string jobTitle,
        string access1Type,
        string access1Source,
        string access1DisplayName,
        string access1Description)
    {
        this.displayName = displayName;
        this.firstName = firstName;
        this.lastName = lastName;
        this.workEmail = workEmail;
        this.cloudLife = cloudLife;
        this.identityId = identityId;
        this.isManager = isManager;
        this.department = department;
        this.jobTitle = jobTitle;
        this.access1Type = access1Type;
        this.access1Source = access1Source;
        this.access1DisplayName = access1DisplayName;
        this.access1Description = access1Description;
    }

    public string displayName { get; set; }
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string workEmail { get; set; }
    public bool cloudLife { get; set; }
    public string identityId { get; set; }
    public bool isManager { get; set; }
    public string department { get; set; }
    public string jobTitle { get; set; }
    public string access1Type { get; set; }
    public string access1Source { get; set; }
    public string access1DisplayName { get; set; }
    public string access1Description { get; set; }

}
