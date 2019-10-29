using SDmS.Domain.Core.Interfases;
using SDmS.Domain.Core.Models.ComboBoxes;
using SDmS.Mvc.Infrastructure.Services;
using System;
using System.Collections.Generic;

namespace SDmS.Domain.Implementations.ComboBoxes
{
    public class DeviceTypesComboBox : IComboBox<DeviceType>
    {
        private string _baseUri;

        public DeviceTypesComboBox()
        {

        }

        public object Args
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string DataTextField => "description";

        public string DataValueField => "type";

        public bool IsArgsRequired => true;

        public IEnumerable<DeviceType> GetEntities()
        {
            //CommandFactory.Instance.GetCommand("BASE_GET_COMMAND").

            throw new NotImplementedException();
        }
    }
}
