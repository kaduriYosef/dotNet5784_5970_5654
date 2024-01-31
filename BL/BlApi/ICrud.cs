using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlApi;


public interface ICrud<T> : DalApi.ICrud<T> where T :class 
{
}
