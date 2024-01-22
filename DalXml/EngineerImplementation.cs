using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dal;

using System;
using System.Collections.Generic;
using System.Data.Common;
using DalApi;
using DO;



internal class EngineerImplementation : IEngineer
{
    readonly string s_Engineer_xml = "engineer";
    

    public int Create(Engineer item)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_Engineer_xml);

        bool isNew = Engineers.Any(en => en.Id == item.Id);

        if (isNew) Engineers.Add(item);
        XMLTools.SaveListToXMLSerializer<Engineer>(Engineers, s_Engineer_xml); 
        
        return isNew ? item.Id : throw new DalAlreadyExistsException($"Engineer with Id={item.Id} already exist.\n");


    }

    public void Delete(int id)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_Engineer_xml);
        int index = Engineers.FindIndex((en) => en.Id == id);
        if (index != -1)
        {
            Engineers.RemoveAt(index);
        }
        else throw new DalDoesNotExistException($"there isn't an Engineer with Id={id}.\n");
        XMLTools.SaveListToXMLSerializer<Engineer>(Engineers, s_Engineer_xml);

    }
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_Engineer_xml);
        if (filter == null) return null;
        return Engineers.FirstOrDefault(eng => filter(eng));
        
    }

    public Engineer? Read(int id)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_Engineer_xml);
        return Engineers.FirstOrDefault(eng => eng.Id == id);
    }

    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_Engineer_xml);
        if (filter == null)
            return Engineers.Select(item => item);
        else
            return Engineers.Where(filter);
    }

    public void Update(Engineer item)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_Engineer_xml);
        int index = Engineers.FindIndex(en => en.Id == item.Id);

        if (index != -1)
        {
            Engineers.RemoveAt(index);
            Engineers.Add(item);
        }
        else
        {
            throw new DalDoesNotExistException($"An Engineer with id={item.Id} does not exist.\n");
        }
        XMLTools.SaveListToXMLSerializer<Engineer>(Engineers, s_Engineer_xml);
        
    }
}