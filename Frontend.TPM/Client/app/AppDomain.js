// ----------------------------------------------------------------------
// <copyright file="AppDomain.js" company="Смарт-Ком">
//     Copyright statement. All right reserved
// </copyright>
// Проект SmartCom
// Версия 1.0
// Автор: Кириченко Ольга (EMail: olga.kirichenko@smart-com.su)
// ------------------------------------------------------------------------

Ext.define('App.AppDomain', {
    extend: 'Ext.app.EventDomain',
    singleton: true,
    type: 'app',
    idProperty: 'id',

    constructor: function () {
        this.callParent();
        this.monitor(App.AppComponent);
    }

});