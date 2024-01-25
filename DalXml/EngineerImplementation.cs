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
    readonly string s_engineers_xml = "engineers";

    //public int Create(Engineer item)
    //{
    //    List<Engineer> listEngineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);// this is root
    //    if (listEngineers.Any(engineer => engineer.Id == item.Id))
    //        throw new DalAlreadyExistsException($"Engineer with ID={item.Id} already exists");
    //    else
    //        listEngineers.Add(item);//insert to list
    //                                //return to XML file
    //    XMLTools.SaveListToXMLSerializer<Engineer>(listEngineers, s_engineers_xml);
    //    return item.Id;
    //}

    public int Create(Engineer item)
    {
        List<Engineer> engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);

        bool isNew = !engineers.Any(en => en.Id == item.Id);

        if (isNew)
            engineers.Add(item);

        XMLTools.SaveListToXMLSerializer<Engineer>(engineers, s_engineers_xml);

        return isNew ? item.Id : throw new DalAlreadyExistsException($"Engineer with Id={item.Id} already exist.\n");


    }

    public void Delete(int id)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        int index = Engineers.FindIndex((en) => en.Id == id);
        if (index != -1)
        {
            Engineers.RemoveAt(index);
        }
        else throw new DalDoesNotExistException($"there isn't an Engineer with Id={id}.\n");
        XMLTools.SaveListToXMLSerializer<Engineer>(Engineers, s_engineers_xml);

    }
    public Engineer? Read(Func<Engineer, bool> filter)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        if (filter == null) return null;
        return Engineers.FirstOrDefault(eng => filter(eng));

    }

    public Engineer? Read(int id)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
        return Engineers.FirstOrDefault(eng => eng.Id == id);
    }


    public IEnumerable<Engineer?> ReadAll(Func<Engineer, bool>? filter = null)
    {
        List<Engineer> Engineers1 = XMLTools.LoadListFromXMLSerializer<Engineer>(   s_engineers_xml);
        if (filter == null)
            return Engineers1.Select(item => item);
        else
            return Engineers1.Where(filter);
    }

    public void Update(Engineer item)
    {
        List<Engineer> Engineers = XMLTools.LoadListFromXMLSerializer<Engineer>(s_engineers_xml);
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
        XMLTools.SaveListToXMLSerializer<Engineer>(Engineers, s_engineers_xml);

    }
}