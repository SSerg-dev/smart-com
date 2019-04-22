// ----------------------------------------------------------------------
// <copyright file="AppComponent.js" company="Смарт-Ком">
//     Copyright statement. All right reserved
// </copyright>
// Проект SmartCom
// Версия 1.0
// Автор: Кириченко Ольга (EMail: olga.kirichenko@smart-com.su)
// ------------------------------------------------------------------------

Ext.define('App.AppComponent', {
    mixins: {
        observable: 'Ext.util.Observable'
    },

    constructor: function (config) {
        this.mixins.observable.constructor.call(this, config);
    }

});