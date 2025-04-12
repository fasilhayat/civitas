# Civitas GraphQL Queries
### This file contains GraphQL queries for the Civitas project.

#### Based on your JSON sample and the Civitas project structure, here are the suggested GraphQL queries divided into logical categories:

 🔐 Access Level Permission (IAM Access Control)
```graphql
query GetAccessLevelPermissions($employeeId: ID!) {
  employee(id: $employeeId) {
    accessLevels {
      id
      name
      departmentId
      departmentName
      roles {
        id
        name
        thirdPartyId
      }
    }
  }
}
```

🏢 Organizational Information
```graphql
query GetOrganizationalInfo($employeeId: ID!) {
  employee(id: $employeeId) {
    employeeDepartment {
      departmentId
      departmentName
      thirdPartyId
    }
    manager {
      id
      firstName
      lastName
    }
    employment {
      jobTitleId
      jobTypeId
      costCenter
      workLocation
      staffCategory
      noticeTimeId
    }
  }
}
```

👤 Personal Information
```graphql
query GetPersonalInformation($employeeId: ID!) {
  employee(id: $employeeId) {
    person {
      id
      firstname
      lastname
      lastname2
      email
      alternateEmail
      gender
      birthdate
      personId
      nationality
      maritalStatus {
        maritalStatusId
        anniversarydate
      }
      educationLevelId
      phonePrivate {
        countryCode
        phone
      }
      phoneMobile {
        countryCode
        phone
      }
      address {
        addressline1
        adressline2
        zip
        city
        country
      }
      nextOfKin1 {
        name
        email
        phone
        relationId
      }
      nextOfKin2 {
        name
        email
        phone
        relationId
      }
    }
  }
}
```

💰 Salary Information
```graphql
query GetSalaryInformation($employeeId: ID!) {
  employee(id: $employeeId) {
    employment {
      salary
    }
    person {
      bankingAccount {
        bankRegistrationNumber
        bankAccountNumber
      }
    }
  }
}
```


📜 Working History
```graphql
query GetWorkingHistory($employeeId: ID!) {
  employee(id: $employeeId) {
    workHistory {
      id
      fromDate
      toDate
      titleId
      departmentId
      departmentName
      superiorId
      superiorName
      jobTypeId
      roleNames
      roleIds
      costCenter
      workLocation
      fte
      salary
      comments
      created
      lastUpdated
    }
  }
}
```
🪪 Access Key Card Data
This combines relevant fields used for generating an access card (e.g., name, photo, department, access roles).

```graphql
query GetAccessKeyCardData($employeeId: ID!) {
  employee(id: $employeeId) {
    person {
      firstname
      lastname
      personId
      photoUrl # Assuming this exists or can be resolved
    }
    employeeDepartment {
      departmentName
    }
    accessLevels {
      name
      roles {
        name
      }
    }
  }
}
```

### Based on your JSON sample and the Civitas project structure, here are the suggested GraphQL queries divided into logical categories:

🔐 Access Level Permission (IAM Access Control)
```graphql
query GetAccessLevelPermissions($employeeId: ID!) {
  employee(id: $employeeId) {
    accessLevels {
      id
      name
      departmentId
      departmentName
      roles {
        id
        name
        thirdPartyId
      }
    }
  }
}
```

🏢 Organizational Information
```graphql
query GetOrganizationalInfo($employeeId: ID!) {
  employee(id: $employeeId) {
    employeeDepartment {
      departmentId
      departmentName
      thirdPartyId
    }
    manager {
      id
      firstName
      lastName
    }
    employment {
      jobTitleId
      jobTypeId
      costCenter
      workLocation
      staffCategory
      noticeTimeId
    }
  }
}
```

👤 Personal Information
```graphql
query GetPersonalInformation($employeeId: ID!) {
  employee(id: $employeeId) {
    person {
      id
      firstname
      lastname
      lastname2
      email
      alternateEmail
      gender
      birthdate
      personId
      nationality
      maritalStatus {
        maritalStatusId
        anniversarydate
      }
      educationLevelId
      phonePrivate {
        countryCode
        phone
      }
      phoneMobile {
        countryCode
        phone
      }
      address {
        addressline1
        adressline2
        zip
        city
        country
      }
      nextOfKin1 {
        name
        email
        phone
        relationId
      }
      nextOfKin2 {
        name
        email
        phone
        relationId
      }
    }
  }
}
```

💰 Salary Information
```graphql
query GetSalaryInformation($employeeId: ID!) {
  employee(id: $employeeId) {
    employment {
      salary
    }
    person {
      bankingAccount {
        bankRegistrationNumber
        bankAccountNumber
      }
    }
  }
}
```

📜 Working History
```graphql
query GetWorkingHistory($employeeId: ID!) {
  employee(id: $employeeId) {
    workHistory {
      id
      fromDate
      toDate
      titleId
      departmentId
      departmentName
      superiorId
      superiorName
      jobTypeId
      roleNames
      roleIds
      costCenter
      workLocation
      fte
      salary
      comments
      created
      lastUpdated
    }
  }
}
```

🪪 Access Key Card Data
This combines relevant fields used for generating an access card (e.g., name, photo, department, access roles).

```graphql
query GetAccessKeyCardData($employeeId: ID!) {
  employee(id: $employeeId) {
    person {
      firstname
      lastname
      personId
      photoUrl # Assuming this exists or can be resolved
    }
    employeeDepartment {
      departmentName
    }
    accessLevels {
      name
      roles {
        name
      }
    }
  }
}
```

#### GraphQL queries in C# using a typical HttpClient and string-based approach. If you're using a library like GraphQL.Client, I can adapt it for that too—just let me know!

🧱 Setup (shared across all queries)
```csharp
public class GraphQLQuery
{
    public string Query { get; set; }
    public object Variables { get; set; }
}

public async Task<string> ExecuteGraphQLQueryAsync(string query, object variables)
{
    using var httpClient = new HttpClient();
    var request = new GraphQLQuery
    {
        Query = query,
        Variables = variables
    };

    var json = JsonSerializer.Serialize(request);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    // Replace with your actual Civitas GraphQL endpoint
    var response = await httpClient.PostAsync("https://civitas.mycompany.dk/graphql", content);
    response.EnsureSuccessStatusCode();

    return await response.Content.ReadAsStringAsync();
}
```

🔐 Access Level Permission
```csharp
string query = @"
query GetAccessLevelPermissions($employeeId: ID!) {
  employee(id: $employeeId) {
    accessLevels {
      id
      name
      departmentId
      departmentName
      roles {
        id
        name
        thirdPartyId
      }
    }
  }
}";

var result = await ExecuteGraphQLQueryAsync(query, new { employeeId = "550" });
```

🏢 Organizational Information
```csharp
string query = @"
query GetOrganizationalInfo($employeeId: ID!) {
  employee(id: $employeeId) {
    employeeDepartment {
      departmentId
      departmentName
      thirdPartyId
    }
    manager {
      id
      firstName
      lastName
    }
    employment {
      jobTitleId
      jobTypeId
      costCenter
      workLocation
      staffCategory
      noticeTimeId
    }
  }
}";

var result = await ExecuteGraphQLQueryAsync(query, new { employeeId = "550" });
```

👤 Personal Information
```csharp

string query = @"
query GetPersonalInformation($employeeId: ID!) {
  employee(id: $employeeId) {
    person {
      id
      firstname
      lastname
      lastname2
      email
      alternateEmail
      gender
      birthdate
      personId
      nationality
      maritalStatus {
        maritalStatusId
        anniversarydate
      }
      educationLevelId
      phonePrivate {
        countryCode
        phone
      }
      phoneMobile {
        countryCode
        phone
      }
      address {
        addressline1
        adressline2
        zip
        city
        country
      }
      nextOfKin1 {
        name
        email
        phone
        relationId
      }
      nextOfKin2 {
        name
        email
        phone
        relationId
      }
    }
  }
}";

var result = await ExecuteGraphQLQueryAsync(query, new { employeeId = "550" });
💰 Salary Information
```csharp
string query = @"
query GetSalaryInformation($employeeId: ID!) {
  employee(id: $employeeId) {
    employment {
      salary
    }
    person {
      bankingAccount {
        bankRegistrationNumber
        bankAccountNumber
      }
    }
  }
}";

var result = await ExecuteGraphQLQueryAsync(query, new { employeeId = "550" });
```

📜 Working History
```csharp
string query = @"
query GetWorkingHistory($employeeId: ID!) {
  employee(id: $employeeId) {
    workHistory {
      id
      fromDate
      toDate
      titleId
      departmentId
      departmentName
      superiorId
      superiorName
      jobTypeId
      roleNames
      roleIds
      costCenter
      workLocation
      fte
      salary
      comments
      created
      lastUpdated
    }
  }
}";
```
var result = await ExecuteGraphQLQueryAsync(query, new { employeeId = "550" });
🪪 Access Key Card Data
```csharp

string query = @"
query GetAccessKeyCardData($employeeId: ID!) {
  employee(id: $employeeId) {
    person {
      firstname
      lastname
      personId
      photoUrl
    }
    employeeDepartment {
      departmentName
    }
    accessLevels {
      name
      roles {
        name
      }
    }
  }
}";

var result = await ExecuteGraphQLQueryAsync(query, new { employeeId = "550" });
```