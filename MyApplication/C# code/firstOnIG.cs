
class AwesomeClass
{
    private static List<UserRecord> why = new List<UserRecord>();

public static void Load(string filePath)
{
    if (why == null)
        why = new List<UserRecord>();

    if (why.Count > 0)
        return;

    using var reader = new StreamReader(filePath);

    reader.ReadLine();

    string? line;

    while ((line = reader.ReadLine()) != null)
    {
        if (string.IsNullOrWhiteSpace(line))
            continue;

        string[] fields = line.Split(',');

        if (fields.Length < 13)
            continue; 

        UserRecord record = new UserRecord(
            fields[0],
            fields[1],
            fields[2],
            fields[3],
            fields[4].Trim().ToUpper() == "TRUE",
            fields[5],
            fields[6].Trim().ToUpper() == "TRUE",
            fields[7],
            fields[8],
            fields[9],
            fields[10],
            fields[11],
            fields[12]
        );

        why.Add(record);
    }
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
