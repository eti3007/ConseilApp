using System;
using System.Collections.Generic;

namespace ConseilApp.Builders.Interfaces
{
    public interface IMenuBuilder
    {
        string[] GetControllerAction(string pageName);
    }
}
