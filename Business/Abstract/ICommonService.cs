using System;
using System.Collections.Generic;
using System.Text;
using Core.Utilities.Results;
using Entities.Abstract;

namespace Business.Abstract
{
    public interface ICommonService
    {
       bool Contains(IEntity entity);
    }
}
