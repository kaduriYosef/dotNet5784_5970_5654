using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;

namespace DalApi;

public interface ICrud<T> where T : class
{
    //Creates new entity object in DAL
    int Create(T item);

    //Reads entity object by its ID 
    T? Read(int id);

    //Reads entity object by some filter
    T? Read(Func<T, bool> filter); // stage 2

    //stage 1 only, Reads all entity objects
    IEnumerable<T?> ReadAll(Func<T, bool>? filter = null); // stage 2

    //Updates entity object
    void Update(T item);

    //Deletes an object by its Id
    void Delete(int id);




}
