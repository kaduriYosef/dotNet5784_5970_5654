namespace DO;

// Custom exceptions for Data Access Layer (DAL) operations, marked as [Serializable] for .NET applications.

[Serializable]
public class DalDoesNotExistException : Exception
{
    // Thrown when an operation is attempted on a non-existent DAL entity.
    public DalDoesNotExistException(string? message) : base(message) { }
}

[Serializable]
public class DalAlreadyExistsException : Exception
{
    // Thrown when trying to create or add a DAL entity that already exists.
    public DalAlreadyExistsException(string? message) : base(message) { }
}

[Serializable]
public class DalDeletionImpossible : Exception
{
    // Thrown when a deletion operation on a DAL entity is not possible.
    public DalDeletionImpossible(string? message) : base(message) { }
}

// These exceptions facilitate precise error handling in DAL operations.
