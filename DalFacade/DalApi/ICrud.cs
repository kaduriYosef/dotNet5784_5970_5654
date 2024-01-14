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

    //stage 1 only, Reads all entity objects
    List<T> ReadAll();

    //Updates entity object
    void Update(T item);

    //Deletes an object by its Id
    void Delete(int id);


}
