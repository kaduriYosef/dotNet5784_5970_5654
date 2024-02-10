namespace BO;

/// <summary>
/// Exception for indicating that a requested entity does not exist within the business logic layer.
/// </summary>
[Serializable]
public class BlDoesNotExistException : Exception
{
    public BlDoesNotExistException(string? message) : base(message) { }
    public BlDoesNotExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception for indicating that an entity cannot be deleted, often due to existing dependencies or constraints.
/// </summary>
[Serializable]
public class BlImpossibleToDeleteException : Exception
{
    public BlImpossibleToDeleteException(string? message) : base(message) { }
    public BlImpossibleToDeleteException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception for indicating that an update operation is impossible to complete, possibly due to validation failures or data integrity issues.
/// </summary>
[Serializable]
public class BlImpossibleToUpdateException : Exception
{
    public BlImpossibleToUpdateException(string? message) : base(message) { }
    public BlImpossibleToUpdateException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when an attempt is made to create an entity that already exists, violating uniqueness constraints.
/// </summary>
[Serializable]
public class BlAlreadyExistException : Exception
{
    public BlAlreadyExistException(string? message) : base(message) { }
    public BlAlreadyExistException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception for indicating that provided data is invalid or does not meet the expected format or constraints.
/// </summary>
[Serializable]
public class BlInvalidDataException : Exception
{
    public BlInvalidDataException(string? message) : base(message) { }
    public BlInvalidDataException(string message, Exception innerException)
                : base(message, innerException) { }
}

/// <summary>
/// Exception thrown when a required property on an entity is null, but a non-null value is expected.
/// </summary>
[Serializable]
public class BlNullPropertyException : Exception
{
    public BlNullPropertyException(string? message) : base(message) { }
}
