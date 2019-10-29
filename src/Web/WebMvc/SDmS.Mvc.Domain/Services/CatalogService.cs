using SDmS.Domain.Core.Interfases;
using SDmS.Domain.Core.Interfases.Services;
using System;
using System.Web.Mvc;

namespace SDmS.Domain.Services
{
    public class CatalogService : ICatalogService
    {
        public CatalogService()
        {

        }

        public SelectList GetComboBox<T>(IComboBox<T> comboBox, params object[] args) where T : class
        {
            return new SelectList(comboBox.GetEntities(), comboBox.DataValueField, comboBox.DataTextField);
        }
    }
}
