// ----------------------------------------------------------------------
// <copyright file="Role.js" company="Смарт-Ком">
//     Copyright statement. All right reserved
// </copyright>
// Проект SmartCom
// Версия 1.0
// Автор: Кириченко Ольга (EMail: olga.kirichenko@smart-com.su)
// ------------------------------------------------------------------------

Ext.define('App.model.core.security.Role', {
    extend: 'Ext.data.Model',
    idProperty: 'Id',

    fields: [
        { name: 'Id', type: 'int' },
        { name: 'SystemName', type: 'string' },
        { name: 'DisplayName', type: 'string' },
        { name: 'IsAllow', type: 'boolean' }
    ]

});