namespace Civitas.Api.Infrastructure.Data;

public class DataKey : IDataKey
    {
        public DataKey(string identifier)
        {
            Identifier = identifier;
        }

        public string Identifier { get; init; }
    }

