// ----------------------------------------------------------------------
// <copyright file="CurrentUserRoles.js" company="Смарт-Ком">
//     Copyright statement. All right reserved
// </copyright>
// Проект SmartCom
// Версия 1.0
// Автор: Кириченко Ольга (EMail: olga.kirichenko@smart-com.su)
// ------------------------------------------------------------------------

Ext.define('App.store.core.security.CurrentUserRoles', {
    extend: 'Ext.data.Store',
    model: 'App.model.core.security.Role',
    alias: 'store.currentuserrolesstore',

    proxy: {
        type: 'rest',
        url: 'api/Security',

        reader: {
            type: 'json',
            root: 'data'
        }
    }

});