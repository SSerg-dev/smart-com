// ----------------------------------------------------------------------
// <copyright file="TimezoneModel.js" company="Смарт-Ком">
//     Copyright statement. All right reserved
// </copyright>
// Проект Transportation Workflow System
// Версия 2.0
// Автор: Максим Молоканов (EMail: maxim.molokanov@smart-com.su)
// ----------------------------------------------------------------------

Ext.define('App.model.core.timezone.TimezoneModel', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id' },
        { name: 'Name' },
        { name: 'Offset', type: 'int' }
	]
});